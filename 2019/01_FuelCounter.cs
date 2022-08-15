using System;

namespace Advent_of_Code._2019
{
    class _01_FuelCounter : AoCDay
    {
        public override void Run()
        {
            int[] masses = Array.ConvertAll(inputLines, int.Parse);
            foreach (int mass in masses)
            {
                int fuel = (int)Math.Floor((decimal)mass / 3) - 2;
                part1 += fuel;
                while (fuel > 0)
                {
                    part2 += fuel;
                    fuel = (int)Math.Floor((decimal)fuel / 3) - 2;
                }
            }
        }
    }
}
