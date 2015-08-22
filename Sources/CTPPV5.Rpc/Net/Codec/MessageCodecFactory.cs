using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Filter.Codec.Demux;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc.Net.Codec
{
    public class MessageCodecFactory : DemuxingProtocolCodecFactory
    {
        public MessageCodecFactory()
        {
            AddMessageDecoder<DuplexMessageDecoder>();
            AddMessageEncoder<DuplexMessage, DuplexMessageEncoder>();
        }
    }
}
