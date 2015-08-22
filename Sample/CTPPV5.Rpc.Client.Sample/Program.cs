using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Models.CommandModel;
using CTPPV5.Rpc.Net.Client;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.TestLib;
using System.Threading;

namespace CTPPV5.Rpc.Client.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            new ClientInitializer().Init(new SampleModule());
            //SingleThreadCommandPerfTest(10000);
            MultiThreadCommandPerfTest(10, 10000);
        }

        static void SingleThreadCommandPerfTest(int count)
        {
            
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var factory = scope.Resolve<IAsyncSessionFactory<DuplexMessage>>();
                var session = factory.OpenSession(string.Format("127.0.0.1:8001;keepalive=false;"));
                var command = session.CreateCommand(CommandCode.Heartbeat);
                var result = command.Run(5000);
                if (result.Header.State == MessageState.Success)
                {
                    var succCount = 0;
                    var watch = Stopwatch.StartNew();
                    for (int i = 0; i < count; i++)
                    {
                        command = session.CreateCommand(CommandCode.Heartbeat);
                        result = command.Run(5000);
                        if (result.Header.State == MessageState.Success) succCount++;
                    }
                    watch.Stop();
                    Console.WriteLine("single thread loop - {0} and elapsed - {1} and success - {2}", count, watch.ElapsedMilliseconds, succCount);
                }
            }
        }

        static void MultiThreadCommandPerfTest(int thread, int count)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var factory = scope.Resolve<IAsyncSessionFactory<DuplexMessage>>();
                var session = factory.OpenSession(string.Format("127.0.0.1:8001;keepalive=false;"));
                var command = session.CreateCommand(CommandCode.Heartbeat);
                var result = command.Run(5000);
                if (result.Header.State == MessageState.Success)
                {
                    var succCount = 0;
                    var failCount = 0;
                    var watch = Stopwatch.StartNew();
                    var are = new AutoResetEvent(false);
                    new ConcurrentRunner(thread, 1, are).Run((_) =>
                    {
                        for (int i = 0; i < count; i++)
                        {
                            command = session.CreateCommand(CommandCode.Heartbeat);
                            result = command.Run(5000);
                            if (result.Header.State == MessageState.Success) Interlocked.Increment(ref succCount);
                            else Interlocked.Increment(ref failCount);
                        }
                    });
                    watch.Stop();
                    Console.WriteLine("multi thread each loop - {0} and elapsed - {1} and success - {2} and fail - {3}", count, watch.ElapsedMilliseconds, succCount, failCount);
                    Console.ReadLine();
                }
            }
        }
    }
}
