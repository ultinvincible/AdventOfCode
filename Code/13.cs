using System;
using System.Collections.Generic;

namespace Advent_of_Code
{
    class Origami:AoCDay
    {
        public Origami() : base(13) { }
        bool[,] dot;
        void Fold(char dir, int axis)
        {
            bool[,] newDots = new bool[0, 0];
            if (dir == 'y')
                newDots = new bool[axis + 1, dot.GetLength(1)];
            else if (dir == 'x')
                newDots = new bool[dot.GetLength(0), axis + 1];
            for (int x = 0; x < dot.GetLength(0); x++)
                for (int y = 0; y < dot.GetLength(1); y++)
                    if (dot[x, y])
                    {
                        if (x > axis && dir == 'y')
                            newDots[2 * axis - x, y] = true;
                        else if (y > axis && dir == 'x')
                            newDots[x, 2 * axis - y] = true;
                        else
                            newDots[x, y] = true;
                    }
            dot = newDots;
            //PrintDots();
        }
        void PrintDots()
        {
            for (int y = 0; y < dot.GetLength(0); y++)
            {
                for (int x = 0; x < dot.GetLength(1); x++)
                {
                    if (dot[y, x])
                        Console.Write('\u2588');
                    else Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override void Run()
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
            dot = new bool[++lengthY, ++lengthX];
            foreach (var (x, y) in d)
                dot[y, x] = true;
            //PrintDots();

            Fold('x', 655);
            int result = 0;
            for (int y = 0; y < dot.GetLength(0); y++)
                for (int x = 0; x < dot.GetLength(1); x++)
                    if (dot[y, x])
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
