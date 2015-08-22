using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CTPPV5.TestLib
{
    public class ConcurrentRunner
    {
        private SemaphoreSlim semaphore;
        private int maxThread;
        private int loopEach;
        private AutoResetEvent signal;
        public ConcurrentRunner(int maxThread, int loopEach, AutoResetEvent signal = null)
        {
            this.maxThread = maxThread;
            this.loopEach = loopEach;
            this.semaphore = new SemaphoreSlim(0, maxThread);
            this.signal = signal;
        }

        public void Run(Action<int> action)
        {
            int readyCount = 0;
            int overCount = 0;
            for (int i = 0; i < maxThread; i++)
            {
                new Thread((state) =>
                    {
                        var val = Convert.ToInt32(state);
                        Interlocked.Increment(ref readyCount);
                        Console.WriteLine("thread -> {0} gets ready. total -> {1}", Thread.CurrentThread.ManagedThreadId, readyCount);
                        semaphore.Wait();
                        for (int j = 0; j < loopEach; j++)
                            action(val);
                          
                        Console.WriteLine("thread -> {0} runs an end. total left -> {1}", Thread.CurrentThread.ManagedThreadId, overCount);
                        Interlocked.Increment(ref overCount);
                    }).Start(i+1);
            }

            new Thread(() =>
                {
                    while (true)
                    {
                        if (Interlocked.CompareExchange(ref readyCount, 0, maxThread) == maxThread)
                        {
                            semaphore.Release(maxThread);
                        }
                        if (overCount == maxThread)
                        {
                            Console.WriteLine("Test over");
                            if (signal != null)
                                signal.Set();
                            break;
                        }
                    }
                }).Start();

            if (signal != null)
                signal.WaitOne();
        }
    }
}
