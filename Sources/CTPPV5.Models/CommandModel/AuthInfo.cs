using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class AuthInfo
    {
        [ProtoMember(1)]
        public string Identifier { get; set; }
        [ProtoMember(2)]
        public string Mac { get; set; }
    }
}
