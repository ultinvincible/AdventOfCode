using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    static class Chunks
    {
        public static void Run()
        {
            string[] input = System.IO.File.ReadAllLines("10.txt");
            string open = "([{<", close = ")]}>";
            List<string> uncorrupted = new();

            int result = 0;
            int[] syntaxScores = new int[] { 3, 57, 1197, 25137 };
            foreach (string chunk in input)
            {
                string openStack = "";
                bool corrupt = false;
                foreach (char c in chunk)
                {
                    if (open.Contains(c)) openStack += c;
                    else if (openStack[^1] == open[close.IndexOf(c)])
                        openStack = openStack[..^1];
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
            Console.WriteLine(result);

            List<long> autoScores = new();
            foreach (string chunk in uncorrupted)
            {
                string openStack = "";
                foreach (char c in chunk)
                {
                    if (open.Contains(c)) openStack += c;
                    else if (openStack[^1] == open[close.IndexOf(c)])
                        openStack = openStack[..^1];
                }
                long score = 0;
                foreach (char c in openStack.Reverse())
                {
                    score *= 5;
                    score += open.IndexOf(c) + 1;
                }
                autoScores.Add(score);
            }
            autoScores.Sort();
            Console.WriteLine(autoScores[autoScores.Count / 2]);
        }
    }
}
