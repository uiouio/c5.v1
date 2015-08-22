using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class TimeSync
    {
        [ProtoMember(1)]
        public byte[] DestinationAddrs { get; set; }
    }
}
