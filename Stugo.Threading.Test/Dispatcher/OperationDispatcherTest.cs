using System;
using System.Threading.Tasks;
using Stugo.Threading.Dispatcher;
using Xunit;

namespace Stugo.Threading.Test.Dispatcher
{
    public class OperationDispatcherTest
    {
        [Fact]
        public async Task Enqueue_executes_operations()
        {
            var dispatcher = new OperationDispatcher();
            var operation = new TestOperation();

            dispatcher.Start();
            dispatcher.Enqueue(operation);
            await operation.Completion;

            Assert.Equal(1, operation.ExecutedCount);
        }


        [Fact]
        public void Start_throws_ObjectDisposedException_if_instance_is_disposed()
        {
            var dispatcher = new OperationDispatcher();
            dispatcher.Dispose();
            Assert.Throws<ObjectDisposedException>(() => dispatcher.Start());
        }
    }
}
