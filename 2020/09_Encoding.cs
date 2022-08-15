using System;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _09_Encoding : AoCDay
    {
        public override void Run()
        {
            
            long[] numbers = Array.ConvertAll(inputLines, long.Parse);

            long result = 0;
            for (int check = 25; check < numbers.Length; check++)
            {
                long[] prev25 = numbers[(check - 25)..check];
                bool valid = false;
                foreach (long p in prev25)
                    if (prev25.Contains(numbers[check] - p))
                    {
                        valid = true;
                        break;
                    }
                if (!valid)
                {
                    result = numbers[check];
                    part1 = result;
                    break;
                }
            }

            for (int start = 0; start < numbers.Length; start++)
            {
                long sum = numbers[start];
                int length = 0;
                while (sum < result && start + ++length < numbers.Length)
                {
                    sum += numbers[start + length];
                    if (sum == result && length != 0)
                    {
                        long[] x = numbers[start..(start + length)];
                        part2 = x.Max() + x.Min();
                    }
                }
            }
        }
    }
}
