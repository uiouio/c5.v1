using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Net.Message.Serializer
{
    public class Protobuf : ISerializer
    {
        #region ISerializer Members

        public byte Code
        {
            get { return SerializeMode.Protobuf.ToByte(); }
        }

        public byte[] Serialize(object value)
        {
            if (value == null) return new byte[0];
            using (var stream = new MemoryStream())
            {
                RuntimeTypeModel.Default.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public T DeSerialize<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return default(T);
            using (var stream = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        #endregion
    }
}
