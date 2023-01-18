using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    internal class _19_Factorio : AoCDay
    {
        protected override void Run()
        {
            debug = 0;
            int[][] blueprints = new int[inputLines.Length][];
            for (int line = 0; line < inputLines.Length; line++)
            {
                string[] split = inputLines[line].Split(new string[]
                    {"costs "," ore.", "ore and ","clay."," obsidian."},
                    StringSplitOptions.None);
                blueprints[line] = Array.ConvertAll(
                    new string[] { split[1], "-1", split[3], "-1", split[5], split[6], split[8], split[9] },
                    int.Parse);
            }

            for (int i = 0; i < blueprints.Length; i++)
            {
                int maxGeodes = MaxGeodes(blueprints[i], 24);
                if (debug == 3) Console.Write($"{maxGeodes,2}|");
                part1 += (i + 1) * maxGeodes;
            }
            if (debug == 3) Console.WriteLine();

            part2 = 1;
            for (int i = 0; i < 3; i++)
                part2 *= MaxGeodes(blueprints[i], 32);
        }

        // blueprint:
        // r[0] costs b[0] q[0],
        // r[1] costs b[2] q[0],
        // r[2] costs b[4] q[0] and b[5] q[1],
        // r[3] costs b[6] q[0] and b[7] q[2],
        // r[m] costs b[m * 2] q[0]
        // if (m >= 2) and b[m * 2 + 1] q[m - 1]
        int MaxGeodes(int[] blueprint, int totalMinutes)
        {
            int[] maxRobots = new int[] { 0, blueprint[5], blueprint[7], int.MaxValue };
            for (int i = 0; i < 4; i++)
                maxRobots[0] = Math.Max(maxRobots[0], blueprint[i * 2]);
            int maxGeodes = 0;
            List<(List<(int minute, int build)> path, int geodes)> paths = new();
            List<(int minute, int build)> refPath = new();
            Recurse(new int[] { 1, 0, 0, 0 }, new int[4], 0, ref refPath);

            if (debug == 2)
            {
                Console.WriteLine(string.Join("|",
                    Array.ConvertAll(blueprint, b => b == -1 ? "" : b.ToString())));
                for (int i = 1; i <= totalMinutes; i++)
                    Console.Write(i.ToString()[^1] + " ");
                Console.WriteLine();
                foreach ((List<(int minute, int build)> path, int geodes) in paths)
                    if (geodes == maxGeodes)
                    {
                        char[] line = new char[totalMinutes];
                        Array.Fill(line, '.');
                        foreach ((int min, int build) in path)
                            line[min - 1] = build.ToString()[0];
                        Console.WriteLine(string.Join(' ', line));
                    }
            }

            return maxGeodes;

            void Recurse(int[] oldRobots, int[] oldQuantity, int minute, ref List<(int, int)> path)
            {
                if (debug == 1)
                {
                    int robotCount = 0;
                    foreach (int r in oldRobots)
                        robotCount += r;
                    Console.WriteLine(new string(' ', robotCount * 2 - 2)
                        + Print(oldRobots, oldQuantity, minute));
                }
                for (int mineral = 3; mineral >= 0; mineral--)
                {
                    if (oldRobots[mineral] == maxRobots[mineral] || mineral >= 2 && oldRobots[mineral - 1] == 0) continue;
                    int[] robots = Array.ConvertAll(oldRobots, _ => _), quantity = Array.ConvertAll(oldQuantity, _ => _);

                    int time = (int)Math.Ceiling((decimal)(blueprint[mineral * 2] - quantity[0]) / robots[0]);
                    if (mineral >= 2) time = Math.Max(time,
                           (int)Math.Ceiling((decimal)(blueprint[mineral * 2 + 1] - quantity[mineral - 1]) / robots[mineral - 1]));
                    if (time <= 0) time = 0;
                    time++;

                    if (minute + time < totalMinutes)
                    {
                        for (int m = 0; m < 4; m++)
                            quantity[m] += robots[m] * time;
                        quantity[0] -= blueprint[mineral * 2];
                        if (mineral >= 2) quantity[mineral - 1] -= blueprint[mineral * 2 + 1];
                        robots[mineral]++;

                        if (debug == 2) path.Add((minute + time, mineral));
                        Recurse(robots, quantity, minute + time, ref path);
                        if (mineral == 3)
                        {
                            int geodes = quantity[3] + robots[3] * (totalMinutes - minute - time);
                            maxGeodes = Math.Max(maxGeodes, geodes);
                            if (debug == 2)
                                paths.Add((new(path), geodes));
                        }
                        if (debug == 2) path.RemoveAt(path.Count - 1);
                    }
                }
            }
        }

        static string Print(int[] robots, int[] quantity, int minute)
            => string.Join(" ", robots) + "|"
             + string.Join(" ", Array.ConvertAll(quantity, i => $"{i,2}")) + "|" +
            $"{minute,2}";
    }
}
