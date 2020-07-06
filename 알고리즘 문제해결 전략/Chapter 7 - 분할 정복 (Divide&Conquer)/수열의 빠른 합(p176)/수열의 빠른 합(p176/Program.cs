using System;

namespace 수열의_빠른_합_p176
{
    class Program
    {
        static void Main(string[] args)
        {
            int sum = Algorithm.FastestSum(100);
            Console.WriteLine($"Sum: {sum}");
        }
    }

    static class Algorithm
    {
        public static int FastestSum(int n)
        {
            if (n == 1)
                return 1;

            if (n % 2 != 0)
                return FastestSum(n - 1) + n;

            return 2 * FastestSum(n / 2) + (n/2 * n/2);
        }
    }
}