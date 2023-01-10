using System;

namespace Advent_of_Code._2022
{
    internal class _18_LavaCubes : AoCDay
    {
        const int maxIndex = 25;
        readonly string[] ToStr = new string[] { ".", "v", "^", "o" };
        bool[,,] map = new bool[maxIndex + 1, maxIndex + 1, maxIndex + 1];
        protected override void Run()
        {
            debug = 0;
            int[][] points = new int[inputLines.Length][];
            (int min, int max)[][,] bounds = new (int min, int max)[3][,];
            for (int i = 0; i < 3; i++)
                bounds[i] = new (int min, int max)[maxIndex + 1, maxIndex + 1];
            for (int line = 0; line < inputLines.Length; line++)
            {
                int[] point = Array.ConvertAll(inputLines[line].Split(','), i => int.Parse(i) + 2);
                map[point[0], point[1], point[2]] = true;
                points[line] = point;

                for (int dimension = 0; dimension < 3; dimension++)
                {
                    int i1 = point[(dimension + 1) % 3], i2 = point[(dimension + 2) % 3];
                    if (bounds[dimension][i1, i2] == (0, 0))
                        bounds[dimension][i1, i2] = (point[dimension], point[dimension]);
                    else bounds[dimension][i1, i2] = (
                        Math.Min(bounds[dimension][i1, i2].min, point[dimension]),
                        Math.Max(bounds[dimension][i1, i2].max, point[dimension]));
                }
            }

            bool[,,] exterior = new bool[maxIndex + 1, maxIndex + 1, maxIndex + 1];
            for (int dimension = 0; dimension < 3; dimension++)
                for (int i1 = 1; i1 < maxIndex; i1++)
                    for (int i2 = 1; i2 < maxIndex; i2++)
                    {
                        if (bounds[dimension][i1, i2] == (0, 0)) continue;
                        int[] line = new int[3];
                        line[(dimension + 1) % 3] = i1;
                        line[(dimension + 2) % 3] = i2;
                        (int min, int max) = bounds[dimension][i1, i2];
                        for (int i = 1; i < maxIndex; i++)
                        {
                            if (i == min) i = max + 1;
                            int[] point = Array.ConvertAll(line, _ => _);
                            point[dimension] = i;
                            exterior[point[0], point[1], point[2]] = true;
                            if (min - 2 <= i && i <= max + 2) // If not working change 2
                                ForEachNeiAir(point, nei =>
                                    exterior[nei[0], nei[1], nei[2]] = true);
                        }
                    }

            foreach (int[] point in points)
                ForEachNeiAir(point, nei =>
                {
                    part1++;
                    if (exterior[nei[0], nei[1], nei[2]])
                    {
                        part2++;
                        if (debug == 2) Console.WriteLine(string.Join(",", Array.ConvertAll(nei, i => i - 1)));
                    }
                });

            if (debug == 1)
            {
                string print = "";
                for (int depth = 1; depth < map.GetLength(0) - 1; depth++)
                {
                    print += $"Depth: {depth}\n0";
                    for (int col = 1; col < map.GetLength(2) - 1; col++)
                        print += col.ToString()[^1];
                    print += "\n";
                    for (int row = 1; row < map.GetLength(1) - 1; row++)
                    {
                        print += row.ToString()[^1];
                        for (int col = 1; col < map.GetLength(2) - 1; col++)
                            if (map[depth, row, col]) print += block;
                            else if (!exterior[depth, row, col]) print += " ";
                            else
                            {
                                int p = 0;
                                if (map[depth - 1, row, col]) p++;
                                if (map[depth + 1, row, col]) p += 2;
                                print += ToStr[p];
                            }
                        print += "\n";
                    }
                    print += "\n";
                }
                Console.WriteLine(print);
            }
        }

        // WARNING: Does not cover all edge cases

        void ForEachNeiAir(int[] point, Action<int[]> Action)
        {
            for (int side = 0; side < 3; side++)
                for (int dir = -1; dir <= 1; dir += 2)
                {
                    int[] nei = Array.ConvertAll(point, _ => _);
                    nei[side] += dir;
                    if (!map[nei[0], nei[1], nei[2]])
                        Action(nei);
                }
        }
    }
}
