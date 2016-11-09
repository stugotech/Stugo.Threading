using System.Threading;

namespace Stugo.Threading.Dispatcher
{
    public class SendOrPostCallbackOperation : OperationBase
    {
        private readonly SendOrPostCallback callback;
        private readonly object state;


        public SendOrPostCallbackOperation(SendOrPostCallback callback, object state)
        {
            this.callback = callback;
            this.state = state;
        }


        protected override void ExecuteOverride()
        {
            callback(state);
        }
    }
}
