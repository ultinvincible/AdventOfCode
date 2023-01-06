using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _08_Trees : AoCDay
    {
        int[,] trees;
        List<(int, int)> visible = new();
        int[,,] distances;
        protected override void Run()
        {
            debug = 0;
            trees = GridParse();
            if (trees.GetLength(0) != trees.GetLength(1))
                throw new Exception("Input not square");
            int length = trees.GetLength(0);
            distances = new int[length, length, 4];
            for (int pivot = 1; pivot <= length - 2; pivot++)
            {
                if (debug == 1) Console.WriteLine("Pivot " + pivot);

                int direction = 0;
                foreach (bool isRow in new bool[] { true, false })
                    foreach (bool reverse in new bool[] { false, true })
                    {
                        int max = -1;
                        int[] nearest = new int[10];
                        for (int i = 0; i < 10; i++)
                            nearest[i] = -1;
                        IEnumerable<int> range = Enumerable.Range(0, length - 1);
                        if (reverse) range = Enumerable.Range(1, length - 1).Reverse();
                        foreach (int move in range)
                        {
                            (int row, int col) = (pivot, move);
                            if (!isRow) (row, col) = (move, pivot);

                            if (max < 9 && trees[row, col] > max)
                            {
                                visible.Add((row, col));
                                max = trees[row, col];
                                if (debug == 1) Console.WriteLine
                                        ($"  {names[direction],-6}{row,2},{col,2}");

                                if (!reverse) distances[row, col, direction] = move;
                                else distances[row, col, direction] = length - 1 - move;
                            }

                            if (distances[row, col, direction] == 0)
                            {
                                int distance = int.MaxValue;
                                for (int tree = trees[row, col]; tree < 10; tree++)
                                    if (nearest[tree] != -1)
                                        distance = Math.Min(distance,
                                            Math.Abs(move - nearest[tree]));
                                if (distance != int.MaxValue)
                                    distances[row, col, direction] = distance;
                            }
                            nearest[trees[row, col]] = move;
                        }
                        direction++;
                    }
            }
            part1 = visible.Distinct().Count() + 4; // corners

            for (int row = 1; row < length - 1; row++)
                for (int col = 1; col < length - 1; col++)
                {
                    int score = 1;
                    for (int dir = 0; dir < 4; dir++)
                        score *= distances[row, col, dir];
                    if (part2 < score)
                    {
                        part2 = score;
                        if (debug == 1) Console.WriteLine
                                ($"{row,2} {col,2} {trees[row, col]} {score}");
                    }
                }
        }

        static string[] names = new string[]
        {
            "Left", "Right", "Up", "Down"
        };
    }
}
