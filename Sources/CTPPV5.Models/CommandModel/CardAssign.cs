using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class CardAssign
    {
        private IList<CardAssignUnit> units = new List<CardAssignUnit>();
        [ProtoMember(1)]
        public IList<CardAssignUnit> Units
        {
            get { return units; }
            set { units = value; }
        }

        public void Add(CardAssignUnit unit)
        {
            units.Add(unit);
        }
    }

    [ProtoContract]
    public class CardAssignUnit
    {
        [ProtoMember(1)]
        public byte DestinationAddr { get; set; }
        [ProtoMember(2)]
        public int No { get; set; }
        [ProtoMember(3)]
        public int HolderNo { get; set; }
        [ProtoMember(4)]
        public byte Sequence { get; set; }
        [ProtoMember(5)]
        public HolderProfile Profile { get; set; }
    }

    [ProtoContract]
    public class HolderProfile
    {
        private byte[] nameBytes;

        [ProtoMember(1)]
        public byte DestinationAddr { get; set; }
        [ProtoMember(2)]
        public int No { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public byte Month { get; set; }
        [ProtoMember(5)]
        public byte Day { get; set; }

        public byte[] NameBytes
        {
            get
            {
                if (nameBytes == null)
                {
                    if (!string.IsNullOrEmpty(Name)) nameBytes = Encoding.GetEncoding("gb2312").GetBytes(Name);
                    else nameBytes = new byte[0];
                }
                return nameBytes;
            }
        }
    }
}
