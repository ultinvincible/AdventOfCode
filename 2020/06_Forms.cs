using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _06_Forms : AoCDay
    {
        protected override void Run(out (object part1, object part2) answer)
        {
            string[] groups = input.Split("\n\n");
            int anyCount = 0, everyCount = 0;
            foreach (var group in groups)
            {
                List<char> anyone = group.Replace("\n", "")
                    .Distinct().ToList();
                anyCount += anyone.Count;

                List<char> everyone = anyone.ConvertAll(_ => _);
                foreach (char yes in anyone)
                    foreach (string person in group.Split('\n',
                        StringSplitOptions.RemoveEmptyEntries))
                        if (!person.Contains(yes))
                        {
                            everyone.Remove(yes);
                            break;
                        }
                everyCount += everyone.Count;
            }
            answer = (anyCount, everyCount);
        }
    }
}
