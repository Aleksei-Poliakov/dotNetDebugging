using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

namespace Demo3_Multiple_controllers
{
    class Demo3_multiple_controllers
    {
        static void Main(string[] args)
        {
            var container = new ResultContainer();
            var controllers = new IController[] { new Controller0(container, 1), new Controller0(container, 2), new Controller1(container), new Controller2(container) };

            try
            {
                Task.WaitAll(controllers.AsParallel().Select(t => t.Run()).ToArray());
            }
            catch (AggregateException e)
            {
                e.Handle(ex => ex is TaskCanceledException);
            }
            
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

    public class ResultContainer
    {
        private int counter = 0;
        private readonly object locker = new object();
        private readonly ConcurrentBag<object> results = new ConcurrentBag<object>();

        public void StartExecution()
        {
            lock (locker)
            {
                Interlocked.Increment(ref counter);
            }
        }

        public void StopExecution()
        {
            lock (locker)
            {
                Interlocked.Decrement(ref counter);
            }

            if (counter == 0)  // counter
            {
                // Send collected data to requester
                Console.WriteLine("Done"); // NOTE: we do not see this one printed out
            }
        }

        public void AddObjectToResult(object result)
        {
            Console.WriteLine("Adding result {0}", result);
            results.Add(result);
        }

    }

    public interface IController
    {
        Task Run();
    }

    public class Controller0 : IController
    {
        public ResultContainer Container { get; set; }
        public int InstanceId { get; set; }

        public Controller0(ResultContainer container, int instanceId)
        {
            Container = container;
            InstanceId = instanceId;
            Container.StartExecution();
        }

        public Task Run()
        {
            if ((InstanceId ^ 2) == 0)
            {
                return Task.Delay(0); // instance is disabled
            }

            return Task.Factory.StartNew(() =>
                                  {
                                      Container.AddObjectToResult(8);
                                      Container.StopExecution();
                                  });
        }
    }

    public class Controller1 : IController
    {
        public ResultContainer Container { get; set; }

        public Controller1(ResultContainer container)
        {
            Container = container;
            Container.StartExecution();
        }

        public Task Run()
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             for (var i = 0; i < 5; i++)
                                             {
                                                 Container.AddObjectToResult(i.ToString());
                                             }
                                             Container.StopExecution();
                                         });
        }
    }

    public class Controller2 : IController
    {
        public ResultContainer Container { get; set; }

        public Controller2(ResultContainer container)
        {
            Container = container;
            Container.StartExecution();
        }

        public Task Run()
        {
            var source = new CancellationTokenSource(1000);
            var token = source.Token;

            var input = Task.Factory.StartNew(() =>
                                              {
                                                  var random = new Random();
                                                  while (true)
                                                  {
                                                      if (token.IsCancellationRequested) return;

                                                      var sleep = random.Next(500, 800);
                                                      Thread.Sleep(sleep);
                                                      Container.AddObjectToResult(sleep.ToString());
                                                  }
                                              }, token);

            return input.ContinueWith(obj =>
                                      {
                                          Container.AddObjectToResult(obj);
                                          Container.StopExecution();
                                      }, token); // NOTE: this is probably a bug - we do not want the same cancellation token for both input and finalization
        }
    }
}
