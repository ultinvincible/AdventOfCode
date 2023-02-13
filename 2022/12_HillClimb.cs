using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _12_HillClimb : AoCDay
    {
        (int row, int col) start = default, end = default;
        static int[,] hill;
        protected override void Run()
        {
            debug = 0;
            hill = GridParse((row, col) =>
             {
                 switch (inputLines[row][col])
                 {
                     case 'S': start = (row, col); return 0;
                     case 'E': end = (row, col); return 25;
                     default: return inputLines[row][col] - 'a';
                 }
             });

            (int prevRow, int prevCol, int cost)[,] map =
                 Dijkstras(hill, point => Step(point.row, point.col), start, end);
            part1 = map[end.row, end.col].cost;

            map = Dijkstras(hill, point => Step(point.row, point.col, true), end);
            part2 = int.MaxValue;
            for (int row = 0; row < map.GetLength(0); row++)
                for (int col = 0; col < map.GetLength(1); col++)
                    if (hill[row, col] == 0)
                        part2 = Math.Min(part2, map[row, col].cost);
        }

        private static List<((int row, int col), int cost)> Step(int row, int col, bool reverse = false)
        {
            List<((int, int), int)> result = new();
            foreach ((int neiRow, int neiCol) in Neighbors(row, col, hill))
                if (!reverse && hill[neiRow, neiCol] - hill[row, col] <= 1 ||
                    (reverse && hill[neiRow, neiCol] - hill[row, col] >= -1))
                    result.Add(((neiRow, neiCol), 1));
            return result;
        }
    }
}
