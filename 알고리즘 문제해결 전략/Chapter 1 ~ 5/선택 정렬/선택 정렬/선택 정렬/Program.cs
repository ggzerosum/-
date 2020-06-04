using System;

namespace 선택_정렬
{
    class Program
    {
        private static int[] inputs = new[]
        {
            10,
            3,
            5,
            20,
            1,
            2,
            100,
            77,
            30,
            4,
            9
        };

        static void Main(string[] args)
        {
            SelectionSort(in inputs);

            int index = 0;
            foreach (int input in inputs)
            {
                Console.WriteLine($"[{index++}] : {input}");
            }
        }

        static void SelectionSort(in int[] inputs)
        {
            for (int i = 0; i < inputs.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < inputs.Length; j++)
                {
                    min = inputs[j] < inputs[min] ? j : min;
                }

                if (min != i)
                    Swap(i, min, in inputs);
            }
        }

        static void Swap(int indexA, int indexB, in int[] array)
        {
            int a = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = a;
        }
    }
}