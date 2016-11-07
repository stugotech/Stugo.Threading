using Stugo.Threading.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stugo.Threading.Test.Async
{
    public class AsyncMultiProducerSingleConsumerQueueTest
    {
        [Fact]
        public async Task It_can_deal_with_load()
        {
            const int iterations = 100000;
            const int threadCount = 10;
            const long n = iterations * threadCount;

            var random = new Random();
            var queue = new AsyncMultiProducerSingleConsumerQueue<int>();
            var go = new AwaitableManualResetEvent();
            var received = new List<long>();
            var threads = new List<Task>();

            // producers
            for (int thread = 0; thread < threadCount; ++thread)
            {
                var threadCopy = thread;

                threads.Add(Task.Run(async () =>
                {
                    await go.WaitAsync();

                    for (int i = 0; i < iterations; ++i)
                    {
                        queue.Enqueue(threadCopy * iterations + i);

                        // don't do it all in one go to allow interleaving
                        if (random.NextDouble() < 0.2d)
                            await Task.Yield();
                    }
                }));
            }

            // consumer
            var consumer = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        received.Add(await queue.Dequeue());
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            });

            go.Set();
            await Task.WhenAll(threads);
            queue.Complete();
            await consumer;

            Assert.Equal(n, received.Count);
            Assert.Equal((n - 1) * n / 2, received.Sum());
        }
    }
}
