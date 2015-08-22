using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mina.Core.Buffer;
using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc.Net.Codec
{
    public class DuplexMessageEncoder : AbstractMessageEncoder<DuplexMessage>
    {
        protected override bool DoEncode(IoBuffer output, DuplexMessage message)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return scope.ResolveKeyed<IMessageWriter<DuplexMessage>>(message.Header.Version).Write(output, message);
            }
        }
    }
}
