using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2748번___피보나치_수2
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int N = 0; N < 18; N++)
            {
                //int N = Convert.ToInt32(Console.ReadLine());
                int[] array = new int[N + 1];
                for (int i = 0; i < N + 1; i++)
                {
                    array[i] = -1;
                }
                Console.WriteLine(Fibonachi_Dynamic(array, N));
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }
        
        // 동적 계획법, 하위 부분 문제를 해결하고 하위 부분 문제의 해를 저장하여 다시 활용함으로써 시간을 줄이는 기법
        private static int Fibonachi_Dynamic(int[] cache, int n)
        {
            if (n == 0)
                return 0;

            if (n == 1)
                return 1;

            if (cache[n] != -1)
            {
                return cache[n];
            }
            else
            {
                cache[n - 1] = Fibonachi_Dynamic(cache, n - 1);
                cache[n - 2] = Fibonachi_Dynamic(cache, n - 2);

                cache[n] = cache[n - 1] + cache[n - 2];
                return cache[n];
            }
        }
    }

    class NormalMethod
    {
        private int Fibonachi(int n)
        {
            if (n == 0)
                return 0;

            if (n == 1)
                return 1;

            return Fibonachi(n - 1) + Fibonachi(n - 2);
        }
    }
}
