using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    class SmokeLow : AoCDay
    {
        public SmokeLow() : base(9) { }
        public override void Run()
        {
            int lengthY = input.Length, lengthX = input[0].Length;
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
            Console.WriteLine(result);

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
            Console.WriteLine(result);
        }
    }
}
