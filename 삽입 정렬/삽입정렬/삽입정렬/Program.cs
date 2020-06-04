using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 삽입정렬
{
    class Program
    {
        static int[] data = new int[]
        {
            1,
            10,
            5,
            8,
            25,
            21,
            18,
            12,
            3,
            2,
            0
        };

        static void Main(string[] args)
        {
            // 삽입 정렬은 이미 정렬된 부분과 비교하여, 자신의 올바른 위치를 찾아 삽입함으로써 정렬을 완성하는 알고리즘이다.
            InsertionSort(data);

            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
        }

        static void InsertionSort(in int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && array[j - 1] > array[j]) // 현재 인덱스부터 첫 인덱스까지 거슬러 올라가며 올바른 위치에 도달할 때까지 계속 삽입해나감
                {
                    Swap(j-1, j, array);
                    j--;
                }
            }
        }

        static void Swap(int a, int b, in int[] array)
        {
            int temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}