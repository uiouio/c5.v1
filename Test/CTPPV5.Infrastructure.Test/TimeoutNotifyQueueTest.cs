using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
using CTPPV5.TestLib;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;

namespace CTPPV5.Rpc.Client.Test
{
    [TestFixture]
    public class TimeoutNotifyQueueTest
    {
        [Test]
        public void OfferTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 2000));
            }
            Assert.IsFalse(queue.Offer(10, 10, 2000));
            Assert.IsFalse(queue.Offer(1, 1, 2000));
            Assert.AreEqual(10, queue.Count);
        }

        [Test]
        public void RemoveTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 2000));
            }
            int val = 0;
            Assert.IsTrue(queue.TryRemove(1, out val));
            Assert.AreEqual(9, queue.Count);
            Assert.IsTrue(queue.Offer(1, 1, 2000));
            Assert.IsFalse(queue.Offer(11, 11, 2000));
            Assert.IsFalse(queue.TryRemove(11, out val));
            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.TryRemove(i, out val));
                Assert.AreEqual(i, val);
            }
            Assert.AreEqual(0, queue.Count);
            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 2000));
            }
            for (int i = 10; i >= 1; i--)
            {
                Assert.IsTrue(queue.TryRemove(i, out val));
                Assert.AreEqual(i, val);
            }
            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 2000));
            }
            Assert.IsTrue(queue.TryRemove(4, out val));
            Assert.AreEqual(4, val);
            Assert.IsTrue(queue.TryRemove(8, out val));
            Assert.AreEqual(8, val);
            Assert.IsTrue(queue.TryRemove(2, out val));
            Assert.AreEqual(2, val);
            Assert.IsTrue(queue.TryRemove(5, out val));
            Assert.AreEqual(5, val);
            Assert.IsTrue(queue.TryRemove(1, out val));
            Assert.AreEqual(1, val);
            Assert.IsTrue(queue.TryRemove(7, out val));
            Assert.AreEqual(7, val);
            Assert.IsTrue(queue.TryRemove(9, out val));
            Assert.AreEqual(9, val);
            Assert.IsTrue(queue.TryRemove(3, out val));
            Assert.AreEqual(3, val);
            Assert.IsTrue(queue.TryRemove(6, out val));
            Assert.AreEqual(6, val);
            Assert.IsTrue(queue.TryRemove(10, out val));
            Assert.AreEqual(10, val);
            Assert.IsFalse(queue.TryRemove(4, out val));
        }

        [Test]
        public void TakeTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 0));
            }
            System.Threading.Thread.Sleep(100);
            int j = 1;
            while (true)
            {
                if (queue.Count > 0)
                {
                    var node = queue.Take();
                    do
                    {
                        Assert.AreEqual(j, node.Key);
                        j++;
                        node = node.Next;
                    }
                    while (node != null);
                }
                else break;
            }
            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(11, j);

            for (int i = 1; i <= 10; i++)
            {
                Assert.IsTrue(queue.Offer(i, i, 0));
            }
            int val = 0;
            Assert.IsTrue(queue.TryRemove(7, out val));
            j = 1;
            while (true)
            {
                if (queue.Count > 0)
                {
                    var node = queue.Take();
                    do
                    {
                        Assert.AreEqual(j, node.Key);
                        if (++j == 7) j++;
                        node = node.Next;
                    }
                    while (node != null);
                }
                else break;
            }
            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(11, j);
        }

        [Test]
        public void TakeWaitTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            queue.Offer(1, 1, 1);
            var watch = Stopwatch.StartNew();
            queue.Take();
            watch.Stop();
            var elapsed = Convert.ToInt32(watch.Elapsed.TotalMilliseconds) / 1000;
            Assert.IsTrue(elapsed >= 0);
            Assert.IsTrue(elapsed <= 2);
            var are = new AutoResetEvent(false);
            new Task(() =>
            {
                watch = Stopwatch.StartNew();
                var i = queue.Take();
                Assert.AreEqual(10, i.Key);
                watch.Stop();
                elapsed = Convert.ToInt32(watch.Elapsed.TotalMilliseconds) / 1000;
                Assert.IsTrue(elapsed >= 1);
                Assert.IsTrue(elapsed <= 3);
                are.Set();
            }).Start();
            //total 3 secs
            System.Threading.Thread.Sleep(2000);
            queue.Offer(10, 10, 1);
            are.WaitOne();
        }

        [Test]
        public void OfferCauseSequenceChangedTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            queue.Offer(2, 2, 3);
            new Task(() =>
            {
                queue.Offer(1, 1, 1);
            }).Start();
            var node = queue.Take();
            Assert.AreEqual(1, node.Key);
            node = queue.Take();
            Assert.AreEqual(2, node.Key);
        }

        [Test]
        public void RemoveCauseSequenceChangedTest()
        {
            var queue = new TimeoutNotifyQueue<int>(10);
            queue.Offer(2, 2, 5);
            queue.Offer(3, 3, 1);
            new Task(() =>
            {
                int val = 0;
                queue.TryRemove(2, out val);
            }).Start();
            var watch = Stopwatch.StartNew();
            var node = queue.Take();
            watch.Stop();
            var elapsed = Convert.ToInt32(watch.Elapsed.TotalMilliseconds) / 1000;
            Assert.AreEqual(1, elapsed);
        }
    }
}
