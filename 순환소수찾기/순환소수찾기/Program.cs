using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 순환소수찾기
{
    class Program
    {
        static void Main(string[] args)
        {
            var algorithm = new Algorithm();
            algorithm.Decimal(1, 18);
        }
    }


    class Algorithm
    {
        public void Decimal(int numerator, int denominator)
        {
            int a = numerator / denominator;
            Console.Write($"{a}.");

            int iter = 0;
            while (numerator > 0)
            {
                numerator = (numerator % denominator) * 10;
                int b = numerator / denominator;
                ++iter;
                if (iter >= denominator)
                    // Denominator는 11이므로, X % 11의 값은 0 ~ 10 사이의 값을 지니게된다. (11가지 경우의 수)
                    // 따라서, 비둘기집의 원리에 따라 11개가 넘는 실수부가 존재한다면 반드시 11번째부터는 앞의 어떠한 수와 중복이 일어나게된다.
                    // (이것은 유리수의 분수 계산에만 적용된다.)
                {
                    Console.Write(" : 순환소수");
                    break;
                }

                Console.Write($"{b}");
            }
        }
    }
}