using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 카라츠바의_빠른_곱셈
{
    class Program
    {
        static void Main(string[] args)
        {
            //JustMultiply();
            Karatsuba();
        }

        static void JustMultiply()
        {
            // 정수 A * B를 계산
            int[] a = new int[] { 0, 0, 1 };
            int[] b = new int[] { 0, 0, 1 };
            int[] A_Multiply_B = new JustMultiply().Multiply(a, b);
            Debug.Print(A_Multiply_B);
        }

        static void Karatsuba()
        {
            // 정수 A * B를 계산
            List<int> a = new List<int>(new [] { 0, 0, 1 });
            List<int> b = new List<int>(new[] { 0, 0, 1 });
            List<int> A_Multiply_B = new Karatsuba().Multiply(a, b);
            Debug.Print(A_Multiply_B);
        }
    }

    class Debug
    {
        [Conditional("DEBUG")]
        public static void Print(ICollection<int> number, string msg = "")
        {
            Console.WriteLine("=== Value ===");

            if (!string.IsNullOrEmpty(msg))
                Console.WriteLine(msg);

            for (int i = number.Count - 1; i >= 0; i--)
            {
                Console.Write(number.ElementAt(i));
            }
            Console.WriteLine();
            Console.WriteLine("=========");
        }
    }

    class JustMultiply
    {
        public int[] Multiply(int[] a, int[] b)
        {
            int size = a.Length + b.Length + 1;
            int[] number = new int[size];

            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    number[i + j] = a[i] * b[j];
                }
            }

            Normalize(ref number);
            return number.ToArray();
        }

        public void Normalize(ref int[] number)
        {
            for (int i = 1; i < number.Length - 1; i++)
            {
                int quotient = number[i] / 10;
                int remainder = number[i] % 10;
                number[i + 1] += quotient;
                number[i] = remainder;
            }
        }
    }

    public class Karatsuba
    {
        public List<int> Multiply(List<int> a, List<int> b)
        {
            // 기저 사례 : a가 b보다 짧을 경우 순서를 바꿔야한다.
            if (a.Count < b.Count)
                return Multiply(b, a);

            // 기저 사례 : a혹은 b의 길이가 0일 경우
            if (a.Count <= 0 || b.Count <= 0)
                return new List<int>(0);

            // 기저 사례 : 더 이상 쪼갤 수 없는 경우 일반 곱셈을 진행해야한다.
            if (a.Count == 1)
                return NormalMultiply(a, b);

            // 1 2 3 4
            //     2 0
            // a1 = 1 2
            // a0 = 3 4
            // b1 = 없음
            // b0 = 2 0
            int halfOfA = a.Count / 2;
            int halfOfB = Math.Min(halfOfA, b.Count);
            // a길이 절반을 나눠 a1, a0로 나눈다.
            List<int> a0 = CopyRange(a, 0, halfOfA);
            List<int> a1 = CopyRange(a, halfOfA, a.Count - halfOfA);

            // b길이 절반을 나눠 b1, b0로 나눈다.
            List<int> b0 = CopyRange(b, 0, halfOfB);
            List<int> b1 = CopyRange(b, halfOfB, b.Count - halfOfB);

            // z0 = a1 * b1
            // z2 = a0 * b0
            List<int> z2 = Multiply(a1, b1);
            List<int> z0 = Multiply(a0, b0);

            // sampleZ1 = add(a0, a1) * add(b0, b1)
            List<int> sampleZ1 = Multiply(Add(a0, a1), Add(b0, b1));
            // z1 = sampleZ1 - z0 - z2 (sampleZ1은 z0+z2보다 무조껀크다.)
            List<int> z1 = Subtract(Subtract(sampleZ1, z0), z2);

            // result = z0*10^N + z1*10^halfN + z0
            List<int> result = new List<int>();
            result = Add(result, ExponentialOf10(z0, 0));
            result = Add(result, ExponentialOf10(z1, halfOfA));
            result = Add(result, ExponentialOf10(z2, halfOfA * 2));
            return result;
        }

        private List<int> ExponentialOf10(List<int> input, int k)
        {
            int expand = input.Count + k;
            List<int> result = new List<int>(expand);
            for (int i = 0; i < k; i++)
            {
                result.Add(0);
            }

            foreach (int i in input)
            {
                result.Add(i);
            }

            return result;
        }

        public List<int> Add(List<int> a, List<int> b)
        {
            // a+b == b+a 임에 기초하여 코드를 간결하게 만들기위함.
            if (a.Count < b.Count)
                return Add(b, a);

            if (a.Count == 0 && b.Count == 0)
                return new List<int>(0);

            List<int> result = new List<int>(a);
            for (int i = 0; i < b.Count; i++)
            {
                result[i] += b[i];
            }

            return Normalize(result);
        }

        // a >= b 임을 가정한다. 그렇지 않을 경우 버그가 발생한다.
        // 이를 통해서 항상 a와 b가 같더라도 항상 마지막 숫자의 계산은 음수가 아니라 양수로 하게되므로
        // i, i+1 예외처리가 필요없어진다.
        public List<int> Subtract(List<int> a, List<int> b)
        {
            if (a.Count < b.Count)
                throw new ArgumentException("A가 B보다 커야합니다.");

            if (a.Count == 0 && b.Count == 0)
                return new List<int>(0);

            List<int> sample = new List<int>(a);
            for (int i = 0; i < b.Count; i++)
            {
                if (sample[i] >= b[i])
                {
                    sample[i] = sample[i] - b[i];
                }
                else
                {
                    int subtract = sample[i] - b[i];
                    int borrow = (Math.Abs(subtract) / 10) + 1;
                    sample[i + 1] -= borrow;
                    sample[i] = borrow * 10 + subtract;
                }
            }

            return Normalize(sample);
        }

        private List<int> Normalize(List<int> input)
        {
            List<int> sample = new List<int>();
            sample.AddRange(input);
            sample.Add(0);

            for (int i = 0; i + 1 < sample.Count; i++)
            {
                if (sample[i] < 0)
                {
                    // 음수는 앞의 수에서 10*n을 빌려와야한다.
                    int borrow = (Math.Abs(sample[i]) / 10) + 1; // 책에서는 (Math.Abs(sample[i]) + 9) / 10 이다.
                    sample[i + 1] -= borrow;
                    sample[i] += borrow * 10;
                }
                else
                {
                    int remainder = sample[i] % 10;
                    int quotient = sample[i] / 10;

                    sample[i] = remainder;
                    sample[i + 1] += quotient;
                }
            }

            // Normalize를 완료한 후 앞에 0이 남아있으면 지워주어야한다.
            int fromLastIndex = sample.Count - 1;
            while (fromLastIndex >= 0 && sample[fromLastIndex] == 0)
            {
                sample.RemoveAt(fromLastIndex);
                fromLastIndex--;
            }

            return sample;
        }

        private List<int> CopyRange(in List<int> copyFrom, int start, int length)
        {
            List<int> container = new List<int>(0);
            if (start < 0 || length <= 0)
                return container;

            for (int i = start; i < start+length; i++)
            {
                //if (i >= copyFrom.Count)
                //    break;

                container.Add(copyFrom[i]);
            }

            return container;
        }

        public List<int> NormalMultiply(List<int> a, List<int> b)
        {
            int size = a.Count + b.Count + 1;
            List<int> sample = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                sample.Add(0);
            }

            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    sample[i + j] = a[i] * b[j];
                }
            }

            return Normalize(sample);
        }
    }
}