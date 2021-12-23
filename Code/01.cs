using System;

namespace Advent_of_Code
{
    class Depths : AoCDay
    {
        public Depths() : base(1) { }
        int[] depths;
        int CountIncreases(int distance)
        {
            int count = 0;
            for (int i = 0; i < depths.Length - distance; i++)
                if (depths[i] < depths[i + distance])
                    count++;
            return count;
        }
        public override void Run()
        {
            depths = Array.ConvertAll(input, line => int.Parse(line));
            Console.WriteLine(CountIncreases(1));
            Console.WriteLine(CountIncreases(3));
        }
    }
}