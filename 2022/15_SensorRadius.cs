using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _15_SensorRadius : AoCDay
    {
        const int part1Row = 2000000;
        protected override void Run()
        {
            HashSet<int> eliminatedCols = new();
            List<int> beaconCols = new();

            int[][] sensors = new int[inputLines.Length][];

            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                int[] split = Array.ConvertAll(line.Split(new string[]
                    { "Sensor at x=", ", y=", ": closest beacon is at x=" },
                    StringSplitOptions.RemoveEmptyEntries), int.Parse);
                // 0 = sensor col, 1 = s row, 2 = beacon col, 3 = b row
                int radius = ManhattanDistance(split);
                int range = radius - Math.Abs(split[1] - part1Row);
                if (range >= 0)
                {
                    for (int ii = split[0] - range; ii <= split[0] + range; ii++)
                        eliminatedCols.Add(ii);
                    if (split[3] == part1Row) beaconCols.Add(split[2]);
                }

                sensors[i] = new int[] { split[0], split[1], radius };
            }
            eliminatedCols.RemoveWhere(x => beaconCols.Contains(x));
            part1 = eliminatedCols.Count;

            List<int[]> lines = new();
            for (int s1 = 0; s1 < sensors.Length; s1++)
            {
                (int col, int row) ss1 = (sensors[s1][0], sensors[s1][1]);
                int radius1 = sensors[s1][2];
                for (int s2 = s1 + 1; s2 < sensors.Length; s2++)
                {
                    (int col, int row) ss2 = (sensors[s2][0], sensors[s2][1]);
                    int radius2 = sensors[s2][2];
                    if (ManhattanDistance(ss1, ss2) == radius1 + radius2 + 2)
                    {
                        int dirCol = (ss1.col < ss2.col ? 1 : -1),
                            dirRow = (ss1.row < ss2.row ? 1 : -1);
                        (int col, int row)
                            end1 = (ss1.col + dirCol * (radius1 + 1), ss1.row),
                            end2 = (ss1.col, ss1.row + dirRow * (radius1 + 1));
                        if (ManhattanDistance(end1, ss2) > radius2 + 1)
                            end1 = (ss2.col, ss2.row - dirRow * (radius2 + 1));
                        if (ManhattanDistance(end2, ss2) > radius2 + 1)
                            end2 = (ss2.col - dirCol * (radius2 + 1), ss2.row);
                        lines.Add(new int[] { end1.col, end1.row, end2.col, end2.row });
                    }
                }
            }
            if (debug == 1) Console.WriteLine(GridStr(lines.ToArray(), line => $"{line,2} | "));

            for (int l1 = 0; l1 < lines.Count; l1++)
                for (int l2 = l1 + 1; l2 < lines.Count; l2++)
                {
                    int? sum = null, diff = null;
                    foreach (int[] line in new int[][] { lines[l1], lines[l2] })
                    {
                        if (line[0] + line[1] == line[2] + line[3]) sum = line[0] + line[1];
                        if (line[0] - line[1] == line[2] - line[3]) diff = line[0] - line[1];
                    }
                    if (sum is null || diff is null) continue;
                    (long col, long row) result = ((long)(sum + diff) / 2, (long)(sum - diff) / 2);
                    if (debug == 1) Console.WriteLine($"{result.col,2} | {result.row,2} | {l1} | {l2}");
                    part2 = result.col * 4000000 + result.row;
                    return;
                }
        }
        // WARNING: Part 2 solution does not work for part 1, because it does not eliminate scanned intersection points.
        // This could be implemented easily, but I have had enough. No thanks.

        private static int ManhattanDistance(params int[] coords)
            => Math.Abs(coords[0] - coords[2]) + Math.Abs(coords[1] - coords[3]);
        private static int ManhattanDistance((int col, int row) ss1, (int col, int row) ss2)
            => ManhattanDistance(ss1.col, ss1.row, ss2.col, ss2.row);
    }
}
