using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class Crc32Filter : IMessageFilter
    {
        public FilterResult In(IMessageDataContainer container)
        {
            var result = new FilterResult { OK = true };
            var checksum = BitConverter.ToUInt32(container.Take(), 0);
            var header = container.Take();
            var content = container.Take();
            var data = header.Concat(content);
            if (Crc32.VerifyDigest(checksum, header.Concat(content), (uint)0, (uint)data.Length))
            {
                container.Push(content);
            }
            else result = new FilterResult { Error = ErrorCode.DataBroken };
            return result;
        }

        public FilterResult Out(MessageHeader header, IMessageDataContainer container)
        {
            var headerBuffer = container.Take();
            var bodyBuffer = container.Take();
            var checkSum = BitConverter.GetBytes(
                Crc32.CalculateDigest(headerBuffer.Concat(bodyBuffer), 
                (uint)0, 
                (uint)(headerBuffer.Length + bodyBuffer.Length)));
            container.Push(checkSum);
            return new FilterResult { OK = true };
        }
    }
}
