using System;

namespace Advent_of_Code._2021
{
    class _02_Pilot : AoCDay
    {
        protected override void Run()
        {
            int move = 0, depth = 0;
            foreach (string line in inputLines)
            {
                string dir = line[0..^2];
                int dist = (int)char.GetNumericValue(line[^1]);
                if (dir == "forward")
                    move += dist;
                else if (dir == "down") depth += dist;
                else depth -= dist;
            }
            part1 = move * depth;

            move = 0; depth = 0; int aim = 0;
            foreach (string line in inputLines)
            {
                string dir = line[0..^2];
                int dist = (int)char.GetNumericValue(line[^1]);
                if (dir == "forward")
                {
                    move += dist;
                    depth += aim * dist;
                }
                else if (dir == "down") aim += dist;
                else aim -= dist;
            }
            part2 = move * depth;
        }
    }
}
