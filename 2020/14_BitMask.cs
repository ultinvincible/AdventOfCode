using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _14_BitMask : AoCDay
    {
        protected override void Run()
        {
            Dictionary<long, long> memory = RunProgram();
            foreach (long value in memory.Values)
                part1 += value;
            memory = RunProgram(2);
            foreach (long value in memory.Values)
                part2 += value;
        }

        Dictionary<long, long> RunProgram(int version = 1)
        {
            Dictionary<long, long> memory = new();
            string mask = "";
            foreach (string line in inputLines)
            {
                if (line[1] == 'a')
                    mask = line[7..];
                else if (line[1] == 'e')
                {
                    string[] split = line[4..].Split("] = ");
                    if (version == 1)
                    {
                        char[] value = Convert.ToString(int.Parse(split[1]), 2)
                            .PadLeft(36, '0').ToCharArray();
                        for (int i = 0; i < 36; i++)
                            if (mask[i] != 'X')
                                value[i] = mask[i];
                        memory[long.Parse(split[0])] = Convert.ToInt64(new string(value), 2);
                    }
                    else if (version == 2)
                    {
                        char[] address = Convert.ToString(int.Parse(split[0]), 2)
                            .PadLeft(36, '0').ToCharArray();
                        for (int i = 0; i < 36; i++)
                            if (mask[i] != '0')
                                address[i] = mask[i];

                        List<string> addresses = new();
                        GetAddresses(address, ref addresses);

                        long value = long.Parse(split[1]);
                        foreach (string add in addresses)
                            memory[Convert.ToInt64(add, 2)] = value;
                    }
                }
            }
            return memory;
        }
        void GetAddresses(char[] floating, ref List<string> addresses)
        {
            char[] address = Array.ConvertAll(floating, _ => _);
            for (int i = 0; i < 36; i++)
                if (address[i] == 'X')
                {
                    address[i] = '0';
                    GetAddresses(address, ref addresses);
                    address[i] = '1';
                    GetAddresses(address, ref addresses);
                    return;
                }
            addresses.Add(new string(Array.ConvertAll(address, _ => _)));
        }
    }
}
