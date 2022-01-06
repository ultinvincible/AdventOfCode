using System;

namespace Advent_of_Code
{
    class ImageEnhance : AoCDay
    {
        public ImageEnhance() : base(20) { }
        bool[] enhance;
        bool[,] image;
        int lY => image.GetLength(0);
        int lX => image.GetLength(1);
        public override void Run()
        {
            enhance = Array.ConvertAll(input[0].ToCharArray(), c =>
                { if (c == '#') return true; return false; });
            image = GridParse(input[2..]);
            bool infiniteLight = false;

            for (int i = 0; i < 50; i++)
            {
                bool[,] newImage = new bool[lY + 2, lX + 2];
                for (int y = -1; y < lY + 1; y++)
                    for (int x = -1; x < lX + 1; x++)
                    {
                        string bin = "";
                        foreach (var (areaY, areaX) in Neighbors(y, x, true, true))
                            if (OutOfBounds(areaY, areaX, lY, lX))
                                bin += Convert.ToByte(infiniteLight);
                            else bin += Convert.ToByte(image[areaY, areaX]);
                        if (enhance[Convert.ToInt32(bin, 2)])
                            newImage[y + 1, x + 1] = true;
                    }
                image = newImage;
                if ((!infiniteLight && enhance[0]) || (infiniteLight && !enhance[^1]))
                    infiniteLight = !infiniteLight;

                //Console.WriteLine(string.Join("\n", GridStr(image)));
                if (i == 1 || i == 49)
                {
                    int lit = 0;
                    foreach (bool pixel in image)
                        if (pixel) lit++;
                    Console.WriteLine(lit);
                }
            }
        }
    }
}
