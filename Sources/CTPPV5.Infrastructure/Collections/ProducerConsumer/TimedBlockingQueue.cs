using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public class TimedBlockingQueue1<TItem> : IIndexedBlockingQueue<TItem>
    {
        private int capacity;
        private Dictionary<object, Node<TItem>> nodeMap;
        private Node<TItem> header;
        private Node<TItem> tail;
        private object syncRoot = new object();

        public TimedBlockingQueue1(int capacity)
        {
            this.capacity = capacity;
            this.nodeMap = new Dictionary<object, Node<TItem>>();
        }

        public int Count { get { return nodeMap.Count; } }

        public bool TryFind(object key, out TItem item)
        {
            var found = false;
            item = default(TItem);
            Node<TItem> node = null;
            if (nodeMap.TryGetValue(key, out node))
            {
                item = node.Item;
                found = true;
            }
            return found;
        }

        public bool TryRemove(object key, out TItem item)
        {
            lock (syncRoot)
            {
                var removeOk = false;
                item = default(TItem);
                Node<TItem> node = null;
                if (nodeMap.TryGetValue(key, out node))
                {
                    if (nodeMap.Count == 0)
                    {
                        header = tail = null;
                    }
                    else
                    {
                        if (key.Equals(header.Key))
                        {
                            header = header.Next;
                        }
                        else if (key.Equals(tail.Key))
                        {
                            tail.Previous.Next = null;
                            tail = tail.Previous;
                        }
                        else
                        {
                            node.Previous.Next = node.Next;
                            node.Next.Previous = node.Previous;
                        }
                    }
                    item = node.Item;
                    nodeMap.Remove(key);
                    removeOk = true;
                }
                return removeOk;
            }
        }

        public bool Offer(TItem item)
        {
            return Offer(Guid.NewGuid().ToString(), item);
        }

        public bool Offer(object key, TItem item)
        {
            lock (syncRoot)
            {
                var offerOk = false;
                if (nodeMap.Count == capacity) return false;
                if (!nodeMap.ContainsKey(key))
                {
                    var node = new Node<TItem>(key, item);
                    nodeMap[key] = node;
                    if (header == null)
                    {
                        header = tail = node;
                    }
                    else
                    {
                        var tmp = header;
                        header = node;
                        header.Next = tmp;
                        tmp.Previous = header;
                    }
                    Monitor.Pulse(syncRoot);
                    offerOk = true;
                }
                return offerOk;
            }
        }

        public TItem Take()
        {
            return Take(0);
        }

        public TItem Take(int afterSeconds)
        {
            var takenItem = default(TItem);
            while(true)
            {
                var waitTime = TimeSpan.FromMilliseconds(0);
                lock (syncRoot)
                {
                    if (nodeMap.Count == 0)
                    {
                        Monitor.Wait(syncRoot);
                        continue;
                    }
                    else
                    {
                        var tmp = tail;
                        var nextTimeout = tmp.LastTime.AddSeconds(afterSeconds);
                        if (nextTimeout <= DateTime.Now)
                        {
                            TryRemove(tmp.Key, out takenItem);
                        }
                        else waitTime = nextTimeout - DateTime.Now;
                    }
                }
                if (waitTime.TotalMilliseconds > 0) Thread.Sleep(waitTime);
                else break;
            }
            
            return takenItem;     
        }

        private class Node<T>
        {
            public Node(object key, T item)
            {
                this.Key = key;
                this.Item = item;
                this.LastTime = DateTime.Now;
            }

            public Node<TItem> Next { get; set; }
            public Node<TItem> Previous { get; set; }
            public object Key { get; private set; }
            public T Item { get; private set; }
            public DateTime LastTime { get; private set; }
        }
    }
}
