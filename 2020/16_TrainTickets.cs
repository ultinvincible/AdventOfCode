using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _16_TrainTickets : AoCDay
    {
        protected override void Run()
        {
            string[][] notes = inputSections;
            string[] fields = notes[0], tickets = notes[2][1..];
            //string[] names = new string[fields.Length]; //useless
            int[][] ranges = new int[fields.Length][];
            for (int i = 0; i < ranges.Length; i++)
            {
                string[] split = fields[i].Split(new string[] { ": ", "-", " or " },
                    StringSplitOptions.RemoveEmptyEntries);
                //names[i] = split[0]; //useless
                ranges[i] = Array.ConvertAll(split[1..], int.Parse);
            }
            int[,] values = new int[tickets.Length, ranges.Length];
            for (int row = 0; row < tickets.Length; row++)
            {
                string[] split = tickets[row].Split(',');
                for (int col = 0; col < ranges.Length; col++)
                    values[row, col] = int.Parse(split[col]);
            }

            bool[] invalid = new bool[values.Length];
            for (int row = 0; row < tickets.Length; row++)
                for (int col = 0; col < ranges.Length; col++)
                {
                    int value = values[row, col];
                    bool valid = false;
                    foreach (int[] range in ranges)
                    {
                        for (int r = 0; r < range.Length; r += 2)
                            if (range[r] <= value && value <= range[r + 1])
                            {
                                valid = true;
                                break;
                            }
                        if (valid) break;
                    }
                    if (!valid)
                    {
                        part1 += value;
                        invalid[row] = true;
                    }
                }

            int[] fieldsOrder = new int[ranges.Length];
            List<int>[] match = new List<int>[ranges.Length];
            for (int i = 0; i < ranges.Length; i++)
            {
                match[i] = new();
                for (int ii = 0; ii < ranges.Length; ii++)
                    match[i].Add(ii);
            }

            for (int col = 0; col < ranges.Length; col++)
            {
                for (int row = 0; row < tickets.Length; row++)
                {
                    if (invalid[row])
                        continue;
                    int value = values[row, col];
                    for (int r = 0; r < ranges.Length; r++)
                    {
                        int ido = match[col].IndexOf(r);
                        if (ido == -1)
                            continue;
                        int[] range = ranges[r];
                        bool matchRange = false;
                        for (int i = 0; i < range.Length; i += 2)
                            if (range[i] <= value && value <= range[i + 1])
                            {
                                matchRange = true;
                                break;
                            }
                        if (!matchRange) match[col].RemoveAt(ido);
                    }
                }
            }
            int[] fieldOrder = new int[ranges.Length];
            int found = -1, toRemove;
            do
            {
                toRemove = found;
                for (int i = 0; i < match.Length; i++)
                {
                    match[i].Remove(toRemove);
                    if (match[i].Count == 1)
                    {
                        fieldOrder[i] = match[i][0];
                        found = match[i][0];
                    }
                }
            } while (toRemove != found);

            part2 = 1;
            int[] yourTicket = Array.ConvertAll
                (notes[1][1].Split(','), int.Parse);
            for (int i = 0; i < yourTicket.Length; i++)
                if (fieldOrder[i] < 6)
                    part2 *= yourTicket[i];
        }
    }
}
