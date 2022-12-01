﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _01_Calories : AoCDay
    {
        protected override void Run()
        {
            string[] reindeers = input.Split("\n\n");
            int[] calories = new int[reindeers.Length];
            //int max = 0;
            for (int r = 0; r < reindeers.Length; r++)
            {
                foreach (string food in reindeers[r].Split("\n",
                    StringSplitOptions.RemoveEmptyEntries))
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
