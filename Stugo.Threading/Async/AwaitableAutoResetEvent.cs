using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stugo.Threading.Async
{
    /// <summary>
    /// Implements an AutoResetEvent that can be awaited.
    /// </summary>
    public sealed class AwaitableAutoResetEvent
    {
        private static readonly Task completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();
        private readonly object mutex = new object();
        private bool signalled;


        public AwaitableAutoResetEvent(bool initialState = false)
        {
            signalled = initialState;
        }


        /// <summary>
        /// Waits asynchronously for the event to be signalled.
        /// </summary>
        public Task WaitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            lock (mutex)
            {
                if (signalled)
                {
                    signalled = false;
                    return completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource<bool>();
                    waiters.Enqueue(tcs);
                    cancellationToken.Register(() => tcs.TrySetCanceled());
                    return tcs.Task;
                }
            }
        }


        /// <summary>
        /// Returns true (and resets the event) immediately if the event is signalled; returns 
        /// false otherwise.
        /// </summary>
        public bool TryWait()
        {
            lock (mutex)
            {
                if (signalled)
                {
                    signalled = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// Sets the event to signalled.
        /// </summary>
        public void Set()
        {
            TaskCompletionSource<bool> current = null;

            lock (mutex)
            {
                if (waiters.Count > 0)
                    current = waiters.Dequeue();
                else if (!signalled)
                    signalled = true;
            }

            if (current != null)
                current.SetResult(true);
        }
    }
}
