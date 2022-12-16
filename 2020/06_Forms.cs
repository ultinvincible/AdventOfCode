using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _06_Forms : AoCDay
    {
        protected override void Run()
        {
            string[][] groups = inputSections;
            foreach (var group in groups)
            {
                List<char> anyone = string.Join("",group)
                    .Distinct().ToList();
                part1 += anyone.Count;

                List<char> everyone = anyone.ConvertAll(_ => _);
                foreach (char yes in anyone)
                    foreach (string person in group)
                        if (!person.Contains(yes))
                        {
                            everyone.Remove(yes);
                            break;
                        }
                part2 += everyone.Count;
            }
        }
    }
}
