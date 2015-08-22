using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class AddrAssign
    {
        [ProtoMember(1)]
        public byte DestinationAddr { get; set; }
        [ProtoMember(2)]
        public byte AddrChangeTo { get; set; }
    }
}
