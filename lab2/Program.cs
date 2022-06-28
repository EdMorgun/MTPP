using System;
using System.Linq;
using System.Threading.Tasks;

namespace lab2
{
    class Program
    {
        static int[] A;
        static int[] B;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            GenerateMatrixA();

            WriteMatrix(A);

            CreateMatrixB();

            WriteMatrix(B);

            FindMinAndIndex();
        }

        private static void FindMinAndIndex()
        {
            int minA = A[0], minB = B[0], indexA = 0, indexB = 0;

            Parallel.ForEach(A, a =>
            {
                if (a < minA)
                {
                    minA = a;
                    indexA = A.ToList().IndexOf(a);
                }
              
            });

            Parallel.ForEach(B, b =>
            {
                if (b < minB)
                {
                    minB = b;
                    indexB = B.ToList().IndexOf(b);
                }
            });

            Console.WriteLine($"\nMinA: {minA}(index:{indexA}) MinB: {minB}(index:{indexB})");
        }

        private static void CreateMatrixB()
        {
            B = new int[A.Length];
            int i = 0;

            Parallel.ForEach(A, a =>
            {
                B[i] = a * 2;
                i++;
            });

            Console.WriteLine("Множення на 2");
        }

        private static void GenerateMatrixA()
        {
            var rand = new Random();

            A = new int[rand.Next(20, 50)];

            for (int i = 0; i < A.Length; i++)
            {
                A[i] = rand.Next(1, 1000);
            }

        }

        private static void WriteMatrix(int[] matrix)
        {
            string text = "[";
            
            for (int i = 0; i < A.Length; i++)
            {
                text += $"{matrix[i]}, ";
            }
            text += "\b\b]";
            Console.WriteLine("\n" + text);
        }
        
    }
}
