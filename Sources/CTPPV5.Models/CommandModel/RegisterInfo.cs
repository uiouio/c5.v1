using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class RegisterInfo
    {
        [ProtoMember(1)]
        public string ClientMacAddr { get; set; }
        [ProtoMember(2)]
        public byte[] ClientPubKey { get; set; }
    }
}
