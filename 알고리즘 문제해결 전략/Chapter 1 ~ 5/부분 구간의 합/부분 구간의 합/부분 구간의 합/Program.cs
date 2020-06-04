using System;

namespace 부분_구간의_합
{
    class Program
    {
        static void Main(string[] args)
        {
            var algorithm = new PartialContinousSum();
            //Console.WriteLine($"부분 구간의 합 : {algorithm.BetterMaxSum()}");

            //algorithm.DoMergeSort();
            //Console.WriteLine($"Better MaxSum : {algorithm.BetterMaxSum()}");
            //Console.WriteLine($"Fast MaxSum : {algorithm.FastMaxSum()}");
            Console.WriteLine($"Fastest MaxSum : {algorithm.FastestMaxSum(algorithm.data)}");
        }

        class PartialContinousSum
        {
            public int[] data = {-2, 1, -2, 1, 1, 11, 1, -4};

            public int BetterMaxSum()
            {
                int N = data.Length, ret = int.MinValue;
                for (int i = 0; i < N; i++)
                {
                    int sum = 0;
                    for (int j = i; j < N; j++)
                    {
                        sum += data[j]; // i ~ N 까지 순차적으로 합을 구해나간다.
                        ret = Math.Max(ret, sum); // 모든 순차적으로 구해지는 합 중, 가장 큰 합을 찾아낸다. Ret은 단순히 결과값을 저장하기위한 용도
                    }
                }

                return ret;
            }

            public int FastMaxSum()
            {
                return FastMaxSum_DivideAndConquer(data, 0, data.Length - 1);
            }
            public int FastMaxSum_DivideAndConquer(int[] data, int left, int right)
            {
                if (left == right) // 기저 사례 : 구간의 길이가 1일 경우
                    return data[left];

                int mid = (left + right) / 2;

                // 1. 두 구간에 걸쳐있는 부분 합을 구한다.
                int leftSum = int.MinValue, rightSum = int.MinValue, tempSum = 0;
                for (int leftPivot = mid; leftPivot >= left; leftPivot--) // 좌측으로
                {
                    tempSum += data[leftPivot];
                    leftSum = Math.Max(leftSum, tempSum);
                }

                tempSum = 0;
                for (int rightPivot = mid + 1; rightPivot <= right; rightPivot++) // 우측으로
                {
                    tempSum += data[rightPivot];
                    rightSum = Math.Max(rightSum, tempSum);
                }

                // 2. 각 구간의 합을 구한다.
                int single = Math.Max(
                    FastMaxSum_DivideAndConquer(data, left, mid),
                    FastMaxSum_DivideAndConquer(data, mid + 1, right));

                return Math.Max(leftSum + rightSum, single);
            }

            public int FastestMaxSum(int[] data)
            {
                int N = data.Length, ret = int.MinValue, psum = 0;
                for (int i = 0; i < N; i++)
                {
                    psum = Math.Max(0, psum) + data[i]; // max = Max(0, MaxAt(i - 1)) + i; 점화식이 성립함에서 출발한다.
                    Console.WriteLine($"{psum}");
                    ret = Math.Max(psum, ret);
                }

                return ret;
            }

            public void DoMergeSort()
            {
                int[] temp = new int[data.Length];
                MergeSort(data, temp, 0, data.Length - 1);
                foreach (int e in data)
                {
                    Console.WriteLine($"MergeSort : {e}");
                }
            }
            void MergeSort(int[] data, int[] temp, int start, int end)
            {
                if (start < end)
                {
                    int mid = (start + end) / 2;
                    MergeSort(data, temp, start, mid);
                    MergeSort(data, temp, mid+1, end);

                    for (int i = start; i <= end; i++)
                    {
                        temp[i] = data[i];
                    }

                    int leftPivot = start;
                    int rightPivot = mid + 1;
                    int index = start;

                    while (leftPivot <= mid && rightPivot <= end)
                    {
                        if (temp[leftPivot] <= temp[rightPivot])
                        {
                            data[index] = temp[leftPivot];
                            leftPivot++;
                        }
                        else
                        {
                            data[index] = temp[rightPivot];
                            rightPivot++;
                        }

                        index++;
                    }

                    for (int remainder = 0; remainder < (mid - leftPivot) + 1; remainder++)
                    {
                        data[index++] = temp[leftPivot + remainder];
                    }
                }
            }
        }
    }
}