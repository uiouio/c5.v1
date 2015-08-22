using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using Autofac;
using System.Diagnostics;
using Autofac.Core;
using Autofac.Features.LazyDependencies;
using CTPPV5.TestLib;
using System.Threading;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Metrics;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Infrastructure.Module;
using System.Net;
using System.Reflection;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Net.Client;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using Castle.DynamicProxy;
using Autofac.Extras.DynamicProxy2;
using CTPPV5.Infrastructure.Crosscutting;
using AutoMapper;
using CTPPV5.Models.DTO;
using System.Text;
using CTPPV5.Domain;
using CTPPV5.Infrastructure.Extension;
using System.Net.NetworkInformation;


namespace CTPPV5.Client
{
    static class Program
    {
        static System.IO.Ports.SerialPort port;
        static object root = new object();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            new Task(() =>
                {
                    lock (root)
                    {
                        Console.WriteLine("a-{0}", DateTime.Now);
                        if (Monitor.Wait(root, 2000))
                        {
                            Console.WriteLine("b-{0}", DateTime.Now);
                        }
                        else Console.WriteLine("c-{0}", DateTime.Now);
                    }
                }).Start();

            new Task(() =>
            {
                lock (root)
                {
                    Console.WriteLine("d");
                    Thread.Sleep(5000);
                    Console.WriteLine("e");
                }
            }).Start();

            Console.ReadLine();

            port = new System.IO.Ports.SerialPort("COM3", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            port.Open();
            while (true)
            {
                port.BaseStream.BeginWrite(new byte[] { 1, 1, 1 }, 0, 3, SendCallback, null);
                Thread.Sleep(200);
            }
            //Test9();
            //Test5();
            //Test4();
            //Test1();
            //Test2();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            byte @byte = 255;
            @byte += 1;
            @byte += 2;
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                port.BaseStream.EndWrite(ar);
            }
            catch (Exception ex)
            {

                return;
            }
        }

        static void Test1()
        {
            var rsa = new RSACryptoServiceProvider(2048);
            var pubKey = rsa.ExportCspBlob(false);
            var privateKey = rsa.ExportCspBlob(true);

            var b = new RSACrypto().Encrypt(pubKey, System.Text.Encoding.UTF8.GetBytes("hello."));
            var c = new RSACrypto().Decrypt(privateKey, b);
            string s = string.Empty;


            

            //var rsa1 = new RSACryptoServiceProvider(1024);
            //rsa1.ImportCspBlob(pubKey);
            //var secret = rsa1.Encrypt(System.Text.Encoding.UTF8.GetBytes("hello."), false);

            //var rsa2 = new RSACryptoServiceProvider(1024);
            //rsa2.ImportCspBlob(privateKey);
            //var plain = rsa2.Decrypt(secret, false);

        }

        static void Test2()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new ProtocalV1()).Keyed<IProtocal>(ProtocalVersion.V1);
            builder.Register(c => new ProtocalV2()).Keyed<IProtocal>(ProtocalVersion.V2);
            var container = builder.Build();
            var watch = Stopwatch.StartNew();
            var are = new AutoResetEvent(false);
            new ConcurrentRunner(10, 1, are).Run((_) =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        var protocal = container.BeginLifetimeScope().ResolveKeyed<IProtocal>(ProtocalVersion.V1);
                        string s = string.Empty;
                    }
                });
            watch.Stop();
            string sdd = string.Empty;
            

            //builder.RegisterType(typeof(Resource)).As<IResource>().InstancePerLifetimeScope();
            //var container = builder.Build();
            //var lifetime1 = container.BeginLifetimeScope();
            //var a1 = lifetime1.Resolve<IResource>();
            //var a2 = lifetime1.Resolve<IResource>();
            //Debug.Assert(a1 == a2);
            //var lifetime2 = container.BeginLifetimeScope();
            //var a3 = lifetime2.Resolve<IResource>();
            //var a4 = lifetime2.Resolve<IResource>();
            //var lifetime3 = lifetime2.BeginLifetimeScope();
            //var a5 = lifetime3.Resolve<IResource>();
            //Debug.Assert(a3 == a4);
            //var b = string.Empty;

            //builder.RegisterType(typeof(A)).As<IA>();
            //builder.RegisterType(typeof(B)).As<IB>();
            ////builder.Register((c, p) => new B(p.Named<string>("b"))).As<IB>();
            //builder.Register(c => new A(c.Resolve<Func<IB>>())).As<IA>().OnActivating(e => Console.WriteLine("1:{0}", e.GetType().Name)).OnActivated(e => Console.WriteLine("1:{0}", e.GetType().Name));
            //var container = builder.Build();
            //var d = container.Resolve<IA>();
            //var d1 = container.Resolve<IA>();
            //Debug.Assert(d == d1);
            //string s = string.Empty;
            //Debug.Assert(d.GetA.Equals("dd"));
        }

        static void Test3()
        {
            long i = 0;
            new Task(() =>
            {
                while (true)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(500);
                }
            }).Start();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new LogModule());
            builder.Register(c => new ProtocalV1()).As<IProtocal>().ExternallyOwned();
            var container = builder.Build();
            
            while (true)
            {
                var d = container.Resolve<IProtocal>();
                i++;
            }
            
            Console.WriteLine("dd");
            Console.ReadLine();
        }

        static void Test4()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new LogModule());
            builder.Register(c => new ProtocalV1()).As<IProtocal>().As<ITest>().ExternallyOwned();
            var container = builder.Build();
            var watch = Stopwatch.StartNew();
            IProtocal d = null;
            for (int i = 0; i < 1000000; i++)
            {
                d = container.Resolve<IProtocal>();
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
            string s = string.Empty;
        }

        static void Test5()
        {
            var data = System.IO.File.ReadAllBytes(@"E:\WorkItem\Dev\project\创智\ctppv5\CTPPV5.v12.suo");
            byte[] zipped = null;

            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);
            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            zipStream.PutNextEntry(new ZipEntry(Guid.NewGuid().ToString()));
            StreamUtils.Copy(new MemoryStream(data), zipStream, new byte[4096]);
            zipStream.CloseEntry();
            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.
            // Alternative outputs:
            // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
            byte[] byteArrayOut = outputMemStream.ToArray();

            ZipInputStream zipInputStream = new ZipInputStream(new MemoryStream(byteArrayOut));
            ZipEntry zipEntry = zipInputStream.GetNextEntry();

            byte[] b = null;
            while (zipEntry != null)
            {
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum


                var outStream = new MemoryStream();

                StreamUtils.Copy(zipInputStream, outStream, buffer);
                zipEntry = zipInputStream.GetNextEntry();

                b = outStream.ToArray();
            }
            string s = string.Empty;
        }

        static void Test6()
        {
            Metric.Config
                .WithHttpEndpoint("http://localhost:1234/")
                .WithReporting(report => report.WithConsoleReport(TimeSpan.FromSeconds(5)));

            
            var tag = new MetricTags(new string[]{"aa", "bb"});

            var meter = Metric.Context("Http").Meter("Error", Unit.Errors, TimeUnit.Seconds, tag);
            meter.Mark();
            meter.Mark();
            var meter2 = Metric.Context("Http2").Meter("Error", Unit.Errors, TimeUnit.Seconds);
            meter2.Mark();
            meter2.Mark();

        }

        static void Test7()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SomeType>()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(Log))
                   .As<ISomeType>();
            builder.Register(c => new AttributeLookup()).AsSelf().SingleInstance();
            builder.Register(c => new Log(c.Resolve<AttributeLookup>())).SingleInstance();

            var container = builder.Build();

            var watch = Stopwatch.StartNew();
            var some = container.Resolve<ISomeType>();
            some = container.Resolve<ISomeType>();
            for (int i = 0; i < 1000000;i++ )
                some.AddA("ff");
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
        }

        static void Test8()
        {
            
            Mapper.CreateMap<SchoolDTO, School>()
                .ForMember(s => s.ServerPrivateKey, m => m.MapFrom(d => Convert.FromBase64String(d.ServerPrivateKey)));

            var list = new List<SchoolDTO>();
            for (int i = 0; i < 1000000; i++)
            {
                var dto = new SchoolDTO();
                dto.ID = "1";
                dto.ServerPrivateKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("aa"));
                list.Add(dto);
            }

            Stopwatch watch = Stopwatch.StartNew();
            var dfff = Mapper.Map<IEnumerable<SchoolDTO>, IEnumerable<School>>(list);
            
            watch.Stop();
            string s2 = string.Empty;

    
           

            //var builder = new ContainerBuilder();
            //builder.Register((c, p) => new B(p.Named<string>("b"))).As<IB>();
            //builder.Register((c, p) => new A(c.Resolve<IB>()));
        }

        static void Test9()
        {
            Mapper.CreateMap<IEnumerable<SomeType>, SomeTypeCollection>()
                .AfterMap((src, dest) => src.ForEach(t => dest.Add(t)));

            var list = new List<SomeType>{new SomeType(), new SomeType()};

            var b = Mapper.Map<IEnumerable<SomeType>, SomeTypeCollection>(list);
        }

        static void Test10()
        {
            string macAddress = "";  
            try  
            {  
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();  
                foreach (NetworkInterface adapter in nics)  
                {  
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))  
                    {  
                        macAddress = adapter.GetPhysicalAddress().ToString();  
                        //for (int i = 1; i < 6; i++)  
                        //{  
                        //    macAddress = macAddress.Insert(3 * i - 1, ":");  
                        //}  
                        //break;  
                    }  
                }  
  
            }  
            catch  
            {  
            }  
        }
    }

    public class SomeTypeCollection
    {
        private List<SomeType> list = new List<SomeType>();
        public void Add(SomeType type) { list.Add(type); }
    }


    public class Log  : IInterceptor
    {
        private AttributeLookup lookUp;
        public Log(AttributeLookup lookUp)
        {
            this.lookUp = lookUp;
        }
        public void Write(string a)
        {
            string s = string.Empty;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            string s = string.Empty;
            var d2 = lookUp.IsDefined<SomeAttribute>(invocation.Method);
            invocation.Proceed();
            var d = invocation.ReturnValue;
            string s2 = string.Empty;
        }

        #endregion
    }

    public interface ISomeType
    {
        string AddA(string a);
        [Some]
        string AddB(string a);
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SomeAttribute : Attribute
    {
    
    }

    public class SomeType : ISomeType
    {
        [Some]
        public virtual string AddA(string a)
        {
            return a;
        }

        public virtual string AddB(string a)
        {
            return a;
        }
    }

    public enum ProtocalVersion
    {
        V1,
        V2
    }

    public interface IProtocal
    {
        void Parse();
    }

    public interface ITest { }

    public class ProtocalV1 : IProtocal, IDisposable, ITest
    {
        public void Parse()
        {
        }



        #region IDisposable Members

        public void Dispose()
        {

        }

        private ILog Log { get; set; }

        #endregion
    }

    public class ProtocalV2 : IProtocal
    {
        public void Parse()
        {
            
        }
    }

    public interface IResource { }

    public class Resource : IResource
    {
        byte[] bytes = new byte[1024 * 1024];
        public void Dispose()
        { }

      
    }

    public class Am : Autofac.Module
    {
 
    }
    public interface IA 
    {
        string GetA { get; }
    }
    public interface IB
    {
        string GetB { get; set; }
    }
    public class A : IA
    {
        private Func<IB> b;
        public A(Func<IB> b)
        {
            this.b = b;
        }

        public A(IB b)
        {
            string s = string.Empty;
        }

        public string GetA { get { return b().GetB; } }
    }
    public class B : IB
    {
        public B()
        {
            GetB = "fff";
        }
        public B(string b)
        {
            GetB = b;
        }
        public string GetB { get; set; }
    }
}
