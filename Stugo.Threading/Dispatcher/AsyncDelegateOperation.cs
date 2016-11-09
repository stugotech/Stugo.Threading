using System;
using System.Threading.Tasks;

namespace Stugo.Threading.Dispatcher
{
    public class AsyncDelegateOperation : OperationBase
    {
        private readonly Func<Task> action;


        public AsyncDelegateOperation(Func<Task> action)
        {
            this.action = action;
        }


        protected override async void StartAsyncOperation(Action complete, Action<Exception> fail)
        {
            try
            {
                await action();
                complete();
            }
            catch (Exception e)
            {
                fail(e);
            }
        }
    }


    public class AsyncDelegateOperation<TResult> : OperationBase<TResult>
    {
        private readonly Func<Task<TResult>> action;


        public AsyncDelegateOperation(Func<Task<TResult>> action)
        {
            this.action = action;
        }


        protected override async void StartAsyncOperation(Action complete, Action<Exception> fail)
        {
            try
            {
                Result = await action();
                complete();
            }
            catch (Exception e)
            {
                fail(e);
            }
        }
    }
}
