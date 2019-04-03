using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo4_ParallelTasks
{
    class Demo4_ParallelTasks
    {
        private static Task deadlock;

        static void Main(string[] args)
        {
            var t1 = new Task(A);
            var t2 = new Task(B);
            var t3 = new Task(() => C(3));
            var t5 = new Task(() => C(1));

            deadlock = t2;

            var t4 = t3.ContinueWith(D);

            t1.Start();
            t2.Start();
            t3.Start();
            t5.Start();

            Task.WaitAll(new[] { t1, t2, t4, t5 });
        }

        private static object locker = new object();
        private static object locker2 = new object();

        public static void A()
        {
            Monitor.Enter(locker);
            bool loop = true;
            while (loop)
            {
                Monitor.Enter(locker2);
            }
            deadlock.Wait();
        }

        public static void B()
        {
            Monitor.Enter(locker2);
            Thread.Sleep(1000);
            Monitor.Enter(locker);
            while (true)
            {
            }
        }

        public static void C(int delta)
        {
            var counter = 0;
            while (true)
            {
                counter+=delta;
            }
        }

        public static void D(object state)
        {
            while (true)
            {

            }
        }
    }
}
