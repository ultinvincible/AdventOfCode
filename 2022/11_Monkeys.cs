using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _11_Monkeys : AoCDay
    {
        protected override void Run()
        {
            List<long>[] items = new List<long>[inputSections.Length];
            Func<long, long>[] Operation = new Func<long, long>[inputSections.Length];
            int[] testDivisor = new int[inputSections.Length],
                monkeyTrue = new int[inputSections.Length], monkeyFalse = new int[inputSections.Length];

            for (int mk = 0; mk < inputSections.Length; mk++)
            {
                string[] section = Array.ConvertAll(inputSections[mk][1..], s => s.Split(':')[1]);

                items[mk] = new(Array.ConvertAll(section[0].Split(','), long.Parse));
                string op = section[1][" new = old ".Length..];
                if (op == "* old")
                    Operation[mk] = (long old) => old * old;
                else if (op[0] == '+')
                    Operation[mk] = (long old) => old + long.Parse(op[2..]);
                else if (op[0] == '*')
                    Operation[mk] = (long old) => old * long.Parse(op[2..]);
                else throw new Exception("Unexpected Operation");
                testDivisor[mk] = int.Parse(section[2][" divisible by ".Length..]);
                monkeyTrue[mk] = int.Parse(section[3][" throw to monkey ".Length..]);
                monkeyFalse[mk] = int.Parse(section[4][" throw to monkey ".Length..]);
            }
            List<long>[] items2 = Array.ConvertAll(items, l => new List<long>(l));

            long[] active = new long[items.Length];
            for (int round = 0; round < 20; round++)
                for (int mk = 0; mk < items.Length; mk++)
                {
                    foreach (long oldI in items[mk])
                    {
                        long newI = Operation[mk](oldI);
                        newI /= 3; //rounded down
                        if (newI % testDivisor[mk] == 0)
                            items[monkeyTrue[mk]].Add(newI);
                        else items[monkeyFalse[mk]].Add(newI);
                    }
                    active[mk] += items[mk].Count;
                    items[mk] = new();
                }
            Array.Sort(active);
            part1 = active[^2] * active[^1];

            List<int[]>[] itemMods = new List<int[]>[inputSections.Length];
            for (int mk = 0; mk < inputSections.Length; mk++)
            {
                itemMods[mk] = new();
                for (int i = 0; i < items2[mk].Count; i++)
                {
                    itemMods[mk].Add(new int[testDivisor.Length]);
                    for (int d = 0; d < testDivisor.Length; d++)
                        itemMods[mk][i][d] = (int)(items2[mk][i] % testDivisor[d]);
                }
            }

            active = new long[itemMods.Length];
            for (int round = 0; round < 10000; round++)
                for (int mk = 0; mk < itemMods.Length; mk++)
                {
                    foreach (int[] oldI in itemMods[mk])
                    {
                        int[] newI = new int[testDivisor.Length];
                        for (int ii = 0; ii < newI.Length; ii++)
                            newI[ii] = (int)Operation[mk](oldI[ii]) % testDivisor[ii];
                        if (newI[mk] == 0)
                            itemMods[monkeyTrue[mk]].Add(newI);
                        else itemMods[monkeyFalse[mk]].Add(newI);
                    }
                    active[mk] += itemMods[mk].Count;
                    itemMods[mk] = new();
                }
            Array.Sort(active);
            part2 = active[^2] * active[^1];
        }
    }
}
