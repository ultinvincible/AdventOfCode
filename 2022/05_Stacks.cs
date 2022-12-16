using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _05 : AoCDay
    {
        protected override void Run()
        {
            //debug = true;
            List<char>[] stacks = new List<char>[9];
            for (int col = 0; col < 9; col++)
            {
                stacks[col] = new();
                for (int row = inputSections[0].Length - 2; row >= 0; row--)
                {
                    char crate = inputSections[0][row][1 + col * 4];
                    if (crate != ' ') stacks[col].Add(crate);
                }
            }
            List<char>[] stacks2 = Array.ConvertAll(stacks, s => new List<char>(s));
            if (debug)
            {
                Console.WriteLine(GridStr(stacks, numbered: true));
                //Console.WriteLine(GridStr(stacks2, numbered: true));
            }

            int[][] moves = Array.ConvertAll(inputSections[1],
                line => Array.ConvertAll(line.Split
                (new string[] { "move ", " from ", " to " },
                StringSplitOptions.RemoveEmptyEntries), int.Parse));
            foreach (int[] move in moves)
            {
                int quantity = move[0], from = move[1] - 1, to = move[2] - 1;
                if (debug) Console.WriteLine
                    ($"move {quantity} from {from} to {to}");

                List<char> range = stacks[from].
                    GetRange(stacks[from].Count - quantity, quantity);
                range.Reverse();
                stacks[to].AddRange(range);
                stacks[from].RemoveRange(stacks[from].Count - quantity, quantity);

                stacks2[to].AddRange(stacks2[from].
                    GetRange(stacks2[from].Count - quantity, quantity));
                stacks2[from].RemoveRange(stacks2[from].Count - quantity, quantity);

                if (debug)
                {
                    Console.WriteLine(GridStr(stacks, numbered: true));
                    Console.WriteLine(GridStr(stacks2, numbered: true));
                }
            }

            for (int i = 0; i < 9; i++)
            {
                part1_str += stacks[i][^1];
                part2_str += stacks2[i][^1];
            }
        }
    }
}
