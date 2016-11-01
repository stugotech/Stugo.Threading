using System;

namespace Stugo.Threading.Dispatcher
{
    public interface IOperationDispatcher : IDisposable
    {
        void Start();
        void Enqueue(IOperation operation);
    }
}
