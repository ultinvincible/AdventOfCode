using System;
using System.Collections.Generic;

namespace Advent_of_Code
{
    class Chiton : AoCDay
    {
        readonly int lengthI, lengthJ;
        public Chiton() : base(15)
        {
            lengthI = input.Length;
            lengthJ = input[0].Length;
        }

        public override void Run()
        {
            int[,] cavern = GridParse();
            A_Star(cavern);

            cavern = new int[lengthI * 5, lengthJ * 5];
            for (int i = 0; i < lengthI; i++)
                for (int j = 0; j < lengthJ; j++)
                    cavern[i, j] = (int)char.GetNumericValue(input[i][j]);
            for (int i = 0; i < lengthI; i++)
                for (int j = 0; j < lengthJ; j++)
                {
                    for (int down = 0; down < 5; down++)
                    {
                        int repI = i + lengthI * down;
                        if (down != 0)
                        {
                            cavern[repI, j] = cavern[i, j] + down;
                            if (cavern[repI, j] > 9)
                                cavern[repI, j] -= 9;
                        }
                        for (int right = 1; right < 5; right++)
                        {
                            int repJ = j + lengthJ * right;
                            cavern[repI, repJ] = cavern[i, j] + down + right;
                            if (cavern[repI, repJ] > 9)
                                cavern[repI, repJ] -= 9;
                        }
                    }
                }
            A_Star(cavern);
        }
        void A_Star(int[,] cavern)
        {
            int lengthI = cavern.GetLength(0), lengthJ = cavern.GetLength(1);
            (int i, int j) dest = (lengthI - 1, lengthJ - 1);
            List<((int, int) key, int distance, int prev)> result =
                A_Star((0, 0), dest,
                ((int i, int j) coords) =>
                {
                    List<((int, int), int)> result = new();
                    foreach ((int i, int j) nei in Neighbors(coords.i, coords.j, lengthI, lengthJ))
                        result.Add((nei, cavern[nei.i, nei.j]));
                    return result;
                },
                (i, j) => i.Item1 == j.Item1 && i.Item2 == j.Item2,
                ((int i, int j) coords) => dest.i - coords.i + dest.j - coords.j); ;
            Console.WriteLine(result[^1].distance);
        }
    }
}

//uint[,] dists = new uint[lengthI, lengthJ];
//bool[,] visited = new bool[lengthI, lengthJ];
//HashSet<(int, int)> unvisited = new();
//for (int i = 0; i < lengthI; i++)
//    for (int j = 0; j < lengthJ; j++)
//    {
//        //visited[i, j] = false;
//        dists[i, j] = uint.MaxValue;
//    }
//dists[0, 0] = 0;
//var current = (0, 0);

//do
//{
//    (int i, int j) = current;
//    List<(int, int)> neighbors = new();
//    if (i > 0)
//        neighbors.Add((i - 1, j));
//    if (j > 0)
//        neighbors.Add((i, j - 1));
//    if (i < lengthI - 1)
//        neighbors.Add((i + 1, j));
//    if (j < lengthJ - 1)
//        neighbors.Add((i, j + 1));

//    foreach (var (neiI, neiJ) in neighbors)
//        if (!visited[neiI, neiJ])
//        {
//            unvisited.Add((neiI, neiJ));
//            if (dists[i, j] + weights[neiI, neiJ] < dists[neiI, neiJ])
//                dists[neiI, neiJ] = dists[i, j] + weights[neiI, neiJ];
//        }
//    visited[i, j] = true;
//    unvisited.Remove(current);

//    uint min = uint.MaxValue;
//    foreach (var (newI, newJ) in unvisited)
//        if (min > dists[newI, newJ])
//        {
//            current = (newI, newJ);
//            min = dists[newI, newJ];
//        }
//} while (!visited[lengthI - 1, lengthJ - 1]);
