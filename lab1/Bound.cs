using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace lab1
{
    public class Bound
    {
        private int threadCount;     // Число потоків
        private readonly string directory = @"D:\Cache\";

        public Bound(int threadCount)
        {
            this.threadCount = threadCount;
        }

        public void CPU()
        {
            for (int i = 0; i < threadCount; i++)
            {
                var task = new Thread(() => DoCPU(i));

                task.Start();
                task.Join();
            }
        }

        private void DoCPU(int i)
        {
            i *= 100;
        }

        public void Memory()
        {
            for (int i = 0; i < threadCount; i++)
            {
                var task = new Thread(() => DoMemory());

                task.Start();
                task.Join();
            }
        }

        private void DoMemory()
        {
            List<int[]> list = new List<int[]>();

            for (int i = 1; i < 1000; i++)
            {
                list.Add(new int[i]);
            }

            foreach (var item in list)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    item[i] = i * 100;
                }
            }
        }

        public void IO()
        {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            for (int i = 0; i < threadCount; i++)
            {
                var task = new Thread(() => DoIO(i));
       
                task.Start();
                task.Join();
            }

        }

        private void DoIO(int i)
        {
            lock (this)
            {
                using (var fs = File.Create($"{directory}{i}.txt"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");

                    fs.Write(info, 0, info.Length);
                }

                using (StreamReader sr = File.OpenText($"{directory}{i}.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null) ;
                }
            }
        }
    }
}
