using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 쿼드_트리_뒤집기__Quard_Tree__p189__
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "xbwxwbbwb";

            // 원본
            Console.WriteLine("원본");
            QuadTree quadTree = new QuadTree();
            int[,] image = quadTree.Decompress(data, 10);
            new Image(image).Print();

            // 상하반전
            Console.WriteLine("상하반전");
            string upsidedownedData = quadTree.UpSideDown(data);
            int[,] upsidedownedImage = quadTree.Decompress(upsidedownedData, 10);
            new Image(upsidedownedImage).Print();
        }

        class QuadTree
        {
            public QuadTree()
            {}

            /* 쿼드 트리의 압축 해제 방법
             * 어떠한 문자열을 압축해제하여 NxN 크기의 배열에 저장하는 압축해제 함수를 생각해보자.
             * 쿼드 트리는 항상 4부분으로 나눠지므로, 한번 재귀함수가 실행될 때마다 N의 사이즈가 2씩 줄어든다고 생각할 수 있다.
             *
             * 압축해제의 경우, 압축해제할 이미지의 크기가 크면 클 수록 용량이 기하 급수적으로 늘어나며 문제에서 요구된 2^20 x 2^20 사이즈의 경우
             * 페타바이트를 넘는 크기를 가지므로 압축해제를 하여 뒤집을 경우 64MB를 초과하게된다. 압축 해제하지않고 상하를 반전시키는 것은 다음 함수 'UpSideDown'에서
             * 확인하길 바란다.
             */
            public int[,] Decompress(string data, int size)
            {
                int[,] decompressed = new int[size, size];
                Decompress(data.GetEnumerator(), 0, 0, size, decompressed);
                return decompressed;
            }
            private void Decompress(CharEnumerator eachCharacter, int y, int x, int size, int[,] decompressed)
            {
                eachCharacter.MoveNext();
                char current = eachCharacter.Current;

                // 기저사례 : w나 b를 만났을 경우, 이하 모든 칸은 w나 b로 칠해져야한다.
                if (current == 'b' || current == 'w')
                {
                    for (int row = 0; row < size; row++)
                    {
                        for (int column = 0; column < size; column++)
                        {
                            decompressed[y + row, x + column] = CharToColor(current);
                        }
                    }
                }
                // 글자가 w나 b가 아닐 경우, 재귀적으로 해당 공간을 4칸으로 분할해서 판단해야한다.
                else
                {
                    int half = size / 2;
                    // 좌상단
                    Decompress(eachCharacter, y, x, half, decompressed);
                    // 우상단
                    Decompress(eachCharacter, y, x + half, half, decompressed);

                    // 좌하단
                    Decompress(eachCharacter, y + half, x, half, decompressed);
                    // 우하단
                    Decompress(eachCharacter, y + half, x + half, half, decompressed);
                }
            }

            public string UpSideDown(string data)
            {
                return UpSideDown(data.GetEnumerator());
            }
            private string UpSideDown(CharEnumerator enumerator)
            {
                enumerator.MoveNext();
                char current = enumerator.Current;

                // 기저사례 : b나 w는 자신이 차지하는 공간이 모두 한색임을 의미하므로 뒤집지 않는다. (뒤집으나 아니나 같은 색임)
                if (current == 'b' || current == 'w')
                {
                    return new string(current, 1);
                }

                // 재귀적으로 하위 공간을 타고 들어간다.
                string leftUpper = UpSideDown(enumerator);
                string rightUpper = UpSideDown(enumerator);
                string leftBottom = UpSideDown(enumerator);
                string rightBottom = UpSideDown(enumerator);

                // 상하를 뒤집고, 하나의 공간을 뒤집어서 리턴할 것이므로 x를 표시해주어야한다.
                return "x" + leftBottom + rightBottom + leftUpper + rightUpper;
            }

            private int CharToColor(char c)
            {
                if (c == 'b')
                    return 0;
                else if (c == 'w')
                    return 1;

                return 0;
            }
        }

        class Image
        {
            private int[,] _color;
            private string _fill = "■";
            private string _blank = "□";

            public Image(int[,] color)
            {
                _color = color;
            }

            public void Print()
            {
                string result = "";
                for (int row = 0; row < _color.GetLength(0); row++)
                {
                    for (int column = 0; column < _color.GetLength(1); column++)
                    {
                        string color = _color[row, column] == 0 ? _fill : _blank;
                        result += color;
                    }
                    result += "\n";
                }

                Console.Write($"{result}");
            }
        }
    }
}
