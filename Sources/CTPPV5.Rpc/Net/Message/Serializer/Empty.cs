using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Serializer
{
    public class Empty : ISerializer
    {
        static Empty serializer = new Empty();
        public byte Code
        {
            get { return Convert.ToByte(SerializeMode.None); }
        }

        public byte[] Serialize(object value)
        {
            return new byte[0];
        }

        public T DeSerialize<T>(byte[] buffer)
        {
            return default(T);
        }

        public static Empty Instance { get { return serializer; } }
    }
}
