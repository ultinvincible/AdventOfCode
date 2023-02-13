
namespace Advent_of_Code._2021
{
    class _15_CaveChiton : AoCDay
    {
        static int Dijkstras(int[,] cavern)
        {
            (int row, int col) end = (cavern.GetLength(0) - 1, cavern.GetLength(1) - 1);
            (int prevRow, int prevCol, int cost)[,] result =
                Dijkstras(cavern, point => Neighbors(point.row, point.col, cavern)
                 .ConvertAll(nei => (nei, cavern[nei.row, nei.col])),
                destination: end);
            return result[end.row, end.col].cost;
        }

        protected override void Run()
        {
            int lengthRow = inputLines.Length, lengthCol = inputLines[0].Length;
            int[,] cavern1 = new int[lengthRow, lengthCol];
            for (int row = 0; row < lengthRow; row++)
                for (int col = 0; col < lengthCol; col++)
                    cavern1[row, col] = (int)char.GetNumericValue(inputLines[row][col]);
            part1 = Dijkstras(cavern1);

            int[,] cavern2 = new int[lengthRow * 5, lengthCol * 5];
            for (int row = 0; row < lengthRow; row++)
                for (int col = 0; col < lengthCol; col++)
                    for (int down = 0; down < 5; down++)
                        for (int right = 0; right < 5; right++)
                            cavern2[row + lengthRow * down, col + lengthCol * right] =
                                (cavern1[row, col] + down + right - 1) % 9 + 1;
            part2 = Dijkstras(cavern2);
        }
    }
}