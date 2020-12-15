using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 백준2839___설탕배달
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://www.acmicpc.net/problem/2839
            /* 백준 2839번 문제 - 설탕배달
                
                상근이는 요즘 설탕공장에서 설탕을 배달하고 있다. 상근이는 지금 사탕가게에 설탕을 정확하게 N킬로그램을 배달해야 한다. 설탕공장에서 만드는 설탕은 봉지에 담겨져 있다. 봉지는 3킬로그램 봉지와 5킬로그램 봉지가 있다.

                상근이는 귀찮기 때문에, 최대한 적은 봉지를 들고 가려고 한다. 예를 들어, 18킬로그램 설탕을 배달해야 할 때, 3킬로그램 봉지 6개를 가져가도 되지만, 5킬로그램 3개와 3킬로그램 1개를 배달하면, 더 적은 개수의 봉지를 배달할 수 있다.

                상근이가 설탕을 정확하게 N킬로그램 배달해야 할 때, 봉지 몇 개를 가져가면 되는지 그 수를 구하는 프로그램을 작성하시오.

                입력
                첫째 줄에 N이 주어진다. (3 ≤ N ≤ 5000)

                결과
                상근이가 배달하는 봉지의 최소 개수를 출력한다. 만약, 정확하게 N킬로그램을 만들 수 없다면 -1을 출력한다.
             */
            Console.WriteLine(GetMinimum_Greedy(Convert.ToInt32(Console.ReadLine()), 5, 3));
        }

        private static int GetMinimum_Greedy(int N, int larger, int smaller)
        {
            /*
             * Greedy 방식
             * 특정 수를 큰 수로 최대한 나눈 경우부터 시작해서
             * 큰 수를 하나씩 줄여나가면서 최소 경우의 수까지 진행시키면
             * 큰 수를 최대로하는 경우를 빠르게 알아낼 수 있다.
             */
            int quotient = N / larger;
            int remainder = N % larger;
            int ret = -1;
            while (quotient >= 0)
            {
                if (remainder % smaller == 0)
                {
                    ret = quotient + (remainder / smaller);
                    break;
                }

                quotient--;
                remainder += larger;
            }

            return ret;
        }

        private static int GetMinimum_BruteForce(int N, int larger, int smaller)
        {
            /*
                string n = Console.ReadLine();
                int N = Convert.ToInt32(n);
                int result = GetMinimum_BruteForce(N, 5, 3);
                Console.WriteLine(result);
            */

            // 1. 큰 수로 나누어 떨어지는 지 확인. 큰 수로 나누어 떨어지면 항상 제일 작은 봉지 갯수를 달성할 수 있다.
            int remainder = N % larger;
            if (remainder == 0)
            {
                return N / larger;
            }

            int ret = int.MaxValue;

            // 2. 큰 수로 나누어 떨어지지 않으면 큰수 + 작은 수이거나 작은 수로 나누어 떨어지게된다.
            // 2. 우선, 큰 수 + 작은 수의 경우의 수를 모두 구하면서 최소 값을 찾는다.
            int quotient = 0;
            int N1 = N;
            while (N1 - larger >= 0)
            {
                N1 = N1 - larger;
                quotient++;

                // 큰 수로 나누어 떨어지는 경우를 제외했으므로, N1이 0이 나오는 경우는 없다.
                if (N1 % smaller == 0)
                {
                    int y = N1 / smaller;
                    ret = Math.Min(ret, quotient + y);
                }
            }

            // 3. 작은 수로 나누어 떨어지는 지 확인하고, 나누어 떨어진다면 그 값을 최소 값과 비교
            if (N % smaller == 0)
            {
                ret = Math.Min(ret, N / smaller);
            }

            return ret == int.MaxValue ? -1 : ret;
        }
    }
}