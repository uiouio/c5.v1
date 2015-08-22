using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CTPPV5.Rpc.Net.Message.Serializer
{
    public class Json : ISerializer
    {
        public byte Code { get { return Convert.ToByte(SerializeMode.Json); } }
        public byte[] Serialize(object value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        }

        public T DeSerialize<T>(byte[] buffer) 
        {
            T val = default(T);
            if (buffer != null && buffer.Length > 0)
                val = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(buffer));
            return val;
        }
    }
}
