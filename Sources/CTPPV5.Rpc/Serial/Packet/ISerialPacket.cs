﻿using Mina.Core.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public interface ISerialPacket
    {
        IoBuffer GetBuffer();
    }
}
