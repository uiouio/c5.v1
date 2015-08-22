using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class StringExtension
    {
        public static bool IsNumeric(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return false;

            for (int i = 0; i < val.Length; i++)
                if (val[i] < '0' || val[i] > '9') return false;

            return true;
        }

        public static string ToHex(this byte[] buffer)
        {
            if (buffer == null) return string.Empty;
            return buffer
                .Select(c => c.ToString("X2"))
                .Aggregate((p, n) => p + n)
                .ToLower();
        }

        public static byte[] FromHex(this string val)
        {
            if (string.IsNullOrEmpty(val)) return new byte[0];
            var buffer = new byte[val.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(val.Substring(i * 2, 2), 16);
            }

            return buffer;
        }

        public static string ToBase64(this byte[] buffer)
        {
            if (buffer == null) return string.Empty;
            return Convert.ToBase64String(buffer);
        }

        public static byte[] FromBase64(this string val)
        {
            if (string.IsNullOrEmpty(val)) return new byte[0];
            return Convert.FromBase64String(val);
        }
    }
}
