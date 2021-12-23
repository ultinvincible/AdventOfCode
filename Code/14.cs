using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent_of_Code
{
    class Polymers : AoCDay
    {
        public Polymers() : base(14)
        {
            start = input[0];
        }
        string start;
        Dictionary<string, ulong> polymers = new();
        Dictionary<string, (string, string)> insert = new();
        void Step(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Dictionary<string, ulong> newPolymers = new(polymers);
                foreach (var (plm, count) in polymers)
                    if (count > 0)
                    {
                        var (i1, i2) = insert[plm];
                        newPolymers[plm] -= count;
                        newPolymers[i1] += count;
                        newPolymers[i2] += count;
                    }
                polymers = newPolymers;
            }
        }
        Dictionary<char, ulong> CharCount()
        {
            Dictionary<char, ulong> result = new();
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
            foreach (string line in input[2..])
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
            Dictionary<char, ulong> result = CharCount();
            //int length = result.Values.Sum();
            Console.WriteLine(result.Values.Max() - result.Values.Min());

            Step(30);
            result = CharCount();
            Console.WriteLine(result.Values.Max() - result.Values.Min());
        }

    }
}
