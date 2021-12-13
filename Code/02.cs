using System;

namespace Advent_of_Code
{
    static class Pilot
    {
        public static void Run()
        {
            var input = System.IO.File.ReadAllLines("02.txt");

            int move = 0, depth = 0;
            foreach (string line in input)
            {
                string dir = line[0..^1];
                int dist = int.Parse(line[^1].ToString());
                if (dir == "forward ")
                    move += dist;
                else if (dir == "down ") depth += dist;
                else depth -= dist;
            }
            Console.WriteLine(move * depth);

            move = 0; depth = 0; int aim = 0;
            foreach (string line in input)
            {
                string dir = line[0..^1];
                int dist = int.Parse(line[^1].ToString());
                if (dir == "forward ")
                {
                    move += dist;
                    depth += aim * dist;
                }
                else if (dir == "down ") aim += dist;
                else aim -= dist;
            }
            Console.WriteLine(move * depth);
        }
    }
}
