using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace lab3
{
    class Program
    {
        static int[] crystal;

        static int N;       // довжина
        static double p;    // імовірність вправо
        static int K;       // кількість часток
        static int I;       // кількість ітерацій                
        static int sec;     // час
        static Mode mode;    // мод

        enum Mode : int
        {
            Time = 1,
            Iteration = 2
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            ChooseMode();
        }

        public static void ChooseMode()
        {
            Console.WriteLine("Оберіть режим: 1 - за часом, 2 - за кіл-тю ітерацій");

            do
            {
                var number = Console.ReadLine();

                Enum.TryParse(number, out mode);
                
            } while (mode != Mode.Time && mode != Mode.Iteration);

            if (mode == Mode.Time)
            {
                GetNumber(" - довжина масиву", out N);
                GetNumber(" - імовірність йти праворуч. Формат -> (0,75)", out p);
                GetNumber(" - кількість часток", out K);
                GetNumber(" - час (секунди)", out sec);

                TimeMode();
            }    
            else
            {
                GetNumber(" - довжина масиву", out N);
                GetNumber(" - імовірність йти праворуч. Формат -> (0,75)", out p);
                GetNumber(" - кількість часток", out K);
                GetNumber(" - кількість ітерацій", out I);

                IterationMode();
            }

        }

        private static void IterationMode()
        {
            GenerateCrystal();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            GenerateThreads();

            stopWatch.Stop();
            WriteCrystal();

            ElapseStopWatch(stopWatch.Elapsed);
        }

        static void ElapseStopWatch(TimeSpan ts)
        {
            Console.WriteLine($"Кінець роботи. Чаc роботи - {ts.TotalMilliseconds} ms\n");
        }

        private static void TimeMode()
        {
            GenerateCrystal();

            GenerateThreads();
        }

        private static void GenerateCrystal()
        {
            crystal = new int[N];
            crystal[0] = K;
        }

        private static void GenerateThreads()
        {
            Parallel.For(0, K, (i, state) =>
            {
                var task = new Thread(() => MovePartical());

                task.Start();
                task.Join();
            });
        }

        private static void MovePartical()
        {
            int position = 0;
            var rand = new Random();
            var now = DateTime.Now;

            if(mode == Mode.Time) WriteCrystal();


            if (sec != 0)
            {
                while (now.AddSeconds(sec) > DateTime.Now)
                {
                    lock (crystal)
                    {
                        if (rand.NextDouble() < p)
                            position = MoveRight(position);
                        else
                            position = MoveLeft(position);
                    }
                    
                    if (mode == Mode.Time) WriteCrystal();                    
                    Thread.Sleep(500);
                }
            }
            else
            {
                for (int i = 0; i < I; i++)
                {
                    lock (crystal)
                    {
                        if (rand.NextDouble() < p)
                            position = MoveRight(position);
                        else
                            position = MoveLeft(position);                       
                    }
                }

                if (mode == Mode.Time) WriteCrystal();    
            }
        }

        private static int MoveLeft(int position)
        {
            if(position - 1 < 0)
            {
                return position;
            }
           
            crystal[position]--;
            crystal[--position]++;
            return position;
        }

        private static int MoveRight(int position)
        {
            if (position + 1 >= N)
            {
                return position;
            }

            crystal[position]--;
            crystal[++position]++;
            return position;
        }

        private static void WriteCrystal()
        {
            Console.WriteLine();

            lock (crystal)
            {
                var str = "[" + string.Join(" ", crystal) + "]";
                Console.WriteLine(str);
            }
        }

        static void GetNumber(string s, out int i)
        {
            while (true)
            {
                Console.WriteLine($"Введіть число {s}");
                var number = Console.ReadLine();

                if (int.TryParse(number, out i) && i > 1)
                {
                    Console.WriteLine();
                    return;
                }
            }
        }

        static void GetNumber(string s, out double i)
        {
            while (true)
            {
                Console.WriteLine($"Введіть число {s}");
                var number = Console.ReadLine();

                if (double.TryParse(number, out i) && i != 0)
                {
                    Console.WriteLine();
                    return;
                }
            }
        }
    }
}
