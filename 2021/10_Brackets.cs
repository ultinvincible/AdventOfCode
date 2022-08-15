using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _10_Brackets : AoCDay
    {
        protected override void Run()
        {
            string open = "([{<", close = ")]}>";
            List<string> uncorrupted = new();

            int result = 0;
            int[] syntaxScores = new int[] { 3, 57, 1197, 25137 };
            foreach (string chunk in inputLines)
            {
                bool corrupt = false;
                Stack<char> openStack = new();
                foreach (char c in chunk)
                {
                    if (open.Contains(c)) openStack.Push(c);
                    else if (openStack.Peek() == open[close.IndexOf(c)])
                        openStack.Pop();
                    else
                    {
                        result += syntaxScores[close.IndexOf(c)];
                        corrupt = true;
                        break;
                    }
                }
                if (!corrupt)
                    uncorrupted.Add(chunk);
            }
            part1 = result;

            List<long> autoScores = new();
            foreach (string chunk in uncorrupted)
            {
                Stack<char> openStack = new();
                foreach (char c in chunk)
                {
                    if (open.Contains(c)) openStack.Push(c);
                    else if (openStack.Peek() == open[close.IndexOf(c)])
                        openStack.Pop();
                }
                long score = 0;
                foreach (char c in openStack)
                {
                    score *= 5;
                    score += open.IndexOf(c) + 1;
                }
                autoScores.Add(score);
            }
            autoScores.Sort();
            part2 = autoScores[autoScores.Count / 2];
        }
    }
}
