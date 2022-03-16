using System;

namespace Advent_of_Code._2021
{
    class _20_ImageEnhance : AoCDay
    {
        bool[] algorithm;
        bool[,] image;
        int LengthY => image.GetLength(0);
        int LengthX => image.GetLength(1);
        bool infiniteLight = false;
        void Enhance()
        {
            bool[,] newImage = new bool[LengthY + 2, LengthX + 2];
            for (int y = -1; y < LengthY + 1; y++)
                for (int x = -1; x < LengthX + 1; x++)
                {
                    string bin = "";
                    foreach (var (areaY, areaX) in Neighbors(y, x, true, true))
                        if (OutOfBounds(areaY, areaX, LengthY, LengthX))
                            bin += Convert.ToByte(infiniteLight);
                        else bin += Convert.ToByte(image[areaY, areaX]);
                    if (algorithm[Convert.ToInt32(bin, 2)])
                        newImage[y + 1, x + 1] = true;
                }
            image = newImage;
            if ((!infiniteLight && algorithm[0]) || (infiniteLight && !algorithm[^1]))
                infiniteLight = !infiniteLight;
        }
        int LitCount()
        {
            int lit = 0;
            foreach (bool pixel in image)
                if (pixel) lit++;
            return lit;
        }
        protected override void Run(out (object part1, object part2) answer)
        {
            algorithm = Array.ConvertAll(inputLines[0].ToCharArray(), c => c == '#');
            image = GridParse('#', 2);

            Enhance();
            Enhance();
            answer.part1 = LitCount();

            for (int i = 2; i < 50; i++)
            {
                Enhance();
                //Console.WriteLine(string.Join("\n", GridStr(image)));
            }
            answer.part2 = LitCount();
        }
    }
}
