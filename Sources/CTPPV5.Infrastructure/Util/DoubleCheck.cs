using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Util
{
    public class DoubleCheckLock
    {
        private Locker @lock = new Locker(new object());
        private UnLocker @unlock = new UnLocker();
        public ILock Accquire(Func<bool> predicate)
        {
            ILock locker = null;
            if (predicate())
            {
                @lock.Lock();
                try
                {
                    if (predicate()) locker = @lock;
                    else
                    {
                        @lock.Dispose();
                        locker = @unlock;
                    }
                }
                catch (Exception)
                {
                    @lock.Dispose();
                }
            }
            else locker = @unlock;
            return locker;
        }
    }

    public interface ILock : IDisposable
    {
        bool Locked { get; }
        void Lock();
    }

    public class Locker : ILock
    {
        private object syncRoot;
        public Locker(object syncRoot)
        {
            this.syncRoot = syncRoot;
        }

        public void Lock()
        {
            Monitor.Enter(syncRoot);
        }

        public bool Locked { get { return true; } }

        public void Dispose()
        {
            Monitor.Exit(syncRoot);
        }
    }

    public class UnLocker : ILock
    {
        public bool Locked { get { return false; } }
        public void Lock() { }
        public void Dispose() { }
    }
}
