using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Filter.Codec;
using Mina.Core.Session;
using Mina.Filter.Codec.Demux;
using Mina.Core.Buffer;
using CTPPV5.Rpc.Serial.Packet;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Rpc.Serial
{
    public class PacketDecoder : IMessageDecoder
    {
        //the min length of both ctl packet and data packet is 7.
        private const int MIN_PACKET_LENGTH = 7;
        public MessageDecoderResult Decodable(IoSession session, IoBuffer input)
        {
            if (input.Remaining < MIN_PACKET_LENGTH)
                return MessageDecoderResult.NeedData;
            var symbol = input.GetArray(2);
            if (DataPacket.True(symbol))
            {
                input.Skip(1);
                var len = input.Get();
                input.Rewind();
                if (len > input.Remaining) return MessageDecoderResult.NeedData;
            }
            else if (!CtlPacket.True(symbol)) return MessageDecoderResult.NotOK;
            return MessageDecoderResult.OK;
        }

        public MessageDecoderResult Decode(IoSession session, IoBuffer input, IProtocolDecoderOutput output)
        {
            var instr = session.GetAttribute<IInstruction>(KeyName.INSTRUCTION);
            if (instr != null) output.Write(instr.CreateDecoder().Decode(input));
            return MessageDecoderResult.OK;
        }

        public void FinishDecode(IoSession session, IProtocolDecoderOutput output)
        {
        }
    }
}
