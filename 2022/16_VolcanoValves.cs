using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _16_VolcanoValves : AoCDay
    {
        protected override void Run()
        {
            string[] names = new string[inputLines.Length];
            int[] rates = new int[inputLines.Length];
            string[][] tunnelsStr = new string[inputLines.Length][];
            List<int> importantValves = new();
            for (int l = 0; l < inputLines.Length; l++)
            {
                string line = inputLines[l];
                string[] split = line.Split(new string[]
                { "Valve "," has flow rate=","; tunnels lead to valves ","; tunnel leads to valve " },
                StringSplitOptions.RemoveEmptyEntries);
                names[l] = split[0];
                rates[l] = int.Parse(split[1]);
                if (rates[l] > 0) importantValves.Add(l);
                tunnelsStr[l] = split[2].Split(", ");
            }
            int start = Array.IndexOf(names, "AA");
            importantValves.Insert(0, start);
            int[][] tunnels = Array.ConvertAll(tunnelsStr,
                v => Array.ConvertAll(v, path => Array.IndexOf(names, path)));
            if (debug == 1)
                for (int i = 0; i < names.Length; i++)
                    Console.WriteLine($"{i,2}: {rates[i],2}|" +
                        $"{string.Join(",", Array.ConvertAll(tunnels[i], i => $"{i,2}"))}");
            Dictionary<int, (int path, int dist)[]> map = new();
            foreach (int iV in importantValves)
                map[iV] = Dijkstras
                        (v => Array.ConvertAll(tunnels[v], t => (t, 1)), start: iV).ToArray();
            List<int> result = new();
            Move(start);
            foreach (int release in result)
                part1 = Math.Max(part1, release);

            debug = 0;
            result = new();
            MoveP2(start);
            foreach (int release in result)
                part2 = Math.Max(part2, release);

            void Move(int current, List<int> path = null, int time = 30, int release = 0)
            {
                path ??= new();
                path.Add(current);
                if (path.Count == importantValves.Count)
                {
                    result.Add(release);
                    Print(path);
                    return;
                }

                bool move = false;
                foreach (int valve in importantValves)
                {
                    if (path.Contains(valve)) continue;
                    int timeLeft = time - map[current][valve].dist - 1,
                        newRelease = rates[valve] * timeLeft;
                    if (timeLeft >= 0)
                    {
                        Move(valve, new(path), timeLeft, release + newRelease);
                        move = true;
                    }
                }
                if (!move)
                {
                    result.Add(release);
                    Print(path);
                }
            }

            void Print(List<int> path)
            {
                if (debug == 1) Console.WriteLine(string.Join(" | ", path.ConvertAll(i => $"{i,2}")));
            }

            void MoveP2(int current, List<int>[] path = null, int[] time = null,
                int release = 0, int mover = 0)
            {
                path ??= new List<int>[] { new(), new() { current } };
                time ??= new int[] { 26, 26 };
                path[mover].Add(current);
                if (path[0].Count + path[1].Count == importantValves.Count + 1)
                {
                    result.Add(release);
                    PrintP2(path);
                    return;
                }

                if (time[mover] < time[1 - mover])
                {
                    mover = 1 - mover;
                    current = path[mover][^1];
                }
                bool move = false;
                foreach (int valve in importantValves)
                {
                    if (path[0].Contains(valve) || path[1].Contains(valve)) continue;
                    int timeLeft = time[mover] - map[current][valve].dist - 1,
                        newRelease = rates[valve] * timeLeft;
                    if (timeLeft >= 0)
                    {
                        int[] newTime = Array.ConvertAll(time, _ => _);
                        newTime[mover] = timeLeft;
                        MoveP2(valve, Array.ConvertAll(path, l => new List<int>(l)),
                            newTime, release + newRelease, mover);
                        move = true;
                    }
                }
                if (!move)
                {
                    result.Add(release);
                    PrintP2(path);
                }
            }
            void PrintP2(List<int>[] path)
            {
                if (debug == 1)
                {
                    foreach (List<int> mover in path)
                        Print(mover);
                    Console.WriteLine();
                }
            }
        }
    }
}
