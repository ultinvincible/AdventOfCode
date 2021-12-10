using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    static class SmokeLow
    {
        public static void Run()
        {
            string[] input = System.IO.File.ReadAllLines("9.txt");
            int lengthY = input.Length, lengthX = input[0].Length;
            int[][] map = new int[lengthY][];
            for (int y = 0; y < lengthY; y++)
            {
                map[y] = new int[lengthX];
                for (int x = 0; x < lengthX; x++)
                    map[y][x] = (int)char.GetNumericValue(input[y][x]);
            }
            List<(int, int)> Neighbors((int, int) yx)
            {
                (int y, int x) = yx;
                List<(int, int)> neis = new();
                foreach (var (neiY, neiX) in new List<(int, int)>() {
                    (y - 1, x), (y, x - 1),
                    (y, x + 1), (y + 1, x), })
                    if (neiY >= 0 && neiY < lengthY &&
                        neiX >= 0 && neiX < lengthX)
                        neis.Add((neiY, neiX));
                return neis;
            }

            int result = 0;
            List<(int, int)> lows = new();
            for (int i = 0; i < lengthY; i++)
                for (int j = 0; j < lengthX; j++)
                {
                    bool low = true;
                    foreach (var (neiI, neiJ) in Neighbors((i, j)))
                        if (map[neiI][neiJ] <= map[i][j])
                        {
                            low = false;
                            break;
                        }
                    if (low)
                    {
                        lows.Add((i, j));
                        result += map[i][j] + 1;
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
                    foreach (var prev in basin[i - 1])
                        foreach (var nei in Neighbors(prev))
                            if (map[nei.Item1][nei.Item2] != 9)
                                basin[i].Add(nei);
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
