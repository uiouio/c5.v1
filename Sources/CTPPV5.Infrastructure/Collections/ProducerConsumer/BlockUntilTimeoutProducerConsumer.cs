using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public class BlockUntilTimeoutProducerConsumer1<TItem> : AbstractProducerConsumer<TItem, TimedBlockingQueue1<TItem>>
    {
        private const int ONLY_ONE_CONSUMER = 1;
        private int untiTimeout;
        private TimedBlockingQueue1<TItem> blockingQueue;
        public BlockUntilTimeoutProducerConsumer1(int capacity, int untiTimeout)
            : base(ONLY_ONE_CONSUMER)
        {
            this.untiTimeout = untiTimeout;
            this.blockingQueue = new TimedBlockingQueue1<TItem>(capacity);
        }

        public bool Produce(object key, TItem item)
        {
            return blockingQueue.Offer(key, item);
        }

        public bool TryRemoveThoseWaitToTimeout(object key, out TItem item)
        {
            return blockingQueue.TryRemove(key, out item);
        }

        protected override TimedBlockingQueue1<TItem> BlockingQueue
        {
            get { return blockingQueue; }
        }

        protected override IConsumingItem TakeConsumingItem()
        {
            return new SingleConsumingItem<TItem>(blockingQueue.Take(untiTimeout));
        }
    }
}
