using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    static class Origami
    {
        static string[] input = System.IO.File.ReadAllLines("13.txt");
        static bool[,] dots;
        static void Fold(char dir, int axis)
        {
            bool[,] newDots = new bool[0, 0];
            if (dir == 'y')
                newDots = new bool[axis + 1, dots.GetLength(1)];
            else if (dir == 'x')
                newDots = new bool[dots.GetLength(0), axis + 1];
            for (int x = 0; x < dots.GetLength(0); x++)
                for (int y = 0; y < dots.GetLength(1); y++)
                    if (dots[x, y])
                    {
                        if (x > axis && dir == 'y')
                            newDots[2 * axis - x, y] = true;
                        else if (y > axis && dir == 'x')
                            newDots[x, 2 * axis - y] = true;
                        else
                            newDots[x, y] = true;
                    }
            dots = newDots;
            //PrintDots();
        }
        static void PrintDots()
        {
            for (int y = 0; y < dots.GetLength(0); y++)
            {
                for (int x = 0; x < dots.GetLength(1); x++)
                {
                    if (dots[y, x])
                        Console.Write('\u2588');
                    else Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void Run()
        {
            int lengthX = 0, lengthY = 0;
            List<(int, int)> d = new();
            for (int i = 0; input[i] != ""; i++)
            {
                string[] split = input[i].Split(',');
                int x = int.Parse(split[0]), y = int.Parse(split[1]);
                lengthX = Math.Max(lengthX, x);
                lengthY = Math.Max(lengthY, y);
                d.Add((x, y));
            }
            dots = new bool[++lengthY, ++lengthX];
            foreach (var (x, y) in d)
                dots[y, x] = true;
            //PrintDots();

            Fold('x', 655);
            int result = 0;
            for (int y = 0; y < dots.GetLength(0); y++)
                for (int x = 0; x < dots.GetLength(1); x++)
                    if (dots[y, x])
                        result++;
            Console.WriteLine(result);

            foreach(string line in input[^11..])
            {
                string[] split = line.Split('=');
                Fold(split[0][^1], int.Parse(split[1]));
            }
            PrintDots();
        }
    }
}
