using System.Threading.Tasks;
using Moq;
using Stugo.Threading.Dispatcher;
using Xunit;

namespace Stugo.Threading.Test.Dispatcher
{
    public class DispatcherExtensionsTest
    {
        [Fact]
        public async Task WaitResult_schedules_the_operation_and_returns_the_result()
        {
            var dispatcherMock = new Mock<IOperationDispatcher>();
            var operation = new TestOperation();

            dispatcherMock.Setup(x => x.Enqueue(It.Is<TestOperation>(a => a == operation)));
            operation.Execute();

            var result = await dispatcherMock.Object.WaitResult(operation);
            Assert.Equal(42, result);
        }
    }
}