using System;

namespace Advent_of_Code._2022
{
    class _01_Calories : AoCDay
    {
        protected override void Run()
        {
            string[][] reindeers = inputSections;
            int[] calories = new int[reindeers.Length];
            //int max = 0;
            for (int r = 0; r < reindeers.Length; r++)
            {
                foreach (string food in reindeers[r])
                    calories[r] += int.Parse(food);
                //max = Math.Max(max, calories[r]);
            }
            //part1 = max;
            Array.Sort(calories);
            part1 = calories[^1];
            part2 = calories[^1] + calories[^2] + calories[^3];
        }
    }
}
