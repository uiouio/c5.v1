using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CTPPV5.Models.CommandModel
{
    [ProtoContract]
    public class SizeAlloc
    {
        private IList<AllocUnit> units = new List<AllocUnit>();
        private IList<GradeProfileSyncUnit> profileUnits = new List<GradeProfileSyncUnit>();
        public void Add(AllocUnit unit)
        {
            units.Add(unit);
        }

        public void AddProfile(GradeProfileSyncUnit unit)
        {
            unit.DestinationAddr = DestinationAddr;
            profileUnits.Add(unit);
        }

        [ProtoMember(1)]
        public byte DestinationAddr { get; set; }

        [ProtoMember(2)]
        public IList<AllocUnit> Units
        {
            get { return units; }
            set { units = value; }
        }

        [ProtoMember(3)]
        public IList<GradeProfileSyncUnit> GradeUnits
        {
            get{ return profileUnits;}
            set{ profileUnits = value; }
        }
    }

    [ProtoContract]
    public class AllocUnit
    {
        [ProtoMember(1)]
        public byte Address { get; set; }
        [ProtoMember(2)]
        public byte Size { get; set; }
    }

    [ProtoContract]
    public class GradeProfileSyncUnit
    {
        private IList<ClassProfileSyncUnit> profileUnits = new List<ClassProfileSyncUnit>();
        [ProtoMember(1)]
        public byte Number { get; set; }
        [ProtoMember(2)]
        public IList<ClassProfileSyncUnit> ClassUnits
        {
            get { return profileUnits; }
            set { profileUnits = value; }
        }
        [ProtoMember(3)]
        public byte DestinationAddr { get; set; }

        public void AddProfile(ClassProfileSyncUnit unit)
        {
            profileUnits.Add(unit);
        }
    }

    [ProtoContract]
    public class ClassProfileSyncUnit
    {
        private byte[] nameBytes;
        [ProtoMember(1)]
        public int Number { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
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
