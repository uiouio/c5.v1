using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Rpc.Net.Client;
using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Message.Filter;
using System.Net;
using CTPPV5.TestLib;
using CTPPV5.Rpc.Net.Message.Serializer;
using Mina.Core.Service;
using Mina.Transport.Socket;
using CTPPV5.Rpc.Net;
using System.Security.Cryptography;
using Mina.Core.Buffer;
using CTPPV5.Infrastructure.Util;
using System.Threading;
using CTPPV5.Infrastructure.Consts;
using Mina.Core.Filterchain;
using Mina.Filter.Logging;
using CTPPV5.Rpc.Net.Server;
using CTPPV5.CommandServer;
using Moq;
using CTPPV5.Models.CommandModel;
using CTPPV5.Repository;
using CTPPV5.Infrastructure.Security;
using System.Collections.Concurrent;
using CTPPV5.Models;

namespace CTPPV5.Rpc.Client.Test
{
    [TestFixture]
    public class RpcNetClientTest
    {
        private Process serverProcess;

        [TestFixtureSetUp]
        public void Setup()
        {
            Process.GetProcessesByName("CTPPV5.Rpc.Server.Sample").Foreach(p => p.Kill());
            new ClientInitializer().Init(new TestModule());
            var sampleFile = Directory.GetCurrentDirectory().Replace(@"Test\CTPPV5.Rpc.Client.Test\bin\Debug", @"Sample\CTPPV5.Rpc.Server.Sample\bin\Debug\CTPPV5.Rpc.Server.Sample.exe");
            serverProcess = Process.Start(sampleFile);
        }

        [Test]
        public void ComplexMessageTest()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var sessionFactory = scope.Resolve<AsyncSessionFactoryImpl>();
                var session = sessionFactory.OpenSession("127.0.0.1:8001");
                var command = session.CreateCommand(CommandCode.Test) as SampleCommand;
                command.SecurityEnabled = true;
                var customer1 = Customer.Invent();
                command.Parameter = customer1;
                var resultMessage = command.Run(2000);
                Assert.AreEqual(MessageState.Success, resultMessage.Header.State);
                Assert.AreEqual(ErrorCode.NoError, resultMessage.Header.ErrorCode);
                Assert.AreEqual(MessageFilterType.All, resultMessage.Header.FilterType);
                Assert.AreEqual(CommandCode.Test, resultMessage.Header.CommandCode);
                Assert.AreEqual(command.ID, resultMessage.Header.MessageID);
                Assert.AreEqual(SerializeMode.Protobuf, resultMessage.Header.SerializeMode);
                Assert.AreEqual(MessageVersion.V1, resultMessage.Header.Version);
                var customer2 = resultMessage.GetContent<Customer>();
                Assert.AreEqual(customer1.CustomerId, customer2.CustomerId);
                Assert.AreEqual(customer1.Birthday, customer2.Birthday);
                Assert.AreEqual(customer1.Contacts.Count, customer2.Contacts.Count);
                Assert.AreEqual(customer1.Contacts[1].CustomerId, customer2.Contacts[1].CustomerId);
            }
        }

        [Test]
        public void UncaughtExceptionTest()
        {
            var are = new AutoResetEvent(false);
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
            var connector = new AsyncSocketConnector();
            connector.FilterChain.AddLast("fireExceptiojn", new FireExceptionFilter(are));
            connector.FilterChain.AddLast("exceptionCounter", ObjectHost.Host.Resolve<ExceptionCounterFilter>());
            connector.FilterChain.AddLast("logger", new LoggingFilter());
            connector.SessionCreated += (s, e) =>
                {
                    e.Session.SetAttributeIfAbsent(KeyName.SESSION_ERROR_COUNTER, 0);
                };

            var future = connector.Connect(endPoint);
            future.Await();
            var session = future.Session;

            var guid = Guid.NewGuid().ToByteArray();
            var serverIdentifier = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("ctppv5_command_server")).ToHex();
            var buffer = ByteBufferAllocator.Instance.Allocate(48);
            buffer.AutoExpand = true;
            buffer.PutInt32(49);
            buffer.Put(1);
            buffer.PutInt16(1);
            buffer.Put(ErrorCode.NoError.ToByte());
            buffer.Put(MessageType.Command.ToByte());
            buffer.Put(Guid.NewGuid().ToByteArray());
            buffer.Put(guid);
            buffer.Put((MessageFilterType.Checksum).ToByte());
            buffer.Put(new byte[] { 0, 0 });
            buffer.Put(SerializeMode.None.ToByte());
            buffer.Position = 0;
            var header = buffer.GetArray(45);
            buffer.Put(BitConverter.GetBytes(Crc32.CalculateDigest(header, (uint)0, (uint)(header.Length))));
            buffer.Position = buffer.Position - 1; // make crc error
            buffer.Put(4);

            for (int i = 0; i < 6; i++)
            {
                buffer.Flip();
                session.Write(buffer);
                are.WaitOne();
            }

            while (true)
            {
                if (session.Connected)
                    Thread.Sleep(200);
                else break;
            }
        }

        [Test]
        public void RegisterAndAuthTest()
        {
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            bootstrap.AddFilterTypes(typeof(AuthenticationValidationFilter));
            bootstrap.StartUp(null, 
                new CommandServerModule(),
                new RepositoryModule(),
                new ClientModule(),
                new TestModule());

            //dont register client module for they are in the same context.
            //new ClientInitializer().Init(new TestModule());
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var factory = scope.Resolve<IAsyncSessionFactory<DuplexMessage>>();
                var session = factory.OpenSession(string.Format("{0};{1};", 
                    bootstrap.EndPoint.ToString(), "keepalive=false"));
                var command = session.CreateCommand(CommandCode.Register);
                command.SecurityEnabled = true;
                command.Parameter = new RegisterInfo { ClientMacAddr = NetworkInfoHelper.GetMacAddr(), ClientPubKey = new TestRsaKeyProvider().GetPublicKey(null) };
                var result = command.Run(5000);
                Assert.AreEqual(ErrorCode.NoError, result.Header.ErrorCode);
                Assert.AreEqual(command.ID, result.Header.MessageID);
                Assert.AreEqual(MessageType.Callback, result.Header.MessageType);

                command = session.CreateCommand(CommandCode.Authentication);
                command.Parameter = new AuthInfo
                {
                    Identifier = scope.Resolve<IIdentifierProvider>().GetIdentifier(),
                    Mac = NetworkInfoHelper.GetMacAddr()
                };
                command.SecurityEnabled = true;
                result = command.Run(5000);
                Assert.AreEqual(ErrorCode.NoError, result.Header.ErrorCode);
                Assert.AreEqual(command.ID, result.Header.MessageID);
                Assert.AreEqual(MessageType.Callback, result.Header.MessageType);

                command = session.CreateCommand(CommandCode.Test);
                result = command.Run(5000);
                Assert.AreEqual(CommandCode.Test, result.Header.CommandCode);
                Assert.AreEqual(ErrorCode.BadRequest, result.Header.ErrorCode); // test command not registered, it's ture that it returns bad request
            }
        }

        [Test]
        public void UnauthorizedCommandTest()
        {
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            bootstrap.AddFilterTypes(typeof(AuthenticationValidationFilter));
            bootstrap.StartUp(null, 
                new CommandServerModule(),
                new RepositoryModule(),
                new ClientModule(),
                new TestModule());

            //dont register client module for they are in the same context.
            //new ClientInitializer().Init(new TestModule());
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var factory = scope.Resolve<IAsyncSessionFactory<DuplexMessage>>();
                var session = factory.OpenSession(string.Format("{0};{1};",
                    bootstrap.EndPoint.ToString(), "keepalive=false"));
                var command = session.CreateCommand(CommandCode.Test);
                var result = command.Run(60000);
                Assert.AreEqual(CommandCode.Test, result.Header.CommandCode);
                Assert.AreEqual(ErrorCode.UnauthorizedCommand, result.Header.ErrorCode);
            }
        }

        [Test]
        public void ListConnectorsTest()
        {
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            bootstrap.AddFilterTypes(typeof(AuthenticationValidationFilter));
            bootstrap.StartUp(null,
                new CommandServerModule(),
                new RepositoryModule(),
                new ClientModule(),
                new TestModule(),
                new DefinedIdentifierModule());

            var command = GetRegisteredAndAuthedSession(bootstrap, true).CreateCommand(CommandCode.ListConnectors);
            var result = command.Run(5000);
            Assert.AreEqual(0, result.GetContent<List<string>>().Count);

            command = GetRegisteredAndAuthedSession(bootstrap, true).CreateCommand(CommandCode.ListConnectors);
            result = command.Run(5000);
            Assert.AreEqual(1, result.GetContent<List<string>>().Count);

            command = GetRegisteredAndAuthedSession(bootstrap, true).CreateCommand(CommandCode.ListConnectors);
            result = command.Run(5000);
            Assert.AreEqual(2, result.GetContent<List<string>>().Count);

            command = GetRegisteredAndAuthedSession(bootstrap, true).CreateCommand(CommandCode.ListConnectors);
            result = command.Run(5000);
            Assert.AreEqual(3, result.GetContent<List<string>>().Count);

            command = GetRegisteredAndAuthedSession(bootstrap, true).CreateCommand(CommandCode.ListConnectors);
            result = command.Run(5000);
            Assert.AreEqual(4, result.GetContent<List<string>>().Count);
        }

        private IAsyncSession<DuplexMessage> GetRegisteredAndAuthedSession(ServerBootstrap bootstrap, bool clearSessionCache = false)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var factory = scope.Resolve<IAsyncSessionFactory<DuplexMessage>>();
                var session = factory.OpenSession(string.Format("{0};{1};",
                    bootstrap.EndPoint.ToString(), "keepalive=false"));
                var command = session.CreateCommand(CommandCode.Register);
                command.SecurityEnabled = true;
                command.Parameter = new RegisterInfo { ClientMacAddr = NetworkInfoHelper.GetMacAddr(), ClientPubKey = new TestRsaKeyProvider().GetPublicKey(null) };
                var result = command.Run(5000);
                command = session.CreateCommand(CommandCode.Authentication);
                command.Parameter = new AuthInfo
                {
                    Identifier = scope.Resolve<IIdentifierProvider>().GetIdentifier(),
                    Mac = NetworkInfoHelper.GetMacAddr()
                };
                command.SecurityEnabled = true;
                result = command.Run(5000);

                if (clearSessionCache)
                {
                    var map = (factory as AsyncSessionFactoryImpl).AsTransparentObject().sessionMap as ConcurrentDictionary<string, IAsyncSession<DuplexMessage>>;
                    map.Clear();
                }

                return session;
            }
        }

        [TestFixtureTearDown]
        public void Down()
        {
            serverProcess.Kill();
            serverProcess.WaitForExit();
        }

        public class FireExceptionFilter : IoFilterAdapter
        {
            private AutoResetEvent are;
            public FireExceptionFilter(AutoResetEvent are)
            {
                this.are = are;
            }
            public override void MessageReceived(INextFilter nextFilter, Mina.Core.Session.IoSession session, object message)
            {
                are.Set();
                throw new Exception(); 
            }
        }

        public class DefinedIdentifierModule : Module
        {
            string[] identifiers = new string[]
            {
                Guid.NewGuid().ToByteArray().ToHex(),
                Guid.NewGuid().ToByteArray().ToHex(),
                Guid.NewGuid().ToByteArray().ToHex(),
                Guid.NewGuid().ToByteArray().ToHex(),
                Guid.NewGuid().ToByteArray().ToHex()
            };
            int count = 0;

            protected override void Load(ContainerBuilder builder)
            {
                var schoolCollection = new SchoolCollection();
                schoolCollection.Add(new School
                {
                    Identifier = identifiers[0],
                    ID = 1,
                    Name = "test1"
                });
                schoolCollection.Add(new School
                {
                    Identifier = identifiers[1],
                    ID = 2,
                    Name = "test2"
                });
                schoolCollection.Add(new School
                {
                    Identifier = identifiers[2],
                    ID = 3,
                    Name = "test3"
                });
                schoolCollection.Add(new School
                {
                    Identifier = identifiers[3],
                    ID = 4,
                    Name = "test4"
                });
                schoolCollection.Add(new School
                {
                    Identifier = identifiers[4],
                    ID = 5,
                    Name = "test5"
                });
                var metaRepoMock = new Mock<IMetaRepository>();
                metaRepoMock.Setup(r => r.GetAllSchools()).Returns(schoolCollection);
                metaRepoMock.Setup(r => r.AddOrUpdateSchool(
                        It.IsAny<School>(),
                        It.IsAny<Action<bool>>()))
                    .Callback<School, Action<bool>>((school, action) =>
                    {
                        new Task(() =>
                        {
                            action(true);
                        }).Start();
                    });

                var identifierMock = new Mock<IIdentifierProvider>();
                identifierMock.Setup(r => r.GetIdentifier()).Returns(identifiers[count++]);
                builder.Register(c => identifierMock.Object).As<IIdentifierProvider>();
                builder.Register(c => metaRepoMock.Object).As<IMetaRepository>();
            }
        }
    }
}
