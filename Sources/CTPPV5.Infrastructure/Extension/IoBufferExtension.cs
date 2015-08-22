using Mina.Core.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class IoBufferExtension
    {
        public static byte[] GetArray(this IoBuffer buffer, int length)
        {
            if (length < 0)
                return new byte[0];
            var dst = new byte[length];
            buffer.Get(dst, 0, dst.Length);
            return dst;
        }

        public static byte[] GetRemainingArray(this IoBuffer buffer)
        {
            if (buffer.Remaining == 0)
                return new byte[0];
            var dst = new byte[buffer.Remaining];
            buffer.Get(dst, 0, buffer.Remaining);
            return dst;
        }
    }
}
