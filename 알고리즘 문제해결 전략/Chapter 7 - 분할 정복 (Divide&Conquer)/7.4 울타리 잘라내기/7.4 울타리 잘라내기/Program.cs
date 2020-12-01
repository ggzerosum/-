using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7._4_울타리_잘라내기
{
    class Program
    {
        static void Main(string[] args)
        {
            MinimumAreaOfFences minimumAreaOfFences = new MinimumAreaOfFences();
            int maxArea = minimumAreaOfFences.GetMaxArea_BruteForce(new int[]
            {
                7, 1, 5, 9, 6, 7, 3
            });

            Console.WriteLine(maxArea);
        }
    }

    internal class MinimumAreaOfFences
    {
        public int GetMaxArea_BruteForce(int[] heights)
        {
            /*
             * 어떤 수들의 집합에서, 시작(S)과 끝(E) 사이의
             * 
             */

            int count = heights.Length;
            int maxArea = 0;
            for (int left = 0; left < count; left++)
            {
                int minHeight = heights[left];
                for (int right = left; right < count; right++)
                {
                    minHeight = Math.Min(minHeight, heights[right]);
                    maxArea = Math.Max(maxArea, (right - left + 1) * minHeight);
                }
            }

            return maxArea;
        }
    }
}