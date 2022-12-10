using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2022
{
    class _09_Rope : AoCDay
    {
        static Dictionary<char, int[]> directions = new()
        {
            { 'L', new int[] { 0, 1 } },
            { 'R', new int[] { 0, -1 } },
            { 'U', new int[] { -1, 0 } },
            { 'D', new int[] { 1, 0 } },
        };
        protected override void Run()
        {
            //debug = true;
            int[] head = new int[2], tail = new int[2];
            HashSet<(int, int)> visited = new() { (0, 0) };
            foreach (string line in inputLines)
                for (int _ = 0; _ < int.Parse(line[2..]); _++)
                {
                    MoveHead(head, line[0]);
                    MoveTail(head, tail);
                    visited.Add((tail[0], tail[1]));
                }
            part1 = visited.Count;

            int[][] rope = new int[10][];
            for (int knot = 0; knot < 10; knot++)
                rope[knot] = new int[2];
            visited = new() { (0, 0) };
            foreach (string line in inputLines)
            {
                for (int _ = 0; _ < int.Parse(line[2..]); _++)
                {
                    MoveHead(rope[0], line[0]);
                    for (int knot = 1; knot <= 9; knot++)
                        MoveTail(rope[knot - 1], rope[knot]);
                    visited.Add((rope[^1][0], rope[^1][1]));
                }
            }
            if (debug) Console.WriteLine(PrintVisited(visited));
            part2 = visited.Count;
        }

        static void MoveHead(int[] head, char direction)
        {
            switch (direction)
            {
                case 'L':
                    head[1]--;
                    break;
                case 'R':
                    head[1]++;
                    break;
                case 'U':
                    head[0]--;
                    break;
                case 'D':
                    head[0]++;
                    break;
            }
        }
        static void MoveTail(int[] head, int[] tail)
        {
            int[] diff = new int[] { head[0] - tail[0], head[1] - tail[1] };
            for (int i = 0; i <= 1; i++)
                if (Math.Abs(diff[i]) > 1)
                {
                    tail[i] += diff[i] / 2;
                    if (diff[1 - i] != 0)
                        tail[1 - i] += diff[1 - i] / Math.Abs(diff[1 - i]);
                    break;
                }
        }
        string PrintVisited(HashSet<(int, int)> visited, int maxAbs = 12)
        {
            char[,] grid = new char[maxAbs * 2 + 1, maxAbs * 2 + 1];
            for (int row = 0; row < grid.GetLength(0); row++)
                for (int col = 0; col < grid.GetLength(1); col++)
                    grid[row, col] = visited.Contains
                       ((row - maxAbs, col - maxAbs)) ? '#' : '.';
            grid[maxAbs, maxAbs] = 's';
            return GridStr(grid);
        }
    }
}
