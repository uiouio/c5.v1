﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public interface IProducerConsumer<TItem, in TQueue>
        where TQueue : IBlockingQueue<TItem>
    {
        bool Produce(TItem item);
        event EventHandler<ConsumeEventArgs> OnConsume;
    }
}
