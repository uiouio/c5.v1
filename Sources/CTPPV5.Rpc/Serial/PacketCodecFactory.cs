using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Serial.Packet;
using Mina.Filter.Codec.Demux;

namespace CTPPV5.Rpc.Serial
{
    public class PacketCodecFactory : DemuxingProtocolCodecFactory
    {
        public PacketCodecFactory()
        {
            AddMessageDecoder<PacketDecoder>();
            AddMessageEncoder<ISerialPacket, PacketEncoder>();
        }
    }
}
