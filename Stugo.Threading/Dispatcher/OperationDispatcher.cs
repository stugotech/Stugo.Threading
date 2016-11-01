using System;
using System.Collections.Generic;
using System.Threading;

namespace Stugo.Threading.Dispatcher
{
    public class OperationDispatcher : IOperationDispatcher
    {
        private readonly object queueLock = new object();
        private readonly Queue<IOperation> queue = new Queue<IOperation>();
        private volatile bool disposed = false;
        private readonly Thread thread;


        public OperationDispatcher()
        {
            thread = new Thread(Process) { IsBackground = true };
        }


        public void Start()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(OperationDispatcher));
            thread.Start();
        }


        public void Enqueue(IOperation operation)
        {
            lock (queueLock)
            {
                queue.Enqueue(operation);
                Monitor.Pulse(queueLock);
            }
        }


        public void Dispose()
        {
            this.disposed = true;
        }


        private void Process()
        {
            while (!disposed)
            {
                Dequeue()?.Execute();
            }
        }


        private IOperation Dequeue()
        {
            lock (queueLock)
            {
                while (!disposed && queue.Count == 0)
                    Monitor.Wait(queueLock);

                return disposed ? null : queue.Dequeue();
            }
        }
    }
}
