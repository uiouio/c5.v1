using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Collections.ProducerConsumer
{
    public interface IIndexedBlockingQueue<TItem> : IBlockingQueue<TItem>
    {
        bool TryFind(object key, out TItem item);
        bool TryRemove(object key, out TItem item);
        bool Offer(object key, TItem item);
    }
}
