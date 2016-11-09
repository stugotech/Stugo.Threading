using System;

namespace Stugo.Threading.Dispatcher
{
    public class DelegateOperation : OperationBase
    {
        private readonly Action action;


        public DelegateOperation(Action action)
        {
            this.action = action;
        }


        protected override void ExecuteOverride()
        {
            action();
        }
    }


    public class DelegateOperation<TResult> : OperationBase<TResult>
    {
        private readonly Func<TResult> action;


        public DelegateOperation(Func<TResult> action)
        {
            this.action = action;
        }


        protected override void ExecuteOverride()
        {
            Result = action();
        }
    }
}
