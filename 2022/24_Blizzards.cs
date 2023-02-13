using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    internal class _24_Blizzards : AoCDay
    {
        static (int dimension, int value)[] directions = new (int dimension, int value)[]
        {
            (1, 1), (0, 1), (1, -1), (0, -1), (0, 0)
        };
        static readonly char[] facing = new char[] { '>', 'v', '<', '^' };
        readonly List<List<int>[,]> maps = new();
        int[] length = new int[2];
        (int row, int col) start = (-1, 0), dest;
        protected override void Run()
        {
            debug = 0;
            length[0] = inputLines.Length - 2;
            length[1] = inputLines[0].Length - 2;
            maps.Add(new List<int>[length[0], length[1]]);
            for (int row = 0; row < length[0]; row++)
                for (int col = 0; col < length[1]; col++)
                {
                    maps[0][row, col] = new();
                    char dir = inputLines[row + 1][col + 1];
                    if (dir != ' ' && dir != '.')
                        maps[0][row, col].Add(Array.IndexOf(facing, dir));
                }
            dest = (length[0], length[1] - 1);

            Dictionary<(int row, int col, int minute), ((int row, int col, int minute) prev, int cost)> tree =
                Dijkstras(Next, (start.row, start.col, 0), s =>
                {
                    bool isDest = (s.row, s.col) == dest;
                    if (isDest) part1 = s.Item3;
                    return isDest;
                });
            if (debug == 2)
            {
                List<(int row, int col, int minute)> path = new() { (dest.row, dest.col, (int)part1) };
                do path.Add(tree[path[^1]].prev);
                while (path[^1] != (start.row, start.col, 0));
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    (int row, int col, int minute) = path[i];
                    PrintMap(row, col, minute);
                }
            }

            part2 = part1;
            tree = Dijkstras(Next, (dest.row, dest.col, (int)part2), s =>
            {
                bool isDest = (s.row, s.col) == start;
                if (isDest) part2 = s.Item3;
                return isDest;
            });
            tree = Dijkstras(Next, (start.row, start.col, (int)part2), s =>
            {
                bool isDest = (s.row, s.col) == dest;
                if (isDest) part2 = s.Item3;
                return isDest;
            });
        }
        List<((int row, int col, int minute) state, int cost)> Next((int row, int col, int minute) state)
        {
            (int posRow, int posCol, int minute) = state;
            if (maps.Count == minute + 1)
            {
                maps.Add(new List<int>[length[0], length[1]]);
                for (int row = 0; row < length[0]; row++)
                    for (int col = 0; col < length[1]; col++)
                        maps[^1][row, col] = new();
                for (int row = 0; row < length[0]; row++)
                    for (int col = 0; col < length[1]; col++)
                        foreach (int dir in maps[^2][row, col])
                        {
                            (int dimension, int value) = directions[dir];
                            int[] newPosition = new int[] { row, col };
                            newPosition[dimension] += value;
                            if (newPosition[dimension] == length[dimension]) newPosition[dimension] = 0;
                            else if (newPosition[dimension] == -1) newPosition[dimension] = length[dimension] - 1;
                            maps[^1][newPosition[0], newPosition[1]].Add(dir);
                        }
            }
            if (debug == 1)
            {
                Console.WriteLine((posRow, posCol, minute));
                PrintMap(posRow, posCol, maps.Count - 1);
            }

            List<int>[,] map = maps[^1];
            List<(int row, int col, int minute)> next;
            if ((posRow, posCol) == start) next = new() { (0, 0, minute + 1) };
            else if ((posRow, posCol) == dest) next = new() { (length[0] - 1, length[1] - 1, minute + 1) };
            else
            {
                next = new();
                foreach ((int dimension, int value) in directions)
                {
                    int[] newPosition = new int[] { posRow, posCol }; ;
                    newPosition[dimension] += value;
                    if ((newPosition[0], newPosition[1]) == start || (newPosition[0], newPosition[1]) == dest)
                    {
                        next.Add((newPosition[0], newPosition[1], minute + 1));
                        continue;
                    }
                    if (newPosition[dimension] == length[dimension] ||
                       newPosition[dimension] == -1 ||
                       map[newPosition[0], newPosition[1]].Count != 0) // hit a wall or blizzard
                        continue;
                    next.Add((newPosition[0], newPosition[1], minute + 1));
                }
            }

            return next.ConvertAll(s => (s, 1));
        }

        private void PrintMap(int row, int col, int minute)
        {
            Console.WriteLine("Minute " + minute);
            Console.WriteLine(GridPrint(maps[minute], (r, c) =>
            {
                List<int> l = maps[minute][r, c];
                return (r, c) == (row, col) ? "E" :
                    l.Count == 0 ? "." :
                    l.Count == 1 ? facing[l[0]].ToString() :
                    l.Count.ToString();
            }));
        }
    }
}
