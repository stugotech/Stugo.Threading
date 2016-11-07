using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stugo.Threading.Async
{
    public class AsyncMultiProducerSingleConsumerQueue<T>
    {
        private readonly Queue<T> operationQueue = new Queue<T>();
        private readonly object queueLock = new object();
        private TaskCompletionSource<T> waiter = null;
        private bool complete = false;


        public bool IsComplete { get { return complete; } }


        public void Enqueue(T entity)
        {
            CheckStatus();
            TaskCompletionSource<T> waiter = null;

            lock (queueLock)
            {
                if (this.waiter != null)
                {
                    waiter = this.waiter;
                    this.waiter = null;
                }
                else
                {
                    operationQueue.Enqueue(entity);
                }
            }

            // continue outisde the lock
            if (waiter != null)
                waiter.SetResult(entity);
        }


        public async Task<T> Dequeue()
        {
            TaskCompletionSource<T> waiter;

            lock (queueLock)
            {
                if (operationQueue.Count > 0)
                    return operationQueue.Dequeue();
                else if (this.waiter != null)
                    throw new InvalidOperationException("This collection does not support multiple consumers");
                else if (complete)
                    throw new TaskCanceledException("The queue is marked complete");
                else
                    this.waiter = waiter = new TaskCompletionSource<T>();
            }

            // waiter will always be set at this point
            return await waiter.Task;
        }


        public void Complete()
        {
            if (!complete)
            {
                TaskCompletionSource<T> waiter;

                lock (queueLock)
                {
                    complete = true;
                    waiter = this.waiter;
                }

                waiter?.TrySetCanceled();
            }
        }


        private void CheckStatus()
        {
            if (complete)
                throw new InvalidOperationException("The queue is marked complete");
        }
    }
}
