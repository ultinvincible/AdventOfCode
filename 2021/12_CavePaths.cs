using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _12_CavePaths:AoCDay
    {
        Dictionary<string, List<string>> connections = new();
        List<List<string>> DFSearch(bool oneSmallTwice = false) // Depth First
        {
            List<List<string>> result = new();
            DFSearch(new(), "start", ref result, oneSmallTwice);
            return result;
        }
        void DFSearch(List<string> p, string cave,
            ref List<List<string>> result, bool oneSmallTwice)
        {
            List<string> path = new(p);
            if (cave[0] < 'a' || !path.Contains(cave))
                // check for capital char/ big cave
            {
                path.Add(cave);
            }
            else if (oneSmallTwice)
            {
                path.Add(cave);
                oneSmallTwice = false;
            }
            else return;
            if (cave == "end")
            {
                result.Add(path);
                return;
            }
            foreach (string conn in connections[cave])
            {
                DFSearch(path, conn, ref result, oneSmallTwice);
            }
        }
        public override void Run()
        {
            foreach (string line in inputLines)
            {
                string[] split = line.Split('-');

                if (split[0] != "end")
                {
                    if (!connections.ContainsKey(split[0]))
                        connections.Add(split[0], new());
                    if (split[1] != "start")
                        connections[split[0]].Add(split[1]);
                }

                if (split[1] != "end")
                {
                    if (!connections.ContainsKey(split[1]))
                        connections.Add(split[1], new());
                    if (split[0] != "start")
                        connections[split[1]].Add(split[0]);
                }
            }

            List<List<string>> paths;
            paths = DFSearch();
            part1 = paths.Count;

            paths = DFSearch(true);
            part2 = paths.Count;
        }
    }
}
