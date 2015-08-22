using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class UserCancel
    {
        [ProtoMember(1)]
        public byte DestinationAddr { get; set; }
        [ProtoMember(2)]
        public int UserNo { get; set; }
    }
}
