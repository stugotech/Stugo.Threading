using System;
using Stugo.Threading.Dispatcher;

namespace Stugo.Threading.Test.Dispatcher
{
    internal class TestOperation : OperationBase<int>
    {
        public bool ThrowException { get; set; }
        public int ExecutedCount { get; private set; }

        protected override void ExecuteInternal()
        {
            if (ThrowException)
                throw new Exception("ThrowException set");
            ExecutedCount++;
            Result = 42;
        }
    }
}
