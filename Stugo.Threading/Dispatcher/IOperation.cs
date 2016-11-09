using System.Threading.Tasks;

namespace Stugo.Threading.Dispatcher
{
    public interface IOperation
    {
        void Execute();
        Task Completion { get; }
        bool IsStarted { get; }
        bool IsCompleted { get; }
    }


    public interface IOperation<out TResult> : IOperation
    {
        TResult Result { get; }
    }
}
