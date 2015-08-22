﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public class TimeoutNotifyProducerConsumer<TItem> : AbstractProducerConsumer<SequentialNode<TItem>, TimeoutNotifyQueue<TItem>>
    {
        private TimeoutNotifyQueue<TItem> blockingQueue;
        public TimeoutNotifyProducerConsumer(int capacity)
        {
            this.blockingQueue = new TimeoutNotifyQueue<TItem>(capacity);
        }

        public bool Produce(object key, TItem item, int timeout)
        {
            return blockingQueue.Offer(key, item, timeout);
        }

        public bool TryRemoveThoseWaitToTimeout(object key, out TItem item)
        {
            return blockingQueue.TryRemove(key, out item);
        }

        protected override TimeoutNotifyQueue<TItem> BlockingQueue
        {
            get { return blockingQueue; }
        }
 
        protected override IConsumingItem TakeConsumingItem()
        {
            var sequenceNode = blockingQueue.Take();
            var itemList = new List<TItem>();
            do
            {
                itemList.Add(sequenceNode.Item);
                sequenceNode = sequenceNode.Next;
            }
            while (sequenceNode != null);
            return new ChunkedConsumingItem<TItem>(itemList.ToArray());
        }
    }
}
