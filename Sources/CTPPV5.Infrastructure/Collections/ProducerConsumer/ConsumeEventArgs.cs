using CTPPV5.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public class ConsumeEventArgs : EventArgs
    {
        public ConsumeEventArgs(IConsumingItem item, ILog log)
        {
            this.ConsumingItem = item;
            this.Log = log;
        }

        public IConsumingItem ConsumingItem { get; private set; }
        public ILog Log { get; private set; }
    }
}
