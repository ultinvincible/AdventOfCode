using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _15_MemoryGame : AoCDay
    {
        int[] start;
        protected override void Run()
        {
            start = Array.ConvertAll(input.Split(','), int.Parse);
            part1 = PlayGame(2020);
            part2 = PlayGame(30000000);
        }

        int PlayGame(int getTurn)
        {
            Dictionary<int, int> memory = new();
            for (int i = 0; i < start.Length - 1; i++)
                memory[start[i]] = i;

            int number = start[^1];
            for (int i = start.Length - 1; i < getTurn - 1; i++)
            {
                int previous = number;
                if (memory.TryGetValue(number, out int turn))
                    number = i - turn;
                else number = 0;
                memory[previous] = i;
            }
            return number;
        }
    }
}
