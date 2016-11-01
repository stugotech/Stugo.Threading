using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stugo.Threading.Dispatcher
{
    public abstract class OperationBase : IOperation
    {
        private readonly TaskCompletionSource<None> completion = new TaskCompletionSource<None>();
        private int hasExecuted;

        public Task Completion => completion.Task;
        public bool HasExecuted => hasExecuted != 0;


        public void Execute()
        {
            if (Interlocked.CompareExchange(ref hasExecuted, 1, 0) != 0)
                throw new InvalidOperationException("The operation has already been executed");

            try
            {
                ExecuteInternal();
                completion.SetResult(None.Value);
            }
            catch (Exception e)
            {
                completion.SetException(e);
            }
        }


        protected abstract void ExecuteInternal();
    }



    public abstract class OperationBase<TResult> : OperationBase
    {
        private TResult result;


        public TResult Result
        {
            get
            {
                if (!HasExecuted)
                    throw new InvalidOperationException("The operation has not been executed");
                return result;
            }
            protected set { result = value; }
        }
    }
}
