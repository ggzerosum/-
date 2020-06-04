#define Question01
#define Question02

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 재귀_보글게임
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if Question01
            // 모든 조합을 찾는 문제
            List<int> result = new List<int>(0);
            new FindAllCombination().Compute(3, ref result, 2);
#endif

            // 보글 게임
#if Question02
           BoggleGame game = new BoggleGame(5, 5);
           game.SetRow(new char[5] { 'U', 'R', 'L', 'P', 'M'}, 0);
           game.SetRow(new char[5] { 'X', 'P', 'R', 'E', 'T' }, 1);
           game.SetRow(new char[5] { 'G', 'I', 'A', 'E', 'T' }, 2);
           game.SetRow(new char[5] { 'X', 'T', 'N', 'Z', 'Y' }, 3);
           game.SetRow(new char[5] { 'X', 'O', 'Q', 'R', 'S' }, 4);

           game.Print();

           game.HasWord(0, 2, "GIRL");
#endif
        }
    }

    /// <summary>
    /// 0부터 차례대로 번호가 매겨진 N개의 원소 중 M개의 모든 조합을 찾습니다.
    /// </summary>
    internal class FindAllCombination
    {
        /*
         * 모든 조합의 경우는 재귀로 풀어내기 좋은 문제이다.
         * N개의 원소의 모든 경우의 수는 다음과 같이 생각해볼 수 있다.
         *
         * 모든 경우의 수를 X(자리수) + (X + 1)의 반복으로 생각해보자.
         * 이 때, X + 1 이상의 모든 자릿수에는 X 이하에서 사용한 숫자를 써서는 안된다. X를 가장 작은 수로 설정하여 X + 1 은 X보다 한단계 큰 수가 되도록 하면
         * 좀 더 직관적인 알고리즘이 된다. (순서를 고려하기 쉽다.)
         */
        public void Compute(int n, ref List<int> picked, int toPick)
        {
            // 더이상 고를 원소가 없을 경우 출력
            if (toPick == 0)
            {
                Print(in picked);
                return;
            }

            // 다음에 사용할 가장 작은 수를 찾는다.
            int smallest = picked.Count <= 0 ? 0 : picked[(picked.Count - 1)] + 1;
            for (int next = smallest; next < n; next++)
            {
                picked.Add(next); // 다음 숫자 입력
                Compute(n, ref picked, toPick - 1); // 한단계 다음 (X - 1
                picked.RemoveAt(picked.Count - 1); // 현재 자릿수에 입력한 숫자를 빼낸다.
            }
        }
        private void Print(in List<int> picked)
        {
            string result = "[(";
            for (int i = 0; i < picked.Count; i++)
            {
                result += $"{picked[i]}" + (i < picked.Count - 1 ? ", " : "");
            }
            result += ")]";

            Console.WriteLine(result);
        }
    }

    internal class BoggleGame
    {
        public BoggleGame(int row, int column)
        {
            this.CreateBoard(row, column);
        }

        // 게임판
        private char[,] gameBoard;
        private int rowUpperBound, columnUpperBound;

        private void CreateBoard(int row, int column)
        {
            this.gameBoard = new char[row, column];
            this.rowUpperBound = row - 1;
            this.columnUpperBound = column - 1;
        }
        public void SetRow(char[] row, int rowIndex)
        {
            for (int columnindex = 0; columnindex <= this.columnUpperBound; columnindex++)
            {
                gameBoard[rowIndex, columnindex] = row[columnindex];
            }
        }
        public void SetColumn(char[] column, int columnIndex)
        {
            for (int rowIndex = 0; rowIndex <= rowUpperBound; rowIndex++)
            {
                gameBoard[rowIndex, columnIndex] = column[rowIndex];
            }
        }

        public void Print()
        {
            for (int i = 0; i <= rowUpperBound; i++)
            {
                for (int j = 0; j <= columnUpperBound; j++)
                {
                    Console.Write($"'{this.gameBoard[i, j]}' ");
                }
                Console.WriteLine();
            }
        }

        #region 나의 풀이
        // HasWord(x, y, word) = 보글 게임판의 x, y에서 시작하는 단어 word의 존재 여부를 반환한다.
        public bool HasWord(int x, int y, string word)
        {
            var block = new Stack<int>();
            int result = HasWord_Internal(x, y, word, 0, ref block);
            Console.WriteLine($"{result}개의 매칭이 발견되었습니다.");
            return result > 0;
        }
        private int HasWord_Internal(int x, int y, string match, int tokenIndex, ref Stack<int> block)
        {
            if ((x < 0 || x > columnUpperBound) || (y < 0 || y > rowUpperBound))
            {
                return 0;
            }

            char token = match[tokenIndex];
            int index = CoordinateToNumber(x, y);
            if (token.Equals(gameBoard[y, x]) && !block.Contains(index))
            {
                block.Push(index);

                if (tokenIndex >= match.Length - 1)
                {
                    return 1;
                }

                int up = HasWord_Internal(x, y - 1, match, tokenIndex + 1, ref block);
                int upRight = HasWord_Internal(x + 1, y - 1, match, tokenIndex + 1, ref block);
                int right = HasWord_Internal(x + 1, y, match, tokenIndex + 1, ref block);
                int bottomRight = HasWord_Internal(x + 1, y + 1, match, tokenIndex + 1, ref block);
                int bottom = HasWord_Internal(x, y + 1, match, tokenIndex + 1, ref block);
                int bottomLeft = HasWord_Internal(x - 1, y + 1, match, tokenIndex + 1, ref block);
                int left = HasWord_Internal(x - 1, y, match, tokenIndex + 1, ref block);
                int upLeft = HasWord_Internal(x - 1, y - 1, match, tokenIndex + 1, ref block);

                int result = up + upRight + right + bottomRight + bottom + bottomLeft + left + upLeft;
                return result;
            }
            else
            {
                return 0;
            }
        }
        private int CoordinateToNumber(int x, int y)
        {
            return (y * (this.rowUpperBound + 1)) + x;
        }
        private void NumberToCoordinate(int number, ref int x, ref int y)
        {
            int row = number / (this.rowUpperBound + 1);
            int column = number % (this.columnUpperBound + 1);

            x = column;
            y = row;
        }
        #endregion
    }
}