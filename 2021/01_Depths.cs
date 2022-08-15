using System;

namespace Advent_of_Code._2021
{
    class _01_Depths : AoCDay
    {
        int[] depths;
        int CountIncreases(int distance)
        {
            int count = 0;
            for (int i = 0; i < depths.Length - distance; i++)
                if (depths[i] < depths[i + distance])
                    count++;
            return count;
        }
        public override void Run()
        {
            depths = Array.ConvertAll(inputLines, int.Parse);
            (part1,part2) = (CountIncreases(1), CountIncreases(3));
        }
    }
}