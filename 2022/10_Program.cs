using System;

namespace Advent_of_Code._2022
{
    class _10_Program : AoCDay
    {
        protected override void Run()
        {
            int X = 1, cycleCount = 1, position = 0;
            char[,] CRT = new char[6, 40];
            foreach (string line in inputLines)
            {
                Cycle(X, ref cycleCount, ref position, CRT);
                if (cycleCount % 40 == 20)
                    part1 += cycleCount * X;
                if (line[..4] == "addx")
                {
                    Cycle(X, ref cycleCount, ref position, CRT);
                    X += int.Parse(line[5..]);
                    if (cycleCount % 40 == 20)
                        part1 += cycleCount * X;
                }
            }
            part2_str = "\n" + GridStr(CRT);
        }

        static void Cycle(int X, ref int cycle, ref int position, char[,] CRT)
        {
            int row = Math.DivRem(position, 40, out int col);
            if (Math.Abs(X - col) <= 1) CRT[row, col] = blockCharacter;
            else CRT[row, col] = ' ';
            position++; cycle++;
        }
    }
}
