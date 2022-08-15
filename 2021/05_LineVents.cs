using System;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _05_LineVents : AoCDay
    {
        struct Line
        {
            public int x1, x2, y1, y2;
            public Line(int[] xy)
            {
                x1 = xy[0];
                y1 = xy[1];
                x2 = xy[2];
                y2 = xy[3];
            }
            public override string ToString()
                => x1 + "," + y1 + " -> " + x2 + "," + y2;
        }
        public override void Run()
        {
            Line[] lines = new Line[inputLines.Length];
            int max = 0;
            for (int i = 0; i < inputLines.Length; i++)
            {
                string[] xy = inputLines[i].Split(" -> ");
                int[] xy12 = new int[4];
                for (int j = 0; j < 2; j++)
                {
                    xy12[j] = int.Parse(xy[0].Split(',')[j]);
                    xy12[j + 2] = int.Parse(xy[1].Split(',')[j]);
                    if (xy12.Max() > max) max = xy12.Max();
                }
                lines[i] = new(xy12);
            }

            int[,] grid = new int[max + 1, max + 1];
            foreach (Line l in lines)
            {
                if (l.x1 == l.x2)
                    for (int y = Math.Min(l.y1, l.y2); y <= Math.Max(l.y1, l.y2); y++)
                        grid[l.x1, y]++;
                else if (l.y1 == l.y2)
                    for (int x = Math.Min(l.x1, l.x2); x <= Math.Max(l.x1, l.x2); x++)
                        grid[x, l.y1]++;
            }

            int Result()
            {
                int result = 0;
                foreach (int i in grid)
                    if (i >= 2) result++;
                return result;
            }
            part1 = Result();

            foreach (Line l in lines)
            {
                if (l.x1 != l.x2 && l.y1 != l.y2)
                {
                    int minX = Math.Min(l.x1, l.x2), maxX = Math.Max(l.x1, l.x2),
                        minY = Math.Min(l.y1, l.y2), maxY = Math.Max(l.y1, l.y2);
                    if (l.x1 < l.x2 == l.y1 < l.y2)
                        for (int x = minX, y = minY; x <= maxX && y <= maxY;)
                            grid[x++, y++]++;
                    else
                        for (int x = minX, y = maxY; x <= maxX && y >= minY;)
                            grid[x++, y--]++;
                }
            }
            part2 = Result();
        }
    }
}