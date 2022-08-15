using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _09_SmokeBasins : AoCDay
    {
        protected override void Run()
        {
            int lengthY = inputLines.Length, lengthX = inputLines[0].Length;
            int[,] map = GridParse();

            int result = 0;
            List<(int, int)> lows = new();
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                {
                    bool low = true;
                    foreach (var (neiY, neiX) in Neighbors(y, x, lengthY, lengthX))
                        if (map[neiY, neiX] <= map[y, x])
                        {
                            low = false;
                            break;
                        }
                    if (low)
                    {
                        lows.Add((y, x));
                        result += map[y, x] + 1;
                    }
                }
            part1 = result;

            result = 1;
            List<(int, int)> Basin((int, int) low)
            {
                List<(int, int)> result = new() { low };
                List<List<(int, int)>> basin = new() { new() { low } };
                for (int i = 1; basin[i - 1].Count != 0; i++)
                {
                    basin.Add(new());
                    foreach (var (prevY, prevX) in basin[i - 1])
                        foreach (var (y, x) in Neighbors(prevY, prevX, lengthY, lengthX))
                            if (map[y, x] != 9)
                                basin[i].Add((y, x));
                    if (i > 1)
                        basin[i] = basin[i].Distinct().Except(basin[i - 2]).ToList();
                    result.AddRange(basin[i]);
                }
                return result;
            }
            int[] sizes = new int[lows.Count];
            for (int l = 0; l < lows.Count; l++)
                sizes[l] = Basin(lows[l]).Count;
            Array.Sort(sizes);
            foreach (int s in sizes[^3..])
                result *= s;
            part2 = result;
        }
    }
}
