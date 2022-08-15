using System;

namespace Advent_of_Code._2020
{
    class _10_AdaptersChain : AoCDay
    {
        protected override void Run()
        {
            int length = inputLines.Length + 2;
            int[] adapters = new int[length];
            adapters[0] = 0;
            Array.ConvertAll(inputLines, int.Parse).CopyTo(adapters, 1);
            Array.Sort(adapters, 1, inputLines.Length);
            adapters[^1] = adapters[^2] + 3;

            int diff1 = 0, diff3 = 0;
            for (int i = 1; i < length; i++)
                if (adapters[i] - adapters[i - 1] == 1)
                    diff1++;
                else if (adapters[i] - adapters[i - 1] == 3)
                    diff3++;
            part1 = diff1 * diff3;

            long[] pathTo = new long[length];
            pathTo[0] = 1;
            for (int i = 1; i < length; i++)
                for (int j = 1; j <= 3 && j <= i; j++)
                {
                    int diff = adapters[i] - adapters[i - j];
                    if (diff >= 1 && diff <= 3)
                        pathTo[i] += pathTo[i - j];
                    else break;
                }
            part2 = pathTo[^1];
        }
    }
}
