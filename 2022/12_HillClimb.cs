using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _12_HillClimb : AoCDay
    {
        (int row, int col) start = (0, 0), end = (0, 0);
        static int[,] hill;
        protected override void Run()
        {
            //debug = true;
            hill = GridParse(inputLines, (row, col) =>
            {
                switch (inputLines[row][col])
                {
                    case 'S': start = (row, col); return 0;
                    case 'E': end = (row, col); return 25;
                    default: return inputLines[row][col] - 'a';
                }
            });

            (int prevRow, int prevCol, int weight)[,] map = Dijkstras(hill,
                point => Step(point.row, point.col), start, end);
            part1 = map[end.row, end.col].weight;

            map = Dijkstras(hill,
                ((int row, int col) point) => Step(point.row, point.col, true), end);
            part2 = int.MaxValue;
            for (int row = 0; row < map.GetLength(0); row++)
                for (int col = 0; col < map.GetLength(1); col++)
                    if (hill[row, col] == 0)
                        part2 = Math.Min(part2, map[row, col].weight);
        }

        private static List<(int row, int col, int weight)> Step(int row, int col, bool reverse = false)
        {
            List<(int, int, int)> result = new();
            foreach ((int neiRow, int neiCol) in
                Neighbors(row, col, hill.GetLength(0), hill.GetLength(1)))
                if (!reverse && hill[neiRow, neiCol] - hill[row, col] <= 1 ||
                    (reverse && hill[neiRow, neiCol] - hill[row, col] >= -1))
                    result.Add((neiRow, neiCol, 1));
            return result;
        }
    }
}
