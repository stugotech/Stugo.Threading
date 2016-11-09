using System;
using System.Threading.Tasks;

namespace Stugo.Threading.Dispatcher
{
    public static class DispatcherExtensions
    {
        public static async Task Wait(this IOperationDispatcher dispatcher, IOperation operation)
        {
            dispatcher.Enqueue(operation);
            await operation.Completion;
        }


        public static async Task Wait(this IOperationDispatcher dispatcher, Action operation)
        {
            await dispatcher.Wait(new DelegateOperation(operation));
        }


        public static async Task Wait(this IOperationDispatcher dispatcher, Func<Task> operation)
        {
            await dispatcher.Wait(new AsyncDelegateOperation(operation));
        }


        public static async Task<TResult> WaitResult<TResult>(this IOperationDispatcher dispatcher,
            IOperation<TResult> operation)
        {
            await Wait(dispatcher, operation);
            return operation.Result;
        }


        public static async Task<TResult> WaitResult<TResult>(this IOperationDispatcher dispatcher,
            Func<TResult> operation)
        {
            return await dispatcher.WaitResult(new DelegateOperation<TResult>(operation));
        }


        public static async Task<TResult> WaitResult<TResult>(this IOperationDispatcher dispatcher,
            Func<Task<TResult>> operation)
        {
            return await dispatcher.WaitResult(new AsyncDelegateOperation<TResult>(operation));
        }
    }
}
