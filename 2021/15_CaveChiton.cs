using System;

namespace Advent_of_Code._2021
{
    class _15_CaveChiton : AoCDay
    {
        static int Dijkstras(int[,] cavern)
        {
            int lengthY = cavern.GetLength(0),
                lengthX = cavern.GetLength(1);
            var result = Dijkstras(i =>
                Neighbors(Math.DivRem(i, lengthX, out int x), x, lengthY, lengthX)
                .ConvertAll(nei => (nei.y * lengthX + nei.x, cavern[nei.y, nei.x])),
                i => i == lengthY * lengthX - 1);
            return result[^1].weight;
        }

        protected override void Run()
        {
            int lengthY = inputLines.Length, lengthX = inputLines[0].Length;
            int[,] cavern1 = new int[lengthY, lengthX];
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                    cavern1[y, x] = (int)char.GetNumericValue(inputLines[y][x]);
            part1 = Dijkstras(cavern1);

            int[,] cavern2 = new int[lengthY * 5, lengthX * 5];
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                    for (int down = 0; down < 5; down++)
                        for (int right = 0; right < 5; right++)
                            cavern2[y + lengthY * down, x + lengthX * right] =
                                (cavern1[y, x] + down + right - 1) % 9 + 1;
            part2 = Dijkstras(cavern2);
        }
    }
}