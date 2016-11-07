using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stugo.Threading.Async
{
    public class AwaitableManualResetEvent
    {
        private volatile TaskCompletionSource<None> completionSource;


        public AwaitableManualResetEvent(bool initialState = false)
        {
            completionSource = new TaskCompletionSource<None>();

            if (initialState)
                completionSource.SetResult(None.Value);
        }


        public async Task WaitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            bool cancel = false;
            var cancelTaskSource = new TaskCompletionSource<None>();

            var reg = cancellationToken.Register(() =>
            {
                cancel = true;
                cancelTaskSource.SetResult(None.Value);
            });

            await Task.WhenAny(cancelTaskSource.Task, completionSource.Task);
            reg.Dispose();

            if (cancel)
                throw new OperationCanceledException();
        }


        public void Set()
        {
            completionSource.TrySetResult(None.Value);
        }


        public void Reset()
        {
            var tcs = completionSource;

            while (tcs.Task.IsCompleted &&
                tcs != Interlocked.CompareExchange(
                    ref completionSource, new TaskCompletionSource<None>(), tcs))
            {
                tcs = completionSource;
            }
        }
    }
}
