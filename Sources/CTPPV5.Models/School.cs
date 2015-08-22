using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class School
    {
        public int ID { get; set; }
        public int AreaID { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public byte[] ServerPrivateKey { get; set; }
        public byte[] ServerPubKey { get; set; }
        public byte[] ClientPubKey { get; set; }
        public string UniqueToken { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is School && Equals((School)obj);
        }

        public bool Equals(School other)
        {
            return this.ID == other.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
