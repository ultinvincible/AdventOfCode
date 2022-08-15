using System;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _07_CrabSubmarines : AoCDay
    {
        public override void Run()
        {
            int[] positions = Array.ConvertAll(input.Split(','),int.Parse);

            //int median = input[(input.Length - 1) / 2];
            int min = positions.Min(), max = positions.Max();

            int minFuel = int.MaxValue;
            for (int position = min; position < max; position++)
            {
                int fuel = 0;
                for (int crab = 0; crab < positions.Length; crab++)
                    fuel += Math.Abs(positions[crab] - position);
                if (minFuel > fuel)
                    minFuel = fuel;
            }
            part1 = minFuel;

            minFuel = int.MaxValue;
            for (int position = min; position < max; position++)
            {
                int fuel = 0;
                for (int crab = 0; crab < positions.Length; crab++)
                    for (int i = 1; i <= Math.Abs(positions[crab] - position); i++)
                        fuel += i;
                if (minFuel > fuel)
                    minFuel = fuel;
            }
            part2 = minFuel;
        }
    }
}
