using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _14_Polymers : AoCDay
    {
        string start;
        Dictionary<string, long> polymers = new();
        Dictionary<string, (string, string)> insert = new();

        void Step(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Dictionary<string, long> newPolymers = new(polymers);
                foreach (var (plm, count) in polymers)
                    if (count > 0)
                    {
                        var (plm1, plm2) = insert[plm];
                        newPolymers[plm] -= count;
                        newPolymers[plm1] += count;
                        newPolymers[plm2] += count;
                    }
                polymers = newPolymers;
            }
        }
        Dictionary<char, long> CharCount()
        {
            Dictionary<char, long> result = new();
            foreach (var (plm, count) in polymers)
            {
                foreach (char c in plm)
                {
                    if (!result.ContainsKey(c))
                        result.Add(c, count);
                    else result[c] += count;
                }
            }
            result[start[0]]++;
            result[start[^1]]++;
            foreach (char c in result.Keys)
                result[c] /= 2;
            return result;
        }
        public override void Run()
        {
            start = inputLines[0];
            foreach (string line in inputLines[2..])
            {
                polymers.Add(line[0..2], 0);
                insert.Add(line[0..2], (new string(new char[] { line[0], line[^1] }),
                    new string(new char[] { line[^1], line[1] })));
            }
            for (int i = 0; i < start.Length - 1; i++)
            {
                polymers[start[i..(i + 2)]]++;
            }

            Step(10);
            Dictionary<char, long> result = CharCount();
            //int length = result.Values.Sum();
            part1 = result.Values.Max() - result.Values.Min();

            Step(30);
            result = CharCount();
            part2 = result.Values.Max() - result.Values.Min();
        }

    }
}
