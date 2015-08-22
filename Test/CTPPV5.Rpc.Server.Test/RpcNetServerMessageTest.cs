using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using NUnit.Framework;
using Autofac;
using Mina.Transport.Socket;
using Mina.Core.Future;
using Mina.Core.Buffer;
using Mina.Core.Service;
using Mina.Core.Session;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net.Message.Serializer;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net;
using CTPPV5.Rpc.Net.Server;
using CTPPV5.Infrastructure.Module;
using CTPPV5.Infrastructure.Security;
using CTPPV5.TestLib;

namespace CTPPV5.Rpc.Server.Test
{
    [TestFixture]
    public class RpcNetServerMessageTest
    {
        [Test]
        public void ErrorVersionTest()
        {
            ConfigurationManager.AppSettings.Set("ReadBufferSize", "4");
            var client = new SimpleTcpClient();
            var handler = client.GetFutureHandler();
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp());
            bootstrap.Server.Handler = handler;
            Assert.IsTrue(client.Connect(bootstrap.EndPoint));

            //version is wrong
            var buffer = ByteBufferAllocator.Instance.Allocate(5);
            buffer.PutInt32(5);
            buffer.Put(2);
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(1, handler.ErrorCount);

            //wrong fixed header length 
            buffer = ByteBufferAllocator.Instance.Allocate(5);
            buffer.PutInt32(5);
            buffer.Put(1);
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(2, handler.ErrorCount);
            client.Close();
        }

        [Test]
        public void NoChecksumNoBodyMessageTest()
        {
            var guid = Guid.NewGuid().ToByteArray();
            var identifier = Guid.NewGuid().ToByteArray();
            var client = new SimpleTcpClient();
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());;
            Assert.IsTrue(bootstrap.StartUp());
            var handler = client.GetFutureHandler(null, (a, b) =>
            {
                var message = b as DuplexMessage;
                Assert.AreEqual(MessageVersion.V1, message.Header.Version);
                Assert.AreEqual(identifier.ToHex(), message.Header.Identifier);
                Assert.AreEqual(Convert.ToBase64String(guid), message.Header.MessageID);
                Assert.AreEqual(MessageFilterType.None, message.Header.FilterType);
                Assert.AreEqual(2, message.Header.FilterCode.Length);
                Assert.AreEqual(0, message.Header.FilterCode[0]);
                Assert.AreEqual(0, message.Header.FilterCode[1]);
                Assert.AreEqual(SerializeMode.None, message.Header.SerializeMode);
                Assert.AreEqual(MessageState.Success, message.Header.State);
                Assert.AreEqual(ErrorCode.NoError, message.Header.ErrorCode);
            });
            bootstrap.Server.Handler = handler;
            Assert.IsTrue(client.Connect(bootstrap.EndPoint));
            var buffer = ByteBufferAllocator.Instance.Allocate(49);
            buffer.PutInt32(49);
            buffer.Put(1);
            buffer.PutInt16(1);
            buffer.Put(ErrorCode.NoError.ToByte());
            buffer.Put(MessageType.Command.ToByte());
            buffer.Put(identifier);
            buffer.Put(guid);
            buffer.Put(0);
            buffer.Put(new byte[] { 0, 0 });
            buffer.Put(0);
            buffer.Put(new byte[] { 0, 0, 0, 0 });
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(1, handler.RcvCount);
            //send again
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(2, handler.RcvCount);
            client.Close();
        }

        [Test]
        public void ChecksumAndGzipAndSerializeTest()
        {
            var client = new SimpleTcpClient();
            var identifier = Guid.NewGuid().ToByteArray();
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp());
            var handler= client.GetFutureHandler(null, (a, b) =>
            {
                var message = b as DuplexMessage;
                Assert.AreEqual(MessageVersion.V1, message.Header.Version);
                Assert.AreEqual(MessageFilterType.Checksum, message.Header.FilterType & MessageFilterType.Checksum);
                Assert.AreEqual(MessageFilterType.Compression, message.Header.FilterType & MessageFilterType.Compression);
                Assert.AreEqual(SerializeMode.Json, message.Header.SerializeMode);
                Assert.AreEqual(MessageState.Success, message.Header.State);
                Assert.AreEqual(ErrorCode.NoError, message.Header.ErrorCode);
                Assert.AreEqual(identifier.ToHex(), message.Header.Identifier);
                var obj = message.GetContent<Entity>();
                Assert.AreEqual("a1", obj.Name);
                Assert.AreEqual(2, obj.Nest.Age);
                Assert.AreEqual(Desc, obj.Task);
            });
            bootstrap.Server.Handler = handler;
            Assert.IsTrue(client.Connect(bootstrap.EndPoint));
            var buffer = ByteBufferAllocator.Instance.Allocate(48);
            buffer.AutoExpand = true;
            var body = new Json().Serialize(new Entity
            {
                Name = "a1",
                Nest = new Nest { Age = 2 },
                Task = Desc
            }).Zip();

            buffer.PutInt32(49 + body.Length);
            buffer.Put(1);
            buffer.PutInt16(1);
            buffer.Put(ErrorCode.NoError.ToByte());
            buffer.Put(MessageType.Command.ToByte());
            buffer.Put(identifier);
            buffer.Put(Guid.NewGuid().ToByteArray());
            buffer.Put((MessageFilterType.Checksum | MessageFilterType.Compression).ToByte());
            buffer.Put(new byte[] { 0, 0 });
            buffer.Put(SerializeMode.Json.ToByte());
            buffer.Position = 0;
            var header = buffer.GetArray(45);
            buffer.Put(BitConverter.GetBytes(Crc32.CalculateDigest(header.Concat(body), (uint)0, (uint)(header.Length + body.Length))));
            buffer.Put(body);
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(1, handler.RcvCount);
            //send again
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(2, handler.RcvCount);
            client.Close();
        }

        [Test]
        public void EnableAllMessageFilterTest()
        {
            var client = new SimpleTcpClient();
            var identifier = Guid.NewGuid().ToByteArray();
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp(null, new TestModule()));
            var handler = client.GetFutureHandler(null, (a, b) =>
            {
                var message = b as DuplexMessage;
                Assert.AreEqual(MessageVersion.V1, message.Header.Version);
                Assert.AreEqual(MessageFilterType.Checksum, message.Header.FilterType & MessageFilterType.Checksum);
                Assert.AreEqual(MessageFilterType.Compression, message.Header.FilterType & MessageFilterType.Compression);
                Assert.AreEqual(SerializeMode.Json, message.Header.SerializeMode);
                Assert.AreEqual(MessageState.Success, message.Header.State);
                Assert.AreEqual(ErrorCode.NoError, message.Header.ErrorCode);
                Assert.AreEqual(identifier.ToHex(), message.Header.Identifier);
                var obj = message.GetContent<Entity>();
                Assert.AreEqual("a1", obj.Name);
                Assert.AreEqual(2, obj.Nest.Age);
                Assert.AreEqual(Desc, obj.Task);
            });
            bootstrap.Server.Handler = handler;
            Assert.IsTrue(client.Connect(bootstrap.EndPoint));
            var buffer = ByteBufferAllocator.Instance.Allocate(48);
            buffer.AutoExpand = true;
            var pubKey = ObjectHost.Host.Resolve<ICryptoKeyProvider>().GetPublicKey(null);
            var body = new RSACrypto().Encrypt(pubKey, new Json().Serialize(new Entity
            {
                Name = "a1",
                Nest = new Nest { Age = 2 },
                Task = Desc
            }).Zip());

            buffer.PutInt32(49 + body.Length);
            buffer.Put(1);
            buffer.PutInt16(1);
            buffer.Put(ErrorCode.NoError.ToByte());
            buffer.Put(MessageType.Command.ToByte());
            buffer.Put(identifier);
            buffer.Put(Guid.NewGuid().ToByteArray());
            buffer.Put((MessageFilterType.Checksum | MessageFilterType.Compression | MessageFilterType.Crypto).ToByte());
            buffer.Put(new byte[] { 0, 0 });
            buffer.Put(SerializeMode.Json.ToByte());
            buffer.Position = 0;
            var header = buffer.GetArray(45);
            buffer.Put(BitConverter.GetBytes(Crc32.CalculateDigest(header.Concat(body), (uint)0, (uint)(header.Length + body.Length))));
            buffer.Put(body);
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(1, handler.RcvCount);
            //send again
            Assert.IsTrue(client.Send(buffer));
            Assert.AreEqual(2, handler.RcvCount);
            client.Close();
        }

        [Test]
        public void BadRequestResultTest()
        {
            var guid = Guid.NewGuid().ToByteArray();
            var identifier = Guid.NewGuid().ToByteArray();
            var client = new SimpleTcpClient();
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp());
            Assert.IsTrue(client.Connect(bootstrap.EndPoint));
            var buffer = ByteBufferAllocator.Instance.Allocate(48);
            buffer.AutoExpand = true;
            var body = new Json().Serialize(new Entity
            {
                Name = "a1",
                Nest = new Nest { Age = 2 },
                Task = Desc
            }).Zip();

            buffer.PutInt32(49 + body.Length);
            buffer.Put(1);
            buffer.PutInt16(1);
            buffer.Put(ErrorCode.NoError.ToByte());
            buffer.Put(MessageType.Command.ToByte());
            buffer.Put(identifier);
            buffer.Put(guid);
            buffer.Put((MessageFilterType.Checksum | MessageFilterType.Compression).ToByte());
            buffer.Put(new byte[] { 0, 0 });
            buffer.Put(SerializeMode.Json.ToByte());
            buffer.Position = 0;
            var header = buffer.GetArray(45);
            buffer.Put(BitConverter.GetBytes(Crc32.CalculateDigest(header.Concat(body), (uint)0, (uint)(header.Length + body.Length))));
            buffer.Position = buffer.Position - 1; // make crc error
            buffer.Put(4);
            buffer.Put(body);
            Assert.IsTrue(client.Send(buffer));
            var input = client.Receive();
            Assert.AreEqual(49, input.GetInt32());
            Assert.AreEqual(1, input.Get());
            Assert.AreEqual(CommandCode.BadRequest, input.GetInt16().ToEnum<CommandCode>());
            Assert.AreEqual(ErrorCode.DataBroken, input.Get().ToEnum<ErrorCode>());
            Assert.AreEqual(MessageType.CommandAck, input.Get().ToEnum<MessageType>());
            Assert.AreEqual(identifier.ToHex(), input.GetArray(16).ToHex());
            Assert.AreEqual(Convert.ToBase64String(guid), Convert.ToBase64String(input.GetArray(16)));
            Assert.AreEqual(MessageFilterType.Checksum, input.Get().ToEnum<MessageFilterType>());
            Assert.AreEqual(0, input.Get());
            Assert.AreEqual(0, input.Get());
            Assert.AreEqual(SerializeMode.None, input.Get().ToEnum<SerializeMode>());
            input.Position = 0;
            var header2 = input.GetArray(45);
            Crc32.VerifyDigest(BitConverter.ToUInt32(input.GetArray(4), 0), header2, 0, 45);

            Assert.IsTrue(client.Send(buffer));
            input = client.Receive();
            Assert.AreEqual(49, input.GetInt32());
            Assert.AreEqual(1, input.Get());
            Assert.AreEqual(CommandCode.BadRequest, input.GetInt16().ToEnum<CommandCode>());
            Assert.AreEqual(ErrorCode.DataBroken, input.Get().ToEnum<ErrorCode>());
            Assert.AreEqual(MessageType.CommandAck, input.Get().ToEnum<MessageType>());
            Assert.AreEqual(identifier.ToHex(), input.GetArray(16).ToHex());
            Assert.AreEqual(Convert.ToBase64String(guid), Convert.ToBase64String(input.GetArray(16)));
            Assert.AreEqual(MessageFilterType.Checksum, input.Get().ToEnum<MessageFilterType>());
            Assert.AreEqual(0, input.Get());
            Assert.AreEqual(0, input.Get());
            Assert.AreEqual(SerializeMode.None, input.Get().ToEnum<SerializeMode>());
            input.Position = 0;
            header2 = input.GetArray(45);
            Crc32.VerifyDigest(BitConverter.ToUInt32(input.GetArray(4), 0), header2, 0, 45);


            client.Close();
        }

        public class Entity
        {
            public string Name { get; set; }
            public Nest Nest { get; set; }
            public string Task { get; set; }
        }

        public class Nest
        {
            public int Age { get; set; }
        }

        public class TestModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.Register(c => new TestRsaKeyProvider()).As<ICryptoKeyProvider>().SingleInstance();
            }
        }

        


        private const string Desc = @"
随着各地政府不断规划新的园区、单个园区规模的快速扩大，园区经济也面临不少困难和挑战。例如为了吸引企业，各地的园区都推出了一系列优惠措施，其中包括提供免费的办公室、
降低税收标准、土地价格、给新创企业提供几十万元甚至几百万元的资金支持等等。但你有我有大家有，近几年这些优惠已经变成了“普惠”政策，对企业形成不了真正的吸引力，
一些企业甚至钻政策的空子，一个园区的优惠期限一到，就如候鸟一般飞往下一个园区，寻找新的补贴。类似的补贴效率不高，甚至导致了浪费，不利于园区发展和经济进步。
如果把云作为一种新的、吸引企业进驻园区的补贴和奖励，一定程度上可以解决上述问题，即给企业送云，让企业用上园区免费提供的云。这些云，不仅提供存贮、还可以提供
企业日常的行政管理功能、甚至计算的能力。这样的补贴，不仅更加有效，还可以增强企业对园区的粘性，把企业长期留在一个园区。给企业送云，还有助于解决园区发展过程中的其他一些问题。
例如，园区把企业请进来了，但几年过后，发现这些企业还是“群而不聚”，产生不了园区经济应该具有的聚合效应。对以装备制造业、高新技术产业、金融业为主的园区来说，云就是一个新的机遇，
通过云为企业提供统一的移动办公平台、客户关系管理系统，让这些企业栖息在同一片云上、管理在同一片云上，这可能推进企业聚合、催化集群效应。此外，给企业送云，也有利于推动“大众创业、万众创新”。
云将成为未来企业、甚至整个社会的基础设施，一个初创企业，如果一开始就可以免费获得云，省下购置服务器、架设机房的钱，就可以大大降低创业成本、增强企业的灵活性和竞争力。也就是说，给企业送云，
可以推进中国的中小企业、初创企业提前享受新的科技成果，在全社会降低创新、创业的门槛。如果把视野放到全球，我们就会发现，给企业送云，不仅可以、应该，而且十分必要。据国际知名机构IDG的分析，
欧美等发达国家目前占据了云服务市场的绝对主导地位，仅美国就拥有全球50%的市场份额；中国所占的市场份额为4%左右，可谓远远滞后。要改变这种格局，中国不仅仅需要在本国尽快普及云，
还要积极想办法帮助中国云走出国门、输出到其他国家去。当下，中国的云服务商正在花大脑筋、大力气图谋破局，政府的引导和园区的鼓励，将加速破冰、加快云计算在国内的普及。
就此而言，给企业送云，对园区、企业和中国新兴的云服务提供商，都是一个赢，即三赢。我呼吁，中国的地方政府作为园区建设的推动者、园区政策的制定者以及园区运行的管理者，
应该更新观念、大胆创新，通过给企业送云，落实国务院的意见，推动新一轮的园区转型和产业升级";
    }
}
