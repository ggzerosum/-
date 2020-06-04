using System;
using System.Collections.Generic;

namespace M_이동평균
{
    class Program
    {
        private static float[] weights = new float[12]
        {
            70,
            75,
            80,
            90,
            85,
            80,
            75,
            72,
            78,
            82,
            85,
            90
        };

        static void Main(string[] args)
        {
            int num = 3;
            var average1 = MovingAverage(weights, num, Algorithm.Exponential);
            Console.WriteLine("지수시간 알고리즘");
            for (int i = 0; i < average1.Length; i++)
            {
                Console.WriteLine($"{i}~{i+(num - 1)}까지의 이동 평균 : {average1[i]}");
            }

            Console.WriteLine("\t = = = = = = ");

            Console.WriteLine("선형시간 알고리즘");
            var average2 = MovingAverage(weights, num, Algorithm.Linear);
            for (int i = 0; i < average2.Length; i++)
            {
                Console.WriteLine($"{i}~{i + (num - 1)}까지의 이동 평균 : {average2[i]}");
            }
        }

        public enum Algorithm
        {
            Exponential,
            Linear
        }
        static float[] MovingAverage(float[] data, int m, Algorithm algorithm)
        {
            return algorithm == Algorithm.Exponential ? 
                MovingAverageCalculator.ExponentialTime_M_Moving_Average(data, m) : MovingAverageCalculator.ExponentialTime_M_Moving_Average(data, m);
        }
    }

    class MovingAverageCalculator
    {
        public static float[] ExponentialTime_M_Moving_Average(float[] array, int m)
        {
            int size = array.Length;
            List<float> everyM_MovingAverage = new List<float>();
            for (int i = m - 1; i < size; i++) // i의 위치에서 이전 M개의 값을 찾아내기위해 M-1부터 시작
            {
                float partialSum = 0f;
                for (int j = 0; j < m; j++) // M-1 이전 M개의 요소들의 합을 구하여
                {
                    partialSum += array[i - j];
                }
                everyM_MovingAverage.Add(partialSum / (float)m); // 평균을 계산
            }

            return everyM_MovingAverage.ToArray();
        }

        public static float[] LinearTime_M_Moving_Average(float[] array, int m)
        {
            int size = array.Length;
            List<float> everyM_MovingAverage = new List<float>();

            float partialSum = 0f;
            for (int i = 0; i < m - 1; i++)
            {
                partialSum += array[i];
            }

            for (int i = m - 1; i < size; i++)
            {
                partialSum += array[i];
                everyM_MovingAverage.Add(partialSum / (float)m);
                partialSum -= array[i - m + 1]; // 최초값이 i = m - 1 이므로,(m - 1) - m + 1 = 0 이 되어야함. (-1 방지)
            }

            return everyM_MovingAverage.ToArray();
        }
    }
}