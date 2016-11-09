using System;
using System.Linq;
using Xunit;

namespace Stugo.Threading.Test.Dispatcher
{
    public class OperationBaseTest
    {
        [Fact]
        public void Execute_completes_the_task_after_execution()
        {
            var operation = new TestOperation();

            Assert.False(operation.Completion.IsCompleted);
            Assert.False(operation.IsStarted);
            operation.Execute();
            Assert.True(operation.Completion.IsCompleted);
            Assert.True(operation.IsStarted);
        }


        [Fact]
        public void Execute_runs_ExecuteInternal_once()
        {
            var operation = new TestOperation();

            operation.Execute();
            Assert.Equal(1, operation.ExecutedCount);
        }


        [Fact]
        public void Execute_throws_InvalidOperationException_if_called_twice()
        {
            var operation = new TestOperation();
            operation.Execute();
            Assert.Throws<InvalidOperationException>(() => operation.Execute());
            Assert.Equal(1, operation.ExecutedCount);
        }


        [Fact]
        public void Result_throws_InvalidOperationException_if_accessed_before_Execute()
        {
            var operation = new TestOperation();
            Assert.Throws<InvalidOperationException>(() => operation.Result);
            operation.Execute();
            Assert.Equal(42, operation.Result);
        }


        [Fact]
        public void Completion_has_exception_set_if_exception_is_thrown_by_ExecuteInternal()
        {
            var operation = new TestOperation { ThrowException = true };
            operation.Execute();
            Assert.True(operation.Completion.IsFaulted);
            var exception = operation.Completion.Exception?.InnerExceptions?.FirstOrDefault();
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
            Assert.Equal("ThrowException set", exception.Message);
        }
    }
}
