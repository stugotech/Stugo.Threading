using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stugo.Threading.Dispatcher
{
    public abstract class OperationBase : IOperation
    {
        private readonly TaskCompletionSource<None> completion = new TaskCompletionSource<None>();
        private int hasStarted;

        public Task Completion => completion.Task;
        public bool IsStarted => hasStarted != 0;
        public bool IsCompleted => Completion.IsCompleted || Completion.IsCanceled || Completion.IsFaulted;


        public void Execute()
        {
            if (Interlocked.CompareExchange(ref hasStarted, 1, 0) != 0)
                throw new InvalidOperationException("The operation has already been executed");

            StartAsyncOperation(() => completion.SetResult(None.Value), e => completion.SetException(e));
        }


        protected virtual void StartAsyncOperation(Action complete, Action<Exception> fail)
        {
            try
            {
                ExecuteOverride();
                complete();
            }
            catch (Exception e)
            {
                fail(e);
            }
        }


        protected virtual void ExecuteOverride()
        {
            throw new NotImplementedException();
        }
    }



    public abstract class OperationBase<TResult> : OperationBase, IOperation<TResult>
    {
        private TResult result;


        public TResult Result
        {
            get
            {
                if (!IsCompleted)
                    throw new InvalidOperationException("The operation has not been executed");
                return result;
            }
            protected set { result = value; }
        }
    }
}
