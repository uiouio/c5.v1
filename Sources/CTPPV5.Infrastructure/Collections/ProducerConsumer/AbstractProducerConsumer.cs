using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public abstract class AbstractProducerConsumer<TItem, TQueue> : IProducerConsumer<TItem, TQueue>
        where TQueue : IBlockingQueue<TItem>
    {
        private Thread pollingThread;
        private SemaphoreSlim semaphore;
        private const int ONLY_ONE_CONSUMER = 1;
        public AbstractProducerConsumer()
            : this(ONLY_ONE_CONSUMER) { }
        public AbstractProducerConsumer(int maxConsumerCount)
        {
            this.semaphore = new SemaphoreSlim(maxConsumerCount, maxConsumerCount);
            pollingThread = new Thread(InfinitePolling);
            pollingThread.IsBackground = true;
            pollingThread.Start();
        }

        public bool Produce(TItem item)
        {
            return BlockingQueue.Offer(item);
        }

        private void InfinitePolling()
        {
            while (true)
            {
                semaphore.Wait();
                try
                {
                    var item = TakeConsumingItem();
                    if (item != null)
                    {
                        ThreadPool.QueueUserWorkItem((state) =>
                        {
                            var consumingItem = state as IConsumingItem;
                            try
                            {
                                if (OnConsume != null)
                                    OnConsume(this, new ConsumeEventArgs(consumingItem, Log));
                            }
                            catch (Exception ex)
                            {
                                Log.Error(LogTitle.CONSUME_QUEUE_ITEM_ERROR, ex);
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }, item);
                    }
                    else semaphore.Release();
                }
                catch (Exception)
                {
                    semaphore.Release();
                }
            }
        }

        public event EventHandler<ConsumeEventArgs> OnConsume;
        protected abstract TQueue BlockingQueue { get; }
        protected abstract IConsumingItem TakeConsumingItem();
        private ILog Log { get; set; }
    }
}
