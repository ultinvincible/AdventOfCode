using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _14_CoarseAndRough : AoCDay
    {
        bool[,] rock, blocked;
        (int row, int col) source;
        protected override void Run()
        {
            debug = 0;
            int minCol = int.MaxValue,
                maxRow = 0,
                maxCol = 0;
            List<(int row, int col)[]> rocks = new();
            foreach (string line in inputLines)
                rocks.Add(Array.ConvertAll(line.Split(" -> "), s =>
                    {
                        int[] split = Array.ConvertAll(s.Split(','), int.Parse);
                        minCol = Math.Min(minCol, split[0]);
                        maxRow = Math.Max(maxRow, split[1]);
                        maxCol = Math.Max(maxCol, split[0]);
                        return (split[1], split[0]);
                    }));
            maxRow++; minCol -= 2; maxCol += 2;
            rock = new bool[maxRow + 2, maxCol - minCol + 1];
            foreach ((int row, int col)[] points in rocks)
                for (int i = 0; i < points.Length - 1; i++)
                {
                    int dirRow = points[i].row < points[i + 1].row ? 1 : -1,
                        dirCol = points[i].col < points[i + 1].col ? 1 : -1;
                    for (int row = points[i].row;
                        (row - points[i + 1].row) * dirRow <= 0; row += dirRow)
                        for (int col = points[i].col;
                            (col - points[i + 1].col) * dirCol <= 0; col += dirCol)
                            rock[row, col - minCol] = true;
                }
            for (int col = 0; col < rock.GetLength(1); col++)
                rock[rock.GetLength(0) - 1, col] = true;

            blocked = rock.Clone() as bool[,];
            source = (0, 500 - minCol);
            List<(int row, int col)> sandPath = new() { source };
            PrintMap();
            int count = 0;
            while (!blocked[source.row, source.col])
            {
                if (sandPath[^1] != source)
                    while (true)
                    {
                        bool remove = true;
                        foreach ((int row, int col) point in Moves(sandPath[^1]))
                            if (!blocked[point.row, point.col])
                            {
                                remove = false;
                                break;
                            }
                        if (remove)
                            sandPath.RemoveAt(sandPath.Count - 1);
                        else break;
                    }

                bool move;
                do
                {
                    move = false;
                    foreach ((int row, int col) in Moves(sandPath[^1]))
                        if (!blocked[row, col])
                        {
                            sandPath.Add((row, col));
                            if (col == 0 || col == maxCol - minCol)
                                count += maxRow - row;
                            else move = true;
                            break;
                        }
                } while (move);
                blocked[sandPath[^1].row, sandPath[^1].col] = true;
                count++;
                if (part1 == 0 && sandPath[^1].row == maxRow)
                {
                    part1 = count - 1;
                    PrintMap();
                }
                sandPath.RemoveAt(sandPath.Count - 1);
            }
            part2 = count;
            PrintMap();
        }

        static (int, int)[] Moves((int row, int col) sand)
            => new (int, int)[] {
                (sand.row + 1, sand.col),
                (sand.row + 1, sand.col - 1),
                (sand.row + 1, sand.col + 1) };

        void PrintMap()
        {
            if (debug == 1) Console.WriteLine(GridStr(blocked, (row, col) =>
                (row, col) == source ? "+" : blocked[row, col] ? rock[row, col] ? "#" : "o" : "."));
        }
    }
}
