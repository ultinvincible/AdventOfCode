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
            debug = true;
            hill = GridParse(inputLines, (row, col) =>
            {
                switch (inputLines[row][col])
                {
                    case 'S': start = (row, col); return 0;
                    case 'E': end = (row, col); return 25;
                    default: return inputLines[row][col] - 'a';
                }
            });

            List<(int weight, int prev)> map = Dijkstras((int i) => Step(i),
            (int i) => i == end.row * hill.GetLength(1) + end.col,
            start.row * hill.GetLength(1) + start.col);
            part1 = map[end.row * hill.GetLength(1) + end.col].weight;

            map = Dijkstras((int i) => Step(i, true),
                start: end.row * hill.GetLength(1) + end.col);
            //if(debug)
            //{
            //    int current = end.row * hill.GetLength(1) + end.col;
            //    while(current!=start.row * hill.GetLength(1) + start.col)
            //    {
            //        Console.Write(Math.DivRem(current, hill.GetLength(1)));
            //        current = map[current].prev;
            //    }
            //}
            part2 = int.MaxValue;
            for (int i = 0; i < map.Count; i++)
                if (hill[Math.DivRem(i, hill.GetLength(1), out int col), col] == 0)
                    part2 = Math.Min(part2, map[i].weight);
        }

        private static List<(int nei, int weight)> Step(int i, bool reverse = false)
        {
            List<(int, int)> result = new();
            (int row, int col) = Math.DivRem(i, hill.GetLength(1));
            foreach ((int neiRow, int neiCol) in
                Neighbors(row, col, hill.GetLength(0), hill.GetLength(1)))
            {
                if (!reverse && hill[neiRow, neiCol] - hill[row, col] <= 1)
                    result.Add((neiRow * hill.GetLength(1) + neiCol, 1));

                if (reverse && hill[neiRow, neiCol] - hill[row, col] >= -1)
                    result.Add((neiRow * hill.GetLength(1) + neiCol, 1));
            }
            return result;
        }
    }
}
