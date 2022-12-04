using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _03_Rucksacks : AoCDay
    {
        protected override void Run()
        {
            foreach (string line in inputLines)
                foreach (char item in line[..(line.Length / 2)])
                    if (line[(line.Length / 2)..].Contains(item))
                    {
                        part1 += Priority(item);
                        break;
                    }

            for (int i = 0; i < inputLines.Length; i += 3)
                foreach (char item in inputLines[i])
                    if (inputLines[i + 1].Contains(item) && inputLines[i + 2].Contains(item))
                    {
                        part2 += Priority(item);
                        break;
                    }
        }

        int Priority(char item)
        {
            if (item >= 'a') return item - 'a' + 1;
            return item - 'A' + 27;
        }
    }
}
