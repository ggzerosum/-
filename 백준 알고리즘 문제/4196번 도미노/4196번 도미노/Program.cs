using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4196_Domino
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MAX_SIZE = 100000;
            int[] relationship = new int[MAX_SIZE];

            int testcase = Convert.ToInt32(Console.ReadLine());

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for (int i = 0; i < testcase; i++)
            {
                string[] cases = Console.ReadLine().Split(' ');
                int totalDomino = Convert.ToInt32(cases[0]);
                int relationshipCount = Convert.ToInt32(cases[1]);

                for (int j = 0; j < relationshipCount; j++)
                {
                    string[] relation = Console.ReadLine().Split(' ');
                    int domino = Convert.ToInt32(relation[0]);
                    int target = Convert.ToInt32(relation[1]);

                    relationship[domino] = target;
                }

                int fingerCount = 1;
                for (int dominoCount = 0; dominoCount < totalDomino; dominoCount++)
                {
                    if (relationship[dominoCount] == 0)
                        continue;

                    if (relationship[dominoCount] != dominoCount + 1)
                        continue;
                }

                Console.WriteLine(fingerCount);
                watch.Stop();

                Console.WriteLine($"ms:{watch.Elapsed.Milliseconds}");
            }
        }
    }
}