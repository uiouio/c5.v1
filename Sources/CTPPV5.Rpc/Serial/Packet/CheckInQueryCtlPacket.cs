﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class CheckInQueryCtlPacket : CtlPacket
    {
        public CheckInQueryCtlPacket(byte destinationAddr)
            : base(destinationAddr) { }
        protected override byte Respond { get { return 0x02; } }
    }
}
