using System;
using System.IO;

namespace Advent_of_Code
{
    static class Depths
    {
        static int CountIncreases(int[] input, int distance)
        {
            int count = 0;
            for (int i = 0; i < input.Length - distance; i++)
                if (input[i] < input[i + distance])
                    count++;
            return count;
        }
        public static void Run()
        {
            var input = Array.ConvertAll(File.ReadAllLines
                ("1.txt"), line => int.Parse(line));

            Console.WriteLine(CountIncreases(input, 1) + "\n" + CountIncreases(input, 3));
        }
    }
}