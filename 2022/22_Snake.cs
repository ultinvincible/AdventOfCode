using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    internal class _22_Snake : AoCDay
    {
        static readonly (int dimension, int sign)[] directions = new (int dimension, int sign)[]
        {
            (1, 1), (0, 1), (1, -1), (0, -1)
        };
        static readonly char[] facing = new char[] { '>', 'v', '<', '^' };
        const int cubeLength = 50;
        static readonly int[][] folds = new int[][]
        {
            new int[] { 49 , 100, 0, 3 },
            new int[] { 50 , 99 , 1, 2 },
            new int[] { 50 , 50 , 1, 0 },
            new int[] { 100, 0  , 0, 1 },
            new int[] { 0  , 149, 1, 2 },
            new int[] { 149, 99 , 3, 2 },
            new int[] { 0  , 50 , 1, 0 },
            new int[] { 149, 0  , 3, 0 },
            new int[] { 149, 50 , 0, 3 },
            new int[] { 150, 49 , 1, 2 },
            new int[] { 0  , 50 , 0, 1 },
            new int[] { 150, 0  , 1, 0 },
            new int[] { 0  , 100, 0, 1 },
            new int[] { 199, 0  , 0, 3 },
        };
        protected override void Run()
        {
            char[][] map = Array.ConvertAll(inputSections[0], s => s.ToCharArray());
            int width = 0;
            for (int row = 0; row < map.Length; row++)
                width = Math.Max(width, map[row].Length);
            (int min, int max)[] rows = new (int min, int max)[map.Length],
                cols = new (int min, int max)[width];
            Array.Fill(cols, (-1, map.Length - 1));
            for (int row = 0; row < map.Length; row++)
            {
                rows[row] = (-1, map[row].Length - 1);
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (rows[row].min == -1)
                    {
                        if (map[row][col] != ' ')
                            rows[row].min = col;
                    }
                    else if (map[row][col] == ' ')
                    {
                        rows[row].max = col - 1;
                        break;
                    }

                    if (cols[col].min == -1)
                    {
                        if (map[row][col] != ' ')
                            cols[col].min = row;
                    }
                    else if (row != map.Length - 1 &&
                        (col >= map[row + 1].Length || map[row + 1][col] == ' '))
                    {
                        cols[col].max = row;
                    }
                }
            }
            (int min, int max)[][] bounds = new (int min, int max)[][] { cols, rows };

            string path = inputSections[1][0];
            List<string> split = new();
            int prev = 0;
            for (int i = 0; i < path.Length; i++)
                if (path[i] == 'L' || path[i] == 'R')
                {
                    split.Add(path[prev..i]);
                    split.Add(path[i].ToString());
                    prev = i + 1;
                }
                else if (i == path.Length - 1)
                    split.Add(path[prev..path.Length]);

            (int[] position, int dir) = Destination((position, dir) =>
            {
                (int dimension, int sign) = directions[dir];
                (int min, int max) = bounds[dimension][position[1 - dimension]];
                int dest = position[dimension] + sign;
                if (dest == max + 1) dest = min;
                else if (dest == min - 1) dest = max;
                int[] newPosition = Array.ConvertAll(position, _ => _);
                newPosition[dimension] = dest;
                return (newPosition, dir);
            });
            part1 = position[0] * 1000 + position[1] * 4 + 1004 + dir;

            debug = 0;
            map = Array.ConvertAll(inputSections[0], s => s.ToCharArray());
            (position, dir) = Destination((position, dir) =>
            {
                (int dimension, int sign) = directions[dir];
                (int min, int max) = bounds[dimension][position[1 - dimension]];
                int dest = position[dimension] + sign;
                int[] newPosition = Array.ConvertAll(position, _ => _);
                newPosition[dimension] = dest;
                int newDir = dir;

                if (dest == max + 1 || dest == min - 1)
                {
                    for (int f = 0; f < folds.Length; f++)
                    {
                        (int foldDim, int foldSign) = directions[folds[f][2]];
                        int distance = (position[foldDim] - folds[f][foldDim]) * foldSign;
                        if (position[1 - foldDim] == folds[f][1 - foldDim] && 0 <= distance && distance < cubeLength)
                        {
                            int[] fold = folds[f + (f % 2 == 0 ? 1 : -1)];
                            (int newDim, int newSign) = directions[fold[2]];
                            newPosition[1 - newDim] = fold[1 - newDim];
                            newPosition[newDim] = fold[newDim] + distance * newSign;
                            newDir = fold[3];
                            break;
                        }
                    }

                    if (debug == 2)
                    {
                        Console.WriteLine();
                        Console.WriteLine((position[0], position[1], facing[dir]));
                        Console.WriteLine((newPosition[0], newPosition[1], facing[newDir]));
                    }
                }

                return (newPosition, newDir);
            });
            if (debug == 2)
                Console.WriteLine();
            part2 = position[0] * 1000 + position[1] * 4 + 1004 + dir;

            (int[], int) Destination(Func<int[], int, (int[], int)> Move)
            {
                int[] position = new int[] { 0, bounds[1][0].min };
                int dir = 0;
                foreach (string move in split)
                {
                    if (debug == 2) Console.Write(move);
                    if (move == "L") dir = (dir + 3) % 4;
                    else if (move == "R") dir = (dir + 1) % 4;
                    else
                    {
                        int dist = int.Parse(move);
                        for (int i = 1; i <= dist; i++)
                        {
                            (int[] newPosition, int newDir) = Move(position, dir);
                            if (map[newPosition[0]][newPosition[1]] != '#')
                            {
                                (position, dir) = (newPosition, newDir);
                                map[newPosition[0]][newPosition[1]] = facing[dir];
                            }
                            else break;
                        }

                        if (debug == 1)
                        {
                            const int range = 15;
                            for (int row = Math.Max(0, position[0] - range);
                                row < Math.Min(map.Length, position[0] + range + 1); row++)
                            {
                                if (row % 50 == 0) Console.WriteLine(new string('-', 52));
                                for (int col = Math.Max(0, position[1] - range);
                                    col < Math.Min(map[row].Length, position[1] + range + 1); col++)
                                {
                                    if (col % 50 == 0) Console.Write('|');
                                    if ((row, col) == (position[0], position[1]))
                                        Console.Write(facing[dir]);
                                    else Console.Write(map[row][col]);
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
                return (position, dir);
            }
        }
    }
}
