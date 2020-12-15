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
            // For문을 활용한 동적계획법
            int n = Convert.ToInt32(Console.ReadLine());

            if (n < 2)
            {
                Console.WriteLine(n);
                return;
            }

            int f0 = 0;
            int f1 = 1;
            for (int i = 2; i <= n; i++)
            {
                int current = f1 + f0;
                f0 = f1;
                f1 = current;
            }

            Console.WriteLine(f1);
        }

        class UsingArray
        {
            public int Solve(int N)
            {
                int[] array = new int[N + 1];
                for (int i = 0; i < N + 1; i++)
                {
                    array[i] = -1;
                }

                return Fibonachi_Dynamic(array, N);
            }
            // 동적 계획법, 하위 부분 문제를 해결하고 하위 부분 문제의 해를 저장하여 다시 활용함으로써 시간을 줄이는 기법
            private int Fibonachi_Dynamic(int[] cache, int n)
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
