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

        /// <summary>
        /// nLogn 시간 풀이
        /// </summary>
        public int GetMaxArea_Divide(int[] heights, int left, int right)
        {
            // 기저 사례 : 좌/우 똑같을 때, 즉 판자 1개일 때의 경우를 커버합니다.
            if (left == right)
                return heights[left];

            // 여기서 부터는, 둘 이상의 판자가 서로 연속해서 붙어있는 경우를 커버합니다.
            int mid = (left + right) / 2;

            int ret = Math.Max(GetMaxArea_Divide(heights, left, mid), GetMaxArea_Divide(heights, mid + 1, right));

            int leftPivot = mid;
            int rightPivot = mid + 1;

            // 둘 이상의 판자가 서로 붙어있는 상태에서 계속 양쪽으로 확장하며 최대 넓이를 찾으려면 두 판자 중 높이가 낮은 녀석을 선택해야합니다.
            int height = Math.Min(heights[leftPivot], heights[rightPivot]);

            // [Mid, Mid+1]의 경우를 계산해서 넣어줍니다. 양쪽으로 확장해나가야하므로, [Mid,Mid+1]은 포함되지않습니다.
            ret = Math.Max(ret, height * 2);

            // 한쪽이 아직 끝에 닿지 못했다면 계속해서 양쪽으로 확장시켜나갑니다.
            while (leftPivot > left || rightPivot < right)
            {
                // Mid를 기준으로 양쪽으로 확장시켜나갑니다.
                // 항상 더 높은 높이를 가진 판자가 있는 쪽으로 확장시켜야합니다.
                if (rightPivot < right // 우측 끝에 닿지 않은 경우,
                    && 
                    (leftPivot <= left // 좌측이 끝에 닿았거나,
                     || heights[leftPivot-1] < heights[rightPivot+1]))// 좌측이 아직 끝에 닿지 않았다면, 좌측보다 우측으로 확장했을 때 도달하는 사각형의 높이가 좌측보다 높을 때, 우측으로 진행합니다.
                {
                    rightPivot++;
                    height = Math.Min(height, heights[rightPivot]);
                }
                else
                {
                    leftPivot--;
                    height = Math.Min(height, heights[leftPivot]);
                }

                // 한쪽으로 확장하였으니, 너비를 계산하여 기록해둡니다.
                // 계속해서 확장하면서 가장 넓었던 너비를 선택합니다.
                ret = Math.Max(ret, (rightPivot - leftPivot + 1) * height);
            }

            return ret;
        }
    }
}