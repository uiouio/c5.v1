using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Serializer
{
    public interface ISerializer
    {
        byte Code { get; }
        byte[] Serialize(object value);
        T DeSerialize<T>(byte[] buffer);
    }
}
