using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    static class Display7Segments
    {
        public static void Run()
        {
            string[] input = System.IO.File.ReadAllLines("8.txt");
            int length = input.Length;
            string[][] signals = new string[length][],
            outputs = new string[length][];
            for (int i = 0; i < length; i++)
            {
                string s = input[i];
                string[] split = s.Split(" | ");
                signals[i] = split[0].Split(' ');
                outputs[i] = split[1].Split(' ');
            }
            int result = 0;
            foreach (string[] line in outputs)
                foreach (string o in line)
                    if (new int[] { 2, 3, 4, 7 }.Contains(o.Length))
                        result++;
            Console.WriteLine(result);

            result = 0;
            int[][] display = new int[10][]{
                new int[] { 0, 1, 2, 4, 5, 6 },
                new int[] { 2, 5 },
                new int[] { 0, 2, 3, 4, 6 },
                new int[] { 0, 2, 3, 5, 6 },
                new int[] { 1, 2, 3, 5 },
                new int[] { 0, 1, 3, 5, 6 },
                new int[] { 0, 1, 3, 4, 5, 6 },
                new int[] { 0, 2, 5 },
                new int[] { 0, 1, 2, 3, 4, 5, 6 },
                new int[] { 0, 1, 2, 3, 5, 6 } };
            for (int line = 0; line < length; line++)
            {
                string[] positions = new string[7];
                for (int s = 0; s < 7; s++)
                    positions[s] = "abcdefg";
                foreach (string signal in signals[line])
                {
                    int[] eliminate = Array.Empty<int>();
                    switch (signal.Length)
                    {
                        case 2:
                            eliminate = display[1];
                            break;
                        case 3:
                            eliminate = display[7];
                            break;
                        case 4:
                            eliminate = display[4];
                            break;
                        case 5:
                            eliminate = new int[] { 0, 3, 6 };
                            break;
                        case 6:
                            eliminate = new int[] { 0, 1, 5, 6 };
                            break;
                    }
                    foreach (int p in eliminate)
                        positions[p] = string.Concat(Enumerable.Intersect(positions[p], signal));
                }
                while (!Array.TrueForAll(positions, p => p.Length == 1))
                    for (int i = 0; i < 7; i++)
                        if (positions[i].Length == 1)
                            for (int j = 0; j < 7; j++)
                                if (i != j)
                                    positions[j] = positions[j].Replace(positions[i], string.Empty);
                Dictionary<char, int> decoder = new();
                for (int i = 0; i < 7; i++)
                    decoder.Add(positions[i][0], i);

                string output = "";
                foreach (string digit in outputs[line])
                {
                    int[] segments = Array.ConvertAll(
                        digit.ToCharArray(), c => decoder[c]);
                    Array.Sort(segments);
                    for (int d = 0; d < display.Length; d++)
                        if (Enumerable.SequenceEqual(display[d], segments))
                        {
                            output += d;
                            break;
                        }
                }
                result += int.Parse(output);
            }
            Console.WriteLine(result);
        }
    }
}

