using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace lab3
{
    class Program
    {
        static List<int>[] crystal;

        static int N;       // довжина
        static double p;    // імовірність вправо
        static int K;       // кількість часток
        static int I;       // кількість ітерацій                
        static int sec;     // час

        static void Main(string[] args)
        {
            ChooseMode();
        }

        public static void ChooseMode()
        {
            Console.WriteLine("Оберіть режим: 1 - за часом, 2 - за кіл-тю ітерацій");

            int mode = 0;

            do
            {
                var number = Console.ReadLine();

                int.TryParse(number, out mode);
                
            } while (mode != 1 && mode != 2);

            if (mode == 1)
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
            crystal = new List<int>[N];

            for (int i = 0; i < crystal.Length; i++)
            {
                crystal[i] = new List<int>();
            }

            GenerateThreads();
        }

        private static void TimeMode()
        {
            crystal = new List<int>[N];

            for (int i = 0; i < crystal.Length; i++)
            {
                crystal[i] = new List<int>();
            }

            GenerateThreads();
        }

        private static void GenerateThreads()
        {
            Parallel.For(0, K, (i, state) =>
            {
                var task = new Thread(() => MovePartical(i+1));

                task.Start();
                task.Join();
            });
        }

        private static void MovePartical(int number)
        {
            int position = 0;
            var rand = new Random();
            var now = DateTime.Now;

            lock (crystal)
            {
                crystal[0].Add(number);
                WriteCrystal();
            }

            if (sec != 0)
            {
                while (now.AddSeconds(sec) > DateTime.Now)
                {
                    lock (crystal)
                    {
                        if (rand.NextDouble() < p)
                            position = MoveRight(position, number);
                        else
                            position = MoveLeft(position, number);

                        WriteCrystal();
                    }
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
                            position = MoveRight(position, number);
                        else
                            position = MoveLeft(position, number);                       
                    }
                }
                lock (crystal)
                {
                    WriteCrystal();
                }
            }
        }

        private static int MoveLeft(int position, int number)
        {
            if(position - 1 < 0)
            {
                return MoveRight(position, number);
            }

            crystal[position].Remove(number);
            crystal[--position].Add(number);
            return position;
        }

        private static int MoveRight(int position, int number)
        {
            if (position + 1 >= N)
            {
                return MoveLeft(position, number);
            }

            crystal[position].Remove(number);
            crystal[++position].Add(number);
            return position;
        }

        private static void WriteCrystal()
        {
            Console.Clear();

            Console.WriteLine(new string('_', N + K * 4));
            Console.Write("|");
            foreach (var cell in crystal)
            {
                foreach (var item in cell)
                {
                    Console.Write($" {item} ");
                }
                Console.Write("|");
            }
            Console.WriteLine();
            Console.WriteLine(new string('_', N + K * 4));
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
