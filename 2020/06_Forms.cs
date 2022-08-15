using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _06_Forms : AoCDay
    {
        protected override void Run()
        {
            string[] groups = input.Split(Environment.NewLine + Environment.NewLine);
            int anyCount = 0, everyCount = 0;
            foreach (var group in groups)
            {
                List<char> anyone = group.Replace(Environment.NewLine, "")
                    .Distinct().ToList();
                anyCount += anyone.Count;

                List<char> everyone = anyone.ConvertAll(_ => _);
                foreach (char yes in anyone)
                    foreach (string person in group.Split(Environment.NewLine,
                        StringSplitOptions.RemoveEmptyEntries))
                        if (!person.Contains(yes))
                        {
                            everyone.Remove(yes);
                            break;
                        }
                everyCount += everyone.Count;
            }
            (part1,part2) = (anyCount, everyCount);
        }
    }
}
