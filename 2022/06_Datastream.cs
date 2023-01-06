using System;

namespace Advent_of_Code._2022
{
    class _06_repeatDistStream : AoCDay
    {
        static int[] repeatDist;
        protected override void Run()
        {
            debug = 0;
            repeatDist = new int[input.Length];
            part1 = Detect(4);
            part2 = Detect(14);
        }

        int Detect(int length)
        {
            for (int i = 0; i < repeatDist.Length; i++)
                repeatDist[i] = length;

            for (int i = 0; i < length - 1; i++)
                for (int prev = 1; prev <= i; prev++)
                    if (input[i] == input[i - prev])
                        repeatDist[i] = prev;

            for (int i = length - 1; i < repeatDist.Length; i++)
            {
                bool result = true;
                for (int prev = 1; prev <= length - 1; prev++)
                    if (input[i] == input[i - prev])
                    {
                        repeatDist[i] = prev;
                        result = false;
                        break;
                    }

                for (int prev = 1; prev <= length - 1; prev++)
                    if (repeatDist[i - prev] <= length - 1 - prev)
                    {
                        result = false;
                        break;
                    }

                if (debug == 1) Console.WriteLine
                    (input[i] + " | " + repeatDist[i]);

                if (result)
                    return i + 1;
            }
            throw new Exception("No result found");
        }
    }
}
