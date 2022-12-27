using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _16_VolcanoValves : AoCDay
    {
        protected override void Run()
        {
            string[] names = new string[inputLines.Length];
            int[] rates = new int[inputLines.Length];
            string[][] paths = new string[inputLines.Length][];
            List<int> flow = new();
            for (int l = 0; l < inputLines.Length; l++)
            {
                string line = inputLines[l];
                string[] split = line.Split(new string[]
                { "Valve "," has flow rate=","; tunnels lead to valves ","; tunnel leads to valve " },
                StringSplitOptions.RemoveEmptyEntries);
                names[l] = split[0];
                rates[l] = int.Parse(split[1]);
                if (rates[l] > 0) flow.Add(l);
                paths[l] = split[2].Split(", ");
            }
            int[][] tunnels = Array.ConvertAll(paths, v => Array.ConvertAll(v, path => Array.IndexOf(names, path)));
            for (int i = 0; i < names.Length; i++)
                Console.WriteLine($"{i,2}: {rates[i],2}|{string.Join(",", Array.ConvertAll(tunnels[i], i => $"{i,2}"))}");
            (int path, int dist)[][] map = new (int path, int dist)[tunnels.Length][];
            Move(new());

            void Move(List<int> path, int current = 0, int time = 30)
            {
                path.Add(current);
                //map[current] ??= Dijkstras<int[]>
                //        (tunnels => Array.ConvertAll(tunnels, t => (t, 1)), start: current).ToArray();
                int[] release = new int[rates.Length];
                foreach (int v in flow)
                    release[v] = rates[v] * (time - map[current][v].dist - 1);
                flow.Sort((a, b) => release[b] - release[a]);
                for (int v = 0; v < flow.Count / 2; v++)
                    Move(path, flow[v], time - map[current][v].dist - 1);
            }
        }
    }
}
