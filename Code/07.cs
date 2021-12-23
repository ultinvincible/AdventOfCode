﻿using System;
using System.Linq;

namespace Advent_of_Code
{
    class CrabSubmarines : AoCDay
    {
        public CrabSubmarines() : base(7, false) { }
        public override void Run()
        {
            int[] positions = Array.ConvertAll(inputString.Split(','), s => int.Parse(s));

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
            Console.WriteLine(minFuel);

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
            Console.WriteLine(minFuel);
        }
    }
}
