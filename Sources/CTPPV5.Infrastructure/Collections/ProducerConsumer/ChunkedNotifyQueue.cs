﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    /// <summary>
    /// 多生产者单消费者阻塞队列。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChunkedNotifyQueue<TItem> : IChunkedBlockingQueue<TItem>
    {
        private readonly ConcurrentQueue<ItemWrapper<TItem>> queue;
        private readonly ManualResetEventSlim mre;
        private readonly int capacity;
        private readonly int reachCountToNotify;
        private readonly int timeoutToNotify;
        private int currentCount = 0;
        public ChunkedNotifyQueue(int capacity, int reachCountToNotify, int timeoutToNotify)
        {
            this.capacity = capacity;
            this.reachCountToNotify = reachCountToNotify;
            this.timeoutToNotify = timeoutToNotify;
            this.queue = new ConcurrentQueue<ItemWrapper<TItem>>();
            this.mre = new ManualResetEventSlim(false);
        }

        /// <summary>
        /// 将消息入队列，当消息数量达到指定值，通知消费者消费。
        /// </summary>
        /// <param name="item">消息</param>
        public bool Offer(TItem item)
        {
            int count = 0;
            var offerOk = false;
            if ((count = Interlocked.Increment(ref currentCount)) <= capacity)
            {
                queue.Enqueue(new ItemWrapper<TItem> { Value = item });
                if (count >= reachCountToNotify)
                {
                    if (!mre.IsSet) mre.Set();
                }
                offerOk = true;
            }
            else
            {
                Interlocked.Decrement(ref currentCount);
            }
            return offerOk;
        }

        public TItem Take()
        {
            var item = default(TItem);
            var items = TakeBatch(1);
            if (items.Length > 0) item = items[0];
            return item;
        }

        /// <summary>
        /// 拉取消息，只能单线程。
        /// </summary>
        /// <param name="maxCount">一次最大取数</param>
        /// <returns></returns>
        public TItem[] TakeBatch(int maxCount)
        {
            while (true)
            {
                mre.Wait(timeoutToNotify);
                TItem[] items = null;
                int queueCount = queue.Count;
                if (queueCount > 0)
                {
                    items = new TItem[queueCount > maxCount ? maxCount : queueCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        ItemWrapper<TItem> item = default(ItemWrapper<TItem>);
                        if (queue.TryDequeue(out item))
                        {
                            items[i] = item.Value;
                            item.Value = default(TItem);
                        }
                    }
                    Interlocked.Add(ref currentCount, -items.Length);
                    return items;
                }
                else
                {
                    Interlocked.Exchange(ref currentCount, 0);
                    if (mre.IsSet) mre.Reset();
                }
            }
        }

        /// <summary>
        /// for .net 4.0 bug
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        private struct ItemWrapper<T>
        {
            public T Value { get; set; }
        }
    }
}
