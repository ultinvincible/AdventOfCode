using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2020
{
    class _17_PocketDimension : AoCDay
    {
        List<(int, int, int)> Neighbors3d(int x, int y, int z)
        {
            List<(int x, int y, int z)> neighbors = new();
            for (int neiX = x - 1; neiX <= x + 1; neiX++)
                for (int neiY = y - 1; neiY <= y + 1; neiY++)
                    for (int neiZ = z - 1; neiZ <= z + 1; neiZ++)
                        if ((neiX, neiY, neiZ) != (x, y, z))
                            neighbors.Add((neiX, neiY, neiZ));
            return neighbors;
        }
        int RunCycles3D(int[,] start, int cycles)
        {
            int[,,] current = new int[start.GetLength(0) + 2 + cycles * 2,
                start.GetLength(1) + 2 + cycles * 2, 3 + cycles * 2];
            for (int col = 0; col < start.GetLength(1); col++)
                for (int row = 0; row < start.GetLength(0); row++)
                    current[col + 1 + cycles, row + 1 + cycles, 1 + cycles] = start[row, col];
            if (debug) Console.WriteLine(Print3D(current, -1, cycles));

            for (int c = 0; c < cycles; c++)
            {
                int[,,] next = new int[current.GetLength(0), current.GetLength(1), current.GetLength(2)];
                for (int x = 1; x < current.GetLength(0) - 1; x++)
                    for (int y = 1; y < current.GetLength(1) - 1; y++)
                        for (int z = 1; z < current.GetLength(2) - 1; z++)
                            if (current[x, y, z] == 1)
                                foreach ((int neiX, int neiY, int neiZ) in Neighbors3d(x, y, z))
                                    next[neiX, neiY, neiZ]++;
                if (debug) Console.WriteLine(Print3D(next, c, cycles, true));
                for (int x = 0; x < next.GetLength(0); x++)
                    for (int y = 0; y < next.GetLength(1); y++)
                        for (int z = 0; z < next.GetLength(2); z++)
                            if ((current[x, y, z] == 1 &&
                                (next[x, y, z] == 2 || next[x, y, z] == 3)) ||
                                 (current[x, y, z] == 0 && next[x, y, z] == 3))
                                next[x, y, z] = 1;
                            else next[x, y, z] = 0;
                current = next;
                if (debug) Console.WriteLine(Print3D(current, c, cycles));
            }
            int activeCount = 0;
            foreach (int cube in current)
                activeCount += cube;
            return activeCount;
        }
        List<(int, int, int, int)> Neighbors4d(int x, int y, int z, int w)
        {
            List<(int, int, int, int)> neighbors = new();
            List<(int, int, int)> nei3D = Neighbors3d(x, y, z);
            nei3D.Add((x, y, z));
            foreach ((int neiX, int neiY, int neiZ) in nei3D)
                for (int neiW = w - 1; neiW <= w + 1; neiW++)
                    if ((neiX, neiY, neiZ, neiW) != (x, y, z, w))
                        neighbors.Add((neiX, neiY, neiZ, neiW));
            return neighbors;
        }
        int RunCycles4D(int[,] start, int cycles)
        {
            int[,,,] current = new int[start.GetLength(0) + 2 + cycles * 2,
                start.GetLength(1) + 2 + cycles * 2, 3 + cycles * 2, 3 + cycles * 2];
            for (int col = 0; col < start.GetLength(1); col++)
                for (int row = 0; row < start.GetLength(0); row++)
                    current[col + 1 + cycles, row + 1 + cycles, 1 + cycles, 1 + cycles] = start[row, col];
            //if (debug) Console.WriteLine(Print3D(current, -1, cycles));

            for (int c = 0; c < cycles; c++)
            {
                int[,,,] next = new int[current.GetLength(0), current.GetLength(1), current.GetLength(2), current.GetLength(3)];
                for (int x = 1; x < current.GetLength(0) - 1; x++)
                    for (int y = 1; y < current.GetLength(1) - 1; y++)
                        for (int z = 1; z < current.GetLength(2) - 1; z++)
                            for (int w = 1; w < current.GetLength(3) - 1; w++)
                                if (current[x, y, z, w] == 1)
                                    foreach ((int neiX, int neiY, int neiZ, int neiW) in Neighbors4d(x, y, z, w))
                                        next[neiX, neiY, neiZ, neiW]++;
                //if (debug) Console.WriteLine(Print3D(next, c, cycles, true));
                for (int x = 0; x < next.GetLength(0); x++)
                    for (int y = 0; y < next.GetLength(1); y++)
                        for (int z = 0; z < next.GetLength(2); z++)
                            for (int w = 0; w < next.GetLength(3); w++)
                                if ((current[x, y, z, w] == 1 &&
                                    (next[x, y, z, w] == 2 || next[x, y, z, w] == 3)) ||
                                     (current[x, y, z, w] == 0 && next[x, y, z, w] == 3))
                                    next[x, y, z, w] = 1;
                                else next[x, y, z, w] = 0;
                current = next;
                //if (debug) Console.WriteLine(Print3D(current, c, cycles));
            }
            int activeCount = 0;
            foreach (int cube in current)
                activeCount += cube;
            return activeCount;
        }
        string Print3D(int[,,] pocket, int cycle = 0, int maxCycle = 0, bool intMode = false)
        {
            string print = "";
            for (int z = maxCycle - cycle;
                z < pocket.GetLength(2) - maxCycle + cycle; z++)
            {
                print += "z = " + z + "\n";
                for (int y = maxCycle - cycle;
                    y < pocket.GetLength(1) - maxCycle + cycle; y++)
                {
                    for (int x = maxCycle - cycle;
                        x < pocket.GetLength(0) - maxCycle + cycle; x++)
                        if (intMode) print += pocket[x, y, z];
                        else if (pocket[x, y, z] == 1) print += "#";
                        else if (pocket[x, y, z] == 0) print += ".";
                    print += "\n";
                }
                print += "\n";
            }
            return print;
        }

        protected override void Run()
        {
            int[,] start = GridParse(c => Convert.ToInt32(c == '#'));
            part1 = RunCycles3D(start, 6);
            part2 = RunCycles4D(start, 6);
        }
    }
}
