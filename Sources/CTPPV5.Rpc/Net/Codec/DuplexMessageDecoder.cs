using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mina.Core.Buffer;
using Mina.Filter.Codec;
using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc.Net.Codec
{
    public class DuplexMessageDecoder : AbstractMessageDecoder
    {
        protected override int FixedHeaderLength(MessageVersion version)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return scope.ResolveKeyed<IMessageReader<DuplexMessage>>(version).HeaderLength;
            }
        }

        protected override object DoDecode(MessageVersion version, IoBuffer input)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return scope.ResolveKeyed<IMessageReader<DuplexMessage>>(version).Read(input);
            }
        }
    }
}
