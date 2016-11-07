using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stugo.Threading.Async
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Run work on thread pool and resume on current synchronization context.
        /// </summary>
        public static async Task Offload(Func<Task> action,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // await here makes sure continuation is on ui thread (in case returned task is
            //  returned rather than awaited
            await Task.Factory.StartNew(
                action, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default
            ).Unwrap();
        }


        /// <summary>
        /// Run work on thread pool and resume on current synchronization context.
        /// </summary>
        public static async Task<TResult> Offload<TResult>(Func<Task<TResult>> action,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // await here makes sure continuation is on ui thread (in case returned task is
            //  returned rather than awaited
            return await Task.Factory.StartNew(
                action, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default
            ).Unwrap();
        }


        /// <summary>
        /// Run work on thread pool and resume on current synchronization context.
        /// </summary>
        public static async Task Offload(Action action,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // await here makes sure continuation is on ui thread (in case returned task is
            //  returned rather than awaited
            await Task.Factory.StartNew(
                action, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default
            );
        }


        /// <summary>
        /// Run work on thread pool and resume on current synchronization context.
        /// </summary>
        public static async Task<TResult> Offload<TResult>(Func<TResult> action,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // await here makes sure continuation is on ui thread (in case returned task is
            //  returned rather than awaited
            return await Task.Factory.StartNew(
                action, cancellationToken, TaskCreationOptions.DenyChildAttach,
                TaskScheduler.Default
            );
        }
    }
}
