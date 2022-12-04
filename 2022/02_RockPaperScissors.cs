using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _02_RockPaperScissors : AoCDay
    {
        protected override void Run()
        {
            (int, int)[] guide = new (int, int)[inputLines.Length];
            for (int l = 0; l < inputLines.Length; l++)
                guide[l] = (inputLines[l][0] - 'A', inputLines[l][2] - 'X');

            int total = 0;
            foreach ((int opp, int play) in guide)
            {
                int outcome = (play - opp + 4) % 3;
                total += outcome * 3 + play + 1;
            }
            part1 = total;

            total = 0;
            foreach ((int opp, int play) in guide)
            {
                int shape = (opp + play + 2) % 3;
                total += play * 3 + shape + 1;
            }
            part2 = total;
        }
    }
}
