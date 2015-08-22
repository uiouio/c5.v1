using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class TimeSyncDataPacket : DataPacket
    {
        public TimeSyncDataPacket(byte destinationAddr)
            : base(destinationAddr) { }

        protected override byte DataLength { get { return 7; } }

        protected override byte Protocol { get { return 0x04; } }

        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            buffer.Put(Convert.ToByte(DateTime.Now.Year % 100));
            buffer.Put(Convert.ToByte((byte)DateTime.Now.Month));
            buffer.Put(Convert.ToByte((byte)DateTime.Now.Day));
            buffer.Put(Convert.ToByte((byte)(DateTime.Now.DayOfWeek + 1)));
            buffer.Put(Convert.ToByte((byte)DateTime.Now.Hour));
            buffer.Put(Convert.ToByte((byte)DateTime.Now.Minute));
            buffer.Put(Convert.ToByte((byte)DateTime.Now.Second));
        }
    }
}
