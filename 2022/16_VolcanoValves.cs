using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _16_VolcanoValves : AoCDay
    {
        int[] rates;
        List<int> importantValves = new();
        Dictionary<int, int[]> costs = new();

        protected override void Run()
        {
            string[] names = new string[inputLines.Length];
            rates = new int[inputLines.Length];
            string[][] tunnelsNames = new string[inputLines.Length][];
            for (int l = 0; l < inputLines.Length; l++)
            {
                string line = inputLines[l];
                string[] split = line.Split(new string[]
                { "Valve "," has flow rate=","; tunnels lead to valves ","; tunnel leads to valve " },
                StringSplitOptions.RemoveEmptyEntries);
                names[l] = split[0];
                rates[l] = int.Parse(split[1]);
                if (rates[l] > 0) importantValves.Add(l);
                tunnelsNames[l] = split[2].Split(", ");
            }
            importantValves.Insert(0, Array.IndexOf(names, "AA"));
            int[][] tunnels = Array.ConvertAll(tunnelsNames,
                valve => Array.ConvertAll(valve, tunnel => Array.IndexOf(names, tunnel)));
            if (debug == 1)
                for (int i = 0; i < names.Length; i++)
                    Console.WriteLine($"{i,2}: {rates[i],2}|" +
                        $"{string.Join(",", Array.ConvertAll(tunnels[i], i => $"{i,2}"))}");
            foreach (int iV in importantValves)
            {
                Dictionary<int, (int prev, int cost)> tree =
                    Dijkstras(v => Array.ConvertAll(tunnels[v], t => (t, 1)), iV);
                costs[iV] = new int[tree.Count];
                foreach ((int index, (int prev, int cost) node) in tree)
                    costs[iV][index] = node.cost;
            }

            Move(importantValves[0]);
            part1 = maxRelease;

            debug = 0;
            maxRelease = 0;
            MoveP2(importantValves[0]);
            part2 = maxRelease;
        }

        int maxRelease = 0;
        void Move(int current, List<int> path = null, int time = 30, int release = 0)
        {
            path ??= new();
            path.Add(current);
            if (path.Count == importantValves.Count)
            {
                maxRelease = Math.Max(maxRelease, release);
                Print(path);
                return;
            }

            bool move = false;
            foreach (int valve in importantValves)
            {
                if (path.Contains(valve)) continue;
                int timeLeft = time - costs[current][valve] - 1,
                    newRelease = rates[valve] * timeLeft;
                if (timeLeft >= 0)
                {
                    Move(valve, new(path), timeLeft, release + newRelease);
                    move = true;
                }
            }
            if (!move)
            {
                maxRelease = Math.Max(maxRelease, release);
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
                maxRelease = Math.Max(maxRelease, release);
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
                int timeLeft = time[mover] - costs[current][valve] - 1,
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
                maxRelease = Math.Max(maxRelease, release);
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
