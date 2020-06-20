using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 시계맞추기_p168_
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 이 문제는 12시(0시), 3시, 6시, 9시 총 4가지의 시간이 반복된다는 것을 먼저 깨달아야한다.
             * 4번 이후부터는 시간이 계속 반복되므로, 모든 경우의 수는 4^10(버튼 갯수가 10개이다.)번이 된다. (대략 백만번정도됨)
             */

            int[] sampleCase = new int[16]
            {
                3, 0, 6, 0,
                0, 9, 3, 0,
                3, 0, 6, 3,
                0, 3, 0, 6
            };
            int[] testCase01 = new int[16]
            {
                12, 6, 6, 6,
                6, 6, 12, 12,
                12, 12, 12, 12,
                12, 12, 12, 12
            };
            int[] testCase02 = new int[16]
            {
                12, 9, 3, 12,
                6, 6, 9, 3,
                12, 9, 12, 9,
                12, 12, 6, 6
            };


            ClockBoard board = new ClockBoard(4,4, 10);
            board.SetClocks(testCase01);

            board.SetButtonLink(0, 0, 1, 2);
            board.SetButtonLink(1, 3, 7, 9, 11);
            board.SetButtonLink(2, 4, 10, 14, 15);
            board.SetButtonLink(3, 0, 4, 5, 6, 7);
            board.SetButtonLink(4, 6, 7, 8, 10, 12);
            board.SetButtonLink(5, 0, 2, 14, 15);
            board.SetButtonLink(6, 3, 14, 15);
            board.SetButtonLink(7, 4, 5, 7, 14, 15);
            board.SetButtonLink(8, 1, 2, 3, 4, 5);
            board.SetButtonLink(9, 3, 4, 5, 9, 13);

            Console.WriteLine($"버튼을 누른 횟수:{new AlignClockCalculator(board, 0).ComputeMinimumButtonPressCount()}");
        }
    }

    class AlignClockCalculator
    {
        public AlignClockCalculator(ClockBoard board, int alignTime)
        {
            this.board = board;
            this.alignTime = alignTime;
        }

        private ClockBoard board;
        private int alignTime;

        public int ComputeMinimumButtonPressCount()
        {
            int startButton = 0;
            int buttonPushedCount = 0;
            int result = 0;

            Solve(startButton, buttonPushedCount, ref result);

            return result;
        }

        private void Solve(int startButton, int buttonPushCount, ref int result)
        {
            // 마지막 버튼을 거친 다음 모든 시계가 원하는 방향으로 정렬되었는 지 확인해야한다.
            if (startButton < 0 || startButton >= board.ButtonCount)
            {
                if (board.IsAllAligned(alignTime))
                {
                    // 모든 버튼을 누르는 경우의 수를 다루므로, 항상 Align이 되는 경우는 마지막 startButton까지 버튼을 누를 지 정하고 난 때이다.
                    result = buttonPushCount;
                }

                return;
            }

            // 버튼을 4번 누르면 제자리로 돌아온다.
            for (int i = 0; i < 4; i++)
            {
                // 먼저 Solve함수를 호출하는 것으로 누르지 않았던 경우를 셈할 수 있고, 마지막에 누르는 4번째 버튼의 경우 셈을 하지 않는다. 4번째 누르는 것은 다시 원래 자리로 돌려주는 행위이기 때문이다.
                Solve(startButton + 1, buttonPushCount, ref result);

                board.PushButton(startButton);
                buttonPushCount += 1; // 현재 버튼의 누른 횟수를 기록
            }
        }
    }

    class ClockBoard
    {
        public ClockBoard(int width, int height, int buttonCount)
        {
            _width = width;
            _height = height;
            _total = width * height;

            _clocks = new Clock[_total];

            _buttons = new int[buttonCount, _total];
            this._buttonCount = buttonCount;
        }

        private Clock[] _clocks;
        private int[,] _buttons;

        private int _total;
        private int _width, _height;
        private int _buttonCount;

        public int ButtonCount => this._buttonCount;

        public void SetClocks(int[] clocks)
        {
            for (int i = 0; i < clocks.Length; i++)
            {
                _clocks[i] = new Clock(clocks[i]);
            }
        }
        public void SetClocks(int number, Clock clock)
        {
            _clocks[number] = clock;
        }
        public void SetButtonLink(int button, params int[] link)
        {
            for (int i = 0; i < link.Length; i++)
            {
                _buttons[button, link[i]] = 1;
            }
        }

        public void PushButton(int button)
        {
            for (int clock = 0; clock < _total; clock++)
            {
                if (_buttons[button, clock] > 0)
                {
                    _clocks[clock].Increament();
                }
            }
        }

        public bool IsAllAligned(int time)
        {
            foreach (Clock clock in this)
            {
                if (clock.Time != time)
                    return false;
            }

            return true;
        }

        public void Print()
        {
            int index = 0;
            foreach (Clock clock in this)
            {
                Console.WriteLine($"Clock:{index} || Time:{clock.Time}");
                index++;
            }
        }

        public IEnumerator<Clock> GetEnumerator()
        {
            for (int i = 0; i < _total; i++)
            {
                yield return _clocks[i];
            }
        }
    }

    class Clock
    {
        public Clock(int initialTime, int cycle = 12, int perMove = 3)
        {
            this.m_hour = initialTime % cycle;
            this.m_perMove = perMove;
            this.m_cycle = cycle;
        }

        private int m_hour;
        private int m_perMove;
        private int m_cycle;

        public int Time => m_hour;

        public void Increament()
        {
            m_hour = RepeatClock(m_hour + m_perMove, m_cycle);
        }
        public void Decreament()
        {
            m_hour = RepeatClock(m_hour - m_perMove, m_cycle);
        }

        private int RepeatClock(int value, int repeat)
        {
            double betweenValue = Utility.Repeat((double)value, (double)repeat);
            double result = (betweenValue + ((betweenValue > (double)0) ? (double)0 : (double)repeat)) % repeat;
            return (int) result;
        }
    }


    static class Utility
    {
        public static double Repeat(double value, double repeat)
        {
            double multipier = System.Math.Floor(value / repeat);
            double reapeatedValue = value - (repeat * multipier);
            return reapeatedValue;
        }
    }
}