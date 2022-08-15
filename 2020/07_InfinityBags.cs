using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _07_InfinityBags : AoCDay
    {
        List<string> contained = new();
        int ContainerCount(string color)
        {
            int result = 0;
            foreach (string rule in inputLines)
            {
                string[] split = rule.Split(" bags contain ");
                if (!contained.Contains(split[0]) && split[0] != color)
                    foreach (string clr in split[1].Split(new[]
                        {" bags", " bag", ", ", "."},
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (clr[2..] == color)
                        {
                            contained.Add(split[0]);
                            result += 1 + ContainerCount(split[0]);
                        }
                    }
            }
            return result;
        }
        int InsideCount(string color)
        {
            int result = 0;
            foreach (string rule in inputLines)
            {
                string[] split = rule.Split(" bags contain ");
                if (split[0] == color && split[1] != "no other bags.")
                {
                    foreach (string clr in split[1].Split(new[]
                        {" bags", " bag", ", ", "."},
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        result += (int)char.GetNumericValue(clr[0]) * (1 + InsideCount(clr[2..]));
                    }
                    break;
                }
            }
            return result;
        }
        public override void Run()
        {
            (part1,part2) = (ContainerCount("shiny gold"), InsideCount("shiny gold"));
        }
    }
}
