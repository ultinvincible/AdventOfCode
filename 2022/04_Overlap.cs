using System;

namespace Advent_of_Code._2022
{
    class _04_Overlap : AoCDay
    {
        protected override void Run()
        {
            foreach (string line in inputLines)
            {
                int[] values = Array.ConvertAll
                    (line.Split(new char[] { '-', ',' }), int.Parse);
                int min1 = values[0], max1 = values[1], min2 = values[2], max2 = values[3];

                if (min1 <= min2 && max2 <= max1 ||
                    (min2 <= min1 && max1 <= max2))
                    part1++;

                if (max1 >= min2 && max2 >= min1)
                    part2++;
            }
        }
    }
}
