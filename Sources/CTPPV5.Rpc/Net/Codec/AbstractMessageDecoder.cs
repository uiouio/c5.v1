using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mina.Filter.Codec.Demux;
using Mina.Core.Session;
using Mina.Core.Buffer;
using Mina.Filter.Codec;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure;

namespace CTPPV5.Rpc.Net.Codec
{
    public abstract class AbstractMessageDecoder : IMessageDecoder
    {
        private const int MESSAGE_LENGTH_BYTES_LENGTH = 4;
        private const int MAX_MESSAGE_LENGTH = 2 * 1024 * 1024;
        public MessageDecoderResult Decodable(IoSession session, IoBuffer input)
        {
            if (input.Remaining < MESSAGE_LENGTH_BYTES_LENGTH)
                return MessageDecoderResult.NeedData;
            var len = input.GetInt32();
            if (len > MAX_MESSAGE_LENGTH)
                return MessageDecoderResult.NotOK;
            if (input.Remaining + MESSAGE_LENGTH_BYTES_LENGTH < len)
                return MessageDecoderResult.NeedData;
            var version = input.Get().ToEnum<MessageVersion>();
            if (version == MessageVersion.BadVersion)
                return MessageDecoderResult.NotOK;
            if (len < FixedHeaderLength(version))
                return MessageDecoderResult.NotOK;

            return MessageDecoderResult.OK;
        }

        public MessageDecoderResult Decode(IoSession session, IoBuffer input, IProtocolDecoderOutput output)
        {
            int limit = input.Limit;
            int position = input.Position;
            var len = input.GetInt32();
            var version = input.Get();
            input.Position = position;
            input.Limit = input.Position + len;
            var buffer = input.Slice();
            input.Position = input.Limit;
            input.Limit = limit;
            var message = DoDecode(version.ToEnum<MessageVersion>(), buffer);
            if (message != null)
                output.Write(message);
            return MessageDecoderResult.OK;
        }

        public void FinishDecode(IoSession session, IProtocolDecoderOutput output)
        {
        }

        protected abstract int FixedHeaderLength(MessageVersion version);
        protected abstract object DoDecode(MessageVersion version, IoBuffer input);
    }
}
