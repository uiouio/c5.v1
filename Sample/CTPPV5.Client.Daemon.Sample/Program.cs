using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Moq;
using Autofac;
using CTPPV5.Client.Daemon;
using CTPPV5.Infrastructure;
using CTPPV5.Client.Daemon.Command;
using CTPPV5.Client.Daemon.Instruction;
using CTPPV5.Models.CommandModel;

namespace CTPPV5.Client.Daemon.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationManager.AppSettings.Set("PortName", "COM3");
            ObjectHost.Setup(new Module[] { new DaemonModule(), new MockModule() });
            //RunCheckInQueryInstructionWithNoReply();
            //RunAddrAssignInstruction();
            RunSizeAllocInstruction();
            Console.ReadLine();
        }

        static void RunCheckInQueryInstructionWithNoReply()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var queryCmd = scope.Resolve<CheckInQueryCommand>();
                queryCmd.Execute();
            }
        }

        static void RunAddrAssignInstruction()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var addrAssignCmd = scope.Resolve<AddrAssignCommand>();
                addrAssignCmd.Execute(null, Rpc.Net.Message.DuplexMessage.CreateCommandMessage(
                    string.Empty,
                    Rpc.Net.Message.MessageVersion.V1,
                    Rpc.Net.Message.CommandCode.Test,
                    Rpc.Net.Message.Filter.MessageFilterType.None,
                    new byte[0],
                    Rpc.Net.Message.Serializer.SerializeMode.None,
                    new AddrAssign { DestinationAddr = 1, AddrChangeTo = 2 }));
            }
        }

        static void RunSizeAllocInstruction()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var sizeAlloc = new SizeAlloc { DestinationAddr = 1 };
                sizeAlloc.Add(new AllocUnit { Address = 11, Size = 70 });
                var addrAssignCmd = scope.Resolve<SizeAllocCommand>();
                addrAssignCmd.Execute(null, Rpc.Net.Message.DuplexMessage.CreateCommandMessage(
                    string.Empty,
                    Rpc.Net.Message.MessageVersion.V1,
                    Rpc.Net.Message.CommandCode.Test,
                    Rpc.Net.Message.Filter.MessageFilterType.None,
                    new byte[0],
                    Rpc.Net.Message.Serializer.SerializeMode.None,
                    sizeAlloc));
            }
        }
    }
}
