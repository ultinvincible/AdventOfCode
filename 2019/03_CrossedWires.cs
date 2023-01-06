using System;

namespace Advent_of_Code._2019
{
    class _03_CrossedWires : AoCDay
    {
        (int, int) Move(int x, int y, char dir, int dist)
        {
            switch (dir)
            {
                case 'U': return (x, y + dist);
                case 'D': return (x, y - dist);
                case 'L': return (x - dist, y);
                case 'R': return (x + dist, y);
                default: throw new Exception("Invalid direction");
            }
        }
        const int max = 10000;
        protected override void Run()
        {
            string[][] wires = Array.ConvertAll
                (input.Split('\n', StringSplitOptions.RemoveEmptyEntries),
                s => s.Split(','));
            (int wire, int steps)[,] map = new (int, int)[2 * max + 1, 2 * max + 1];
            int minCrossX = 0, minCrossY = 0, minDist = int.MaxValue, minSteps = int.MaxValue;
            for (int w = 0; w < wires.Length; w++)
            {
                int x = max, y = max, steps = 0;
                foreach (string line in wires[w])
                {
                    int dist = int.Parse(line[1..]);
                    for (int i = 1; i <= dist; i++)
                    {
                        (int newX, int newY) = Move(x, y, line[0], i);
                        if (w == 0) map[newX, newY] = (1, steps + i);
                        else if (map[newX, newY].wire != 1) map[newX, newY].wire = 2;
                        else
                        {
                            map[newX, newY].wire = 3;
                            int newDist = Math.Abs(newX - max) + Math.Abs(newY - max);
                            if (newDist < minDist)
                            {
                                (minCrossX, minCrossY) = (newX, newY);
                                minDist = newDist;
                            }
                            minSteps = Math.Min(minSteps, steps + i + map[newX, newY].steps);
                        }
                    }
                    (x, y) = Move(x, y, line[0], dist);
                    steps += dist;
                    part1 = minDist;
                    part2 = minSteps;
                }
            }

            //string print = "";
            //for (int y = max + 200; y >= max - 100; y--)
            //{
            //    for (int x = max; x <= max + 200; x++)
            //    {
            //        if (x == max && y == max) print += "#";
            //        else if (map[x, y].wire == 0) print += ".";
            //        else print += map[x, y].wire;
            //    }
            //    print += "\n";
            //}
            //Console.WriteLine(print);
        }
    }
}
