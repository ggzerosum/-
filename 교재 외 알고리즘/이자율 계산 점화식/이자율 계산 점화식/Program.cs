using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 이자율_계산_점화식
{
    class Program
    {
        static void Main(string[] args)
        {
            float principal = 100f;
            float interestRate = 0.01f;
            int deltaMonth = 20;

            Deposit deposit = new Deposit();
            Console.WriteLine($"원금:{principal} 이자율:{interestRate} 기간(월):{deltaMonth}");
            // 단리
            float simpleInterest = deposit.GetSimpleInterest(principal, interestRate, deltaMonth);
            Console.WriteLine($"단리:{simpleInterest}");
            // 복리
            float compoundInterest = deposit.GetCompoundInterest(principal, interestRate, deltaMonth);
            Console.WriteLine($"복리:{compoundInterest}");

            int month = deposit.GetMonth(principal, compoundInterest, interestRate);
            Console.WriteLine($"복리 달성 가능 개월 수:{month}(should be equal with {deltaMonth})");
        }
    }

    class Deposit
    {
        // 단리 계산
        // 단리는 단순히 원금 * 이자율을 매달 더해나가는 형식
        public float GetSimpleInterest(float principal, float interestRate, int month)
        {
            float interest = 0f;
            for (int i = 0; i < month; i++)
            {
                interest += principal * interestRate;
            }

            return principal + interest;
        }

        // 복리 계산
        // 복리는 이전 달의 총 금액에 이자율을 더하는 방식.
        // An = A0 * (1 + interestRate)^n 이라는 점화식이 도출된다.
        // (이전 달의 금액 + (이전달 금액 * 이자율) = 이전달 금액 * (1 + (1*이자율)) = 다음달 금액 = 이전달 금액 * (1 + 이자율)
        public float GetCompoundInterest(float principal, float interestRate, int month)
        {
            return principal * GetCompoundInterestRate(interestRate, month);
        }
        private float GetCompoundInterestRate(float interestRate, int month)
        {
            if (month == 0)
                return 1f;

            return (1f + interestRate) * GetCompoundInterestRate(interestRate, month - 1);
        }

        // 복리 달성 가능한 개월 수 계산
        public int GetMonth(float principal, float desire, float interestRate)
        {
            return GetMonth(principal, desire, interestRate, 1f, 0);
        }
        private int GetMonth(float principal, float desire, float interestRate, float currentInterest, int month)
        {
            float compoundPrincipal = principal * currentInterest;
            if (compoundPrincipal >= desire)
                return month;

            currentInterest *= (1 + interestRate);

            return GetMonth(principal, desire, interestRate, currentInterest, month + 1);
        }
    }
}