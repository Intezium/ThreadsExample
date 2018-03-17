using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadsExample
{
    static class Rand
    {
        static Random rand;

        static Rand()
        {
            rand = new Random();
        }

        public static int Next()
        {
            return rand.Next();
        }
        public static int Next(int maxValue)
        {
            return rand.Next(maxValue);
        }
        public static int Next(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }
    }

    class Program
    {
        static void OneThread()
        {
            int arraySize = 1024 * 1024 * 256;
            int threadsCount = 1;

            Start(arraySize, threadsCount);
        }
        static void FourThreads()
        {
            int arraySize = 1024 * 1024 * 256;
            int threadsCount = 4;

            Start(arraySize, threadsCount);
        }

        static void Start(int arraySize, int threadsCount)
        {
            if (threadsCount > 0)
            {
                int[] numbers = new int[arraySize];
                Thread[] threads = new Thread[threadsCount];

                for (byte i = 0; i < threadsCount; i++)
                    threads[i] = new Thread(() => FillArray(numbers, i, threadsCount));

                for (byte i = 0; i < threadsCount; i++)
                    threads[i].Start();

                for (byte i = 0; i < threadsCount; i++)
                    threads[i].Join();


                Stopwatch stopWatch = new Stopwatch();

                stopWatch.Start();

                if (threadsCount > 4)
                    Console.WriteLine("[{0} Потоков]\nМинимальное: {1}", threadsCount, GetMinNumber(numbers, threadsCount));
                else if (threadsCount > 1)
                    Console.WriteLine("[{0} Потока]\nМинимальное: {1}", threadsCount, GetMinNumber(numbers, threadsCount));
                else if (threadsCount == 1)
                    Console.WriteLine("[{0} Поток]\nМинимальное: {1}", threadsCount, GetMinNumber(numbers, threadsCount));

                stopWatch.Stop();

                Console.WriteLine("Прошло миллисекунд: {0}\n", stopWatch.ElapsedMilliseconds);
            }
            else
                Console.WriteLine("[Ошибка]\nУкажите один или более потоков!\n");
        }

        static void FillArray(int[] array, int threadNumber, int threadsCount)
        {
            for (int i = threadNumber; i < array.Length; i += threadsCount)
                array[i] = Rand.Next(10);
        }

        static int GetMinNumber(int[] array, int threadsCount)
        {
            Thread[] threads = new Thread[threadsCount];
            int[] resultArray = new int[threadsCount];

            for (int i = 0; i < threadsCount; i++)
            {
                int j = i;

                threads[i] = new Thread(() => { resultArray[j] = SearchMinNumber(array, j, threadsCount); });
            }

            for (int i = 0; i < threadsCount; i++)
                threads[i].Start();

            for (int i = 0; i < threadsCount; i++)
                threads[i].Join();


            int min = resultArray[0];

            for (int i = 1; i < resultArray.Length; i++)
                if (min > resultArray[i])
                    min = resultArray[i];

            return min;
        }

        static int SearchMinNumber(int[] array, int threadNumber, int threadsCount)
        {
            int min = array[0];

            for (int i = threadNumber; i < array.Length; i += threadsCount)
                if (min > array[i])
                    min = array[i];

            return min;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("===>[Поиск минимального числа в массиве]<===\n");

            OneThread();
            FourThreads();

            Console.ReadKey();
        }
    }
}
