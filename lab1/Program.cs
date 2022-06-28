using System;
using System.Diagnostics;

namespace lab1
{
    class Program
    {     
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            int threadCount;

            do
            {
                threadCount = Lab1(GetThreads());
            } while (threadCount != 0);
        }

        static int GetThreads()
        {
            int threadCount = 0;

            while (true)
            {
                Console.WriteLine("Введіть число потоків (0 - вихід)");
                var number = Console.ReadLine();

                if (int.TryParse(number, out threadCount))
                {
                    Console.WriteLine();
                    return threadCount;
                }
            }
        }

        static int Lab1(int threadCount)
        {
            Stopwatch stopWatch = new Stopwatch();
            var bound = new Bound(threadCount);

            Console.WriteLine("CPU: початок роботи. Кількість потоків: " + threadCount);
            stopWatch.Start();
            bound.CPU();
            stopWatch.Stop();
            ElapseStopWatch(stopWatch.Elapsed, "CPU-bound");
            stopWatch.Reset();

            Console.WriteLine("Memory: початок роботи. Кількість потоків: " + threadCount);
            stopWatch.Start();
            bound.Memory();
            stopWatch.Stop();
            ElapseStopWatch(stopWatch.Elapsed, "Memory-bound");
            stopWatch.Reset();

            Console.WriteLine("IO: початок роботи. Кількість потоків: " + threadCount);
            stopWatch.Start();
            bound.IO();
            stopWatch.Stop();
            ElapseStopWatch(stopWatch.Elapsed, "IO-bound");

            return threadCount;
        }

        static void ElapseStopWatch(TimeSpan ts, string name)
        {
            Console.WriteLine($"Кінець роботи {name}. Чаc роботи - {ts.TotalMilliseconds}\n");
        }
    }
}
