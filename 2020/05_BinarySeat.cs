using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _05_BinarySeat : AoCDay
    {
        protected override void Run(out (object part1, object part2) answer)
        {
            int max = 0; List<int> IDs = new();
            foreach (string line in inputLines)
            {
                string row = "", column = "";
                for (int i = 0; i < 7; i++)
                {
                    if (line[i] == 'F') row += '0';
                    else if (line[i] == 'B') row += '1';
                }
                for (int i = 7; i < 10; i++)
                {
                    if (line[i] == 'L') column += '0';
                    else if (line[i] == 'R') column += '1';
                }
                int id = Convert.ToInt32(row, 2) * 8 +
                         Convert.ToInt32(column, 2);
                max = Math.Max(max, id);
                IDs.Add(id);
            }
            answer = (max, default);
            IDs.Sort();
            for (int i = 1; i < IDs.Count; i++)
                if (IDs[i] == IDs[i - 1] + 2)
                    answer.part2 = IDs[i] - 1;
        }
    }
}
