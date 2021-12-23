using System;

namespace Advent_of_Code
{
    class Pilot:AoCDay
    {
        public Pilot() : base(2) { }
        public override void Run()
        {
            int move = 0, depth = 0;
            foreach (string line in input)
            {
                string dir = line[0..^2];
                int dist = (int)char.GetNumericValue(line[^1]);
                if (dir == "forward")
                    move += dist;
                else if (dir == "down") depth += dist;
                else depth -= dist;
            }
            Console.WriteLine(move * depth);

            move = 0; depth = 0; int aim = 0;
            foreach (string line in input)
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
            Console.WriteLine(move * depth);
        }
    }
}
