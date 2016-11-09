using System.Threading;

namespace Stugo.Threading.Dispatcher
{
    public class DispatcherSynchronizationContext : SynchronizationContext
    {
        public IOperationDispatcher Dispatcher { get; }


        public DispatcherSynchronizationContext(IOperationDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }


        public override void Post(SendOrPostCallback callback, object state)
        {
            Dispatcher.Enqueue(new SendOrPostCallbackOperation(callback, state));
        }


        public override void Send(SendOrPostCallback callback, object state)
        {
            Dispatcher.Wait(new SendOrPostCallbackOperation(callback, state)).Wait();
        }
    }
}
