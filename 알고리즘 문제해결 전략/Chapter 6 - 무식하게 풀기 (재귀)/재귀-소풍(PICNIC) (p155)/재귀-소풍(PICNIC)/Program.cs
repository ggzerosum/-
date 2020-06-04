using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 재귀_소풍_PICNIC_
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Problem_Picinic
    {
        public Problem_Picinic()
        {}


        public int Solve(int number , int[] couples)
        {
            int[,] student = new int[10, 10];
            int studentNumber = number;

            for (int i = 0; i < couples.Length; i += 2)
            {
                student[i, i + 1] = 1;
            }

            return RecursiveSolve(studentNumber, student, new int[10]);
        }

        /// <summary>
        /// 순서에 상관없이 나열하고 싶을 경우, 특정 형태를 갖는 답만 도출하는 것이 노하우이다.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="array"></param>
        /// <param name="taken"></param>
        /// <returns></returns>
        private int RecursiveSolve(int n, int[,] array, int[] taken)
        {
            int firstStudent = -1;
            for (int i = 0; i < n; i++)
            {
                if (taken[i] != 1)
                {
                    firstStudent = i;
                    break;
                }
            }

            if (firstStudent == -1)
                return 1;

            int result = 0;

            for (int i = firstStudent + 1; i < n; i++)
            {
                if (taken[i] != 1 && array[firstStudent, i] != 1)
                {
                    taken[i] = array[firstStudent, i] = 1;
                    result += RecursiveSolve(n, array, taken);
                    taken[i] = array[firstStudent, i] = 0;
                }
            }

            return result;
        }
    }
}
