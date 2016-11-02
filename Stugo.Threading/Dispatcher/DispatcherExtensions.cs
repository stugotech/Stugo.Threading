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


        public static async Task<TResult> WaitResult<TResult>(this IOperationDispatcher dispatcher, 
            IOperation<TResult> operation)
        {
            await Wait(dispatcher, operation);
            return operation.Result;
        }
    }
}
