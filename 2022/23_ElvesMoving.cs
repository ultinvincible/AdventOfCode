using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2022
{
    internal class _23_ElvesMoving : AoCDay
    {
        static readonly List<(int dimension, int value)> directions = new()
        {
            (0, -1), (0, 1), (1, -1), (1, 1),
        }; // N, S, W, E
        protected override void Run()
        {
            debug = 1;
            List<List<bool>> map = new(Array.ConvertAll(inputLines,
                s => new List<char>(s).ConvertAll(c => c == '#')));
            int elves = 0;
            for (int row = 0; row < map.Count; row++)
                for (int col = 0; col < map[0].Count; col++)
                    if (map[row][col]) elves++;

            for (int r = 0; ; r++)
            {
                map.Insert(0, new(new bool[map[0].Count]));
                map.Add(new(new bool[map[0].Count]));
                for (int row = 0; row < map.Count; row++)
                {
                    map[row].Insert(0, false);
                    map[row].Add(false);
                }
                (int, int)[,] propose = new (int, int)[map.Count, map[0].Count];
                for (int row = 0; row < propose.GetLength(0); row++)
                    for (int col = 0; col < propose.GetLength(1); col++)
                        propose[row, col] = (-1, -1);

                for (int row = 1; row < map.Count - 1; row++)
                    for (int col = 1; col < map[0].Count - 1; col++)
                    {
                        if (!map[row][col]) continue;
                        int neiCount = 0;
                        foreach ((int neiRow, int neiCol) in Neighbors(row, col, diagonal: true))
                            if (map[neiRow][neiCol]) neiCount++;
                        if (neiCount == 0) continue;

                        int[] move = new int[] { row, col };
                        foreach ((int dimension, int value) in directions)
                        {
                            int i = -1;
                            for (; i <= 1; i++)
                            {
                                int[] check = Array.ConvertAll(move, _ => _);
                                check[dimension] += value;
                                check[1 - dimension] += i;
                                if (map[check[0]][check[1]])
                                    break;
                            }
                            if (i == 2)
                            {
                                move[dimension] += value;
                                if (propose[move[0], move[1]] == (-2, -2)) // conflicted move
                                    move[dimension] -= value;
                                else break;
                            }
                        }

                        //Console.WriteLine((row, col) + " => " + (move[0], move[1]));
                        if (propose[move[0], move[1]] == (-1, -1)) // default, empty
                        {
                            propose[move[0], move[1]] = (row, col);
                        }
                        else propose[move[0], move[1]] = (-2, -2); // conflicted move
                    }

                bool moved = false;
                for (int row = 0; row < map.Count; row++)
                    for (int col = 0; col < map[0].Count; col++)
                        if (propose[row, col] != (-1, -1) && propose[row, col] != (-2, -2)
                            && propose[row, col] != (row, col))
                        {
                            (int elfRow, int elfCol) = propose[row, col];
                            map[elfRow][elfCol] = false;
                            map[row][col] = true;
                            moved = true;
                        }

                if (!map[0].Contains(true)) map.RemoveAt(0);
                if (!map[^1].Contains(true)) map.RemoveAt(map.Count - 1);
                if (map.All(row => !row[0])) map.ForEach(row => row.RemoveAt(0));
                if (map.All(row => !row[^1])) map.ForEach(row => row.RemoveAt(row.Count - 1));

                if (debug == 1)
                {
                    Console.WriteLine(GridPrint(map, b => b ? "#" : ".", true));
                }

                if (r == 9 || !moved && part1 == 0)
                    part1 = map.Count * map[0].Count - elves;

                if (!moved)
                {
                    part2 = r + 1;
                    break;
                }

                directions.Add(directions[0]);
                directions.RemoveAt(0);
            }
        }
    }
}
