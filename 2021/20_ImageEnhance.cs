using System;

namespace Advent_of_Code._2021
{
    class _20_ImageEnhance : AoCDay
    {
        bool[] algorithm;
        bool[,] image;
        int length0 => image.GetLength(0);
        int length1 => image.GetLength(1);
        bool infiniteLight = false;
        void Enhance()
        {
            bool[,] newImage = new bool[length0 + 2, length1 + 2];
            for (int row = -1; row < length0 + 1; row++)
                for (int col = -1; col < length1 + 1; col++)
                {
                    string bin = "";
                    foreach (var (neiRow, neiCol) in Neighbors(row, col, null, true, true))
                        if (OutOfBounds(neiRow, neiCol, image))
                            bin += Convert.ToByte(infiniteLight);
                        else bin += Convert.ToByte(image[neiRow, neiCol]);
                    if (algorithm[Convert.ToInt32(bin, 2)])
                        newImage[row + 1, col + 1] = true;
                }
            image = newImage;
            if ((!infiniteLight && algorithm[0]) || (infiniteLight && !algorithm[^1]))
                infiniteLight = !infiniteLight;
            if (debug == 1) Console.WriteLine(string.Join("\n", GridStr(image, b => b ? "#" : ".")));
        }
        int LitCount()
        {
            int lit = 0;
            foreach (bool pixel in image)
                if (pixel) lit++;
            return lit;
        }
        protected override void Run()
        {
            debug = 0;
            algorithm = Array.ConvertAll(inputLines[0].ToCharArray(), c => c == '#');
            image = GridParse(c => c == '#', 2);
            if (debug == 1) Console.WriteLine(string.Join("\n", GridStr(image, b => b ? "#" : ".")));

            Enhance();
            Enhance();
            part1 = LitCount();

            for (int i = 2; i < 50; i++) Enhance();
            part2 = LitCount();
        }
    }
}
