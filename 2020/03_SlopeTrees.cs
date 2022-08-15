using System;

namespace Advent_of_Code._2020
{
    class _03_SlopeTrees : AoCDay
    {
        int columnsCount;
        long CountTrees(int right, int down)
        {
            long trees = 0; int x = 0;
            for (int i = 0; i < inputLines.Length; i += down)
            {
                if (inputLines[i][x % columnsCount] == '#') trees++;
                x += right;
            }
            //Console.WriteLine("Trees(" + right + "," + down + "): " + trees);
            return trees;
        }
        protected override void Run()
        {
            columnsCount = inputLines[0].Length;
            long result = CountTrees(3, 1);
            (part1,part2) = (result, CountTrees(1, 1) * result
                * CountTrees(5, 1) * CountTrees(7, 1) * CountTrees(1, 2));
        }
    }
}
