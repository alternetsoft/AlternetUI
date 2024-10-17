using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads
{
    public class Program
    {
        static Queue<int> queue = new Queue<int>();
        static object consoleSync = new object();

        static void ProducerThreadProc()
        {
            var random = new Random(Environment.TickCount);
            while (true)
            {
                var value = random.Next();
                lock (queue)
                {
                    if (queue.Count < 1000)
                        queue.Enqueue(value);
                }
            }
        }

        static void ConsumerThreadProc()
        {
            while (true)
            {
                int? value = null;
                lock (queue)
                {
                    if (queue.Count > 0)
                        value = queue.Dequeue();
                }

                if (value != null)
                {
                    lock (consoleSync)
                        Console.WriteLine(value);
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            const int ProducerCount = 4;
            const int ConsumerCount = 3;

            for (int i = 0; i < ProducerCount; i++)
                new Thread(ProducerThreadProc) { IsBackground = true }.Start();

            for (int i = 0; i < ConsumerCount; i++)
                new Thread(ConsumerThreadProc) { IsBackground = true }.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
