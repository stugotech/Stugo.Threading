using System;
using System.Threading.Tasks;

namespace Stugo.Threading.Async
{
    public sealed class AwaitableCriticalSection
    {
        private readonly AwaitableAutoResetEvent token = new AwaitableAutoResetEvent(true);


        public async Task<Token> EnterAsync()
        {
            await token.WaitAsync();
            return new Token(this, true);
        }


        public Token TryEnterAsync()
        {
            return new Token(this, token.TryWait());
        }


        public void Exit()
        {
            token.Set();
        }


        public class Token : IDisposable
        {
            private readonly AwaitableCriticalSection owner;

            public bool LockTaken { get; private set; }


            internal Token(AwaitableCriticalSection owner, bool lockTaken)
            {
                this.owner = owner;
                this.LockTaken = lockTaken;
            }


            public void Dispose()
            {
                if (LockTaken)
                    owner.Exit();
            }
        }
    }
}
