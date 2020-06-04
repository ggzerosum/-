#define _SLIENT_

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 게임판_덮기
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream file = new FileStream("data.txt", FileMode.Open);
            StreamReader fileReader = new StreamReader(file);

            int testCase = Convert.ToInt32(fileReader.ReadLine());
            for (int i = 0; i < testCase; i++)
            {
                string widthAndHeight = fileReader.ReadLine();
                string[] wh = widthAndHeight.Split(' ');
                int inputHeight = Convert.ToInt32(wh[0]);
                int inputWidth = Convert.ToInt32(wh[1]);

                int[,] extractedBoard = new int[inputHeight, inputWidth];
                for (int rowIndex = 0; rowIndex < inputHeight; rowIndex++)
                {
                    string str = fileReader.ReadLine();
                    for (int columnIndex = 0; columnIndex < inputWidth; columnIndex++)
                    {
                        extractedBoard[rowIndex, columnIndex] = str[columnIndex] == '#' ? 1 : 0;
                    }
                }

                int result = new Solution().Solve(inputWidth, inputHeight, extractedBoard);
                Console.WriteLine($"Result : {result}");
            }
        }
    }

    class Solution
    {
        // 좌상단부터 채워나가는 형태만을 선택한다고할 때 Shape은 이런 모양만!!
        private int[,] shape =
        {
            {1,0 , 1,1},
            {1,0 , 0,1},
            {0,1 , 1,1},
            {0,1 , -1,1}
        };

        public int Solve(int width, int height, int[,] board)
        {
            int wh = width * height;
            int[] newBoard = new int[wh];

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int index = ToOneDimension(width, row, column);
                    newBoard[index] = board[row, column];
                }
            }

            return Solve(width, height, newBoard);
        }

        private int Solve(int width, int height, int[] board)
        {
            // 좌상단 -> 우하단으로 가장 빠른 인덱스를 찾으면서 진행
            int validIndex = -1;
            for (int i = 0; i < board.Length; i++)
            {
                if (!IsFilled(i, board)) // 선택한 중심이 아직 덮히지 않았을 경우
                {
                    validIndex = i;
                    break;
                }
            }

            if (validIndex == -1) // 좌상단 -> 우상단으로 검색을 마쳤을 때 모든 셀이 채워져있을 경우
                return 1;

            int result = 0;
            for (int coverType = 0; coverType < 4; coverType++)
            {
                // 빈 셀을 중심으로 미리 지정해둔 4가지 모양을 덮을 수 있는 지 판단
                if (CanCover(width, height, validIndex, coverType, board))
                {
                    // 덮을 수 있다면 칸을 채운다.
                    IncrementCoverTypeCells(width, height, validIndex, coverType, 1, board);

                    // 칸을 채우고 다음 빈 셀을 찾아 같은 알고리즘을 반복한다.
                    result += Solve(width, height, board);

                    IncrementCoverTypeCells(width, height, validIndex, coverType, -1, board);
                }
            }

            return result;
        }

        private void IncrementCoverTypeCells(int width, int height, int centerIndex, int coverType, int delta, int[] board)
        {
            for (int vertices = 0; vertices < 4; vertices += 2)
            {
                int localColumn = shape[coverType, vertices];
                int localRow = shape[coverType, vertices + 1];

                ToTwoDimension(width, centerIndex, out int centerRow, out int centerColumn);

                int row = centerRow + localRow;
                int column = centerColumn + localColumn;

                board[centerIndex] += delta;
                board[ToOneDimension(width, row, column)] += delta;
            }
        }
        private bool CanCover(int width, int height, int centerIndex, int coverType, int[] board)
        {
            bool canCover = true;
            for (int vertices = 0; vertices < 4; vertices += 2)
            {
                int localColumn = shape[coverType, vertices];
                int localRow = shape[coverType, vertices + 1];

                ToTwoDimension(width, centerIndex, out int centerRow, out int centerColumn);

                int row = centerRow + localRow;
                int column = centerColumn + localColumn;

                bool isInRange = Solution.IsInRange(0, width - 1, column) && Solution.IsInRange(0, height - 1, row);
                if (!isInRange)
                {
                    canCover = false;
                }
                else if (Solution.IsFilled(width, row, column, board)) // Range안에 있을 경우
                {
                    canCover = false;
                }
            }

            return canCover;
        }

        public static int ToOneDimension(int width, int row, int column)
        {
            int w = row * width;
            return w + column;
        }
        public static void ToTwoDimension(int width, int number, out int row, out int column)
        {
            column = number % width;
            row = number / width;
        }

        public static bool IsInRange(int min, int max, int number)
        {
            return number >= min && number <= max;
        }
        public static bool IsFilled(int width, int row, int column, int[] board)
        {
            int index = ToOneDimension(width, row, column);
            return (board[index]) > 0;
        }
        public static bool IsFilled(int oneDimension, int[] board)
        {
            return board[oneDimension] > 0;
        }
    }
}