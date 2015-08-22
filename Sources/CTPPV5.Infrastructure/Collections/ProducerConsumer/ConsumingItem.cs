﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public class SingleConsumingItem<TItem> : IConsumingItem
    {
        public SingleConsumingItem(TItem item)
        {
            this.Item = item;
        }

        public TItem Item { get; set; }
    }

    public class ChunkedConsumingItem<TItem> : IConsumingItem
    {
        public ChunkedConsumingItem(TItem[] items)
        {
            this.Chunk = items;
        }
        public TItem[] Chunk { get; private set; }
    }
}
