using System;
using System.IO;
using System.Collections.Generic;

namespace Advent_of_Code
{
    abstract class AoCDay
    {
        public int day;
        public string inputString;
        public string[] input;
        public AoCDay(int d, bool lines = true)
        {
            day = d;
            if (lines)
                input = File.ReadAllLines(day.ToString("00") + ".txt");
            else
                inputString = File.ReadAllText(day.ToString("00") + ".txt");
        }
        public abstract void Run();

        // Helper functions
        protected static List<(int, int)> Neighbors((int, int) yx, int boundY, int boundX)
        {
            (int y, int x) = yx;
            return Neighbors(y, x, boundY, boundX);
        }
        protected static List<(int, int)> Neighbors(int y, int x, int boundY, int boundX)
        {
            List<(int, int)> neighbors = new();
            if (y > 0)
                neighbors.Add((y - 1, x));
            if (y < boundY - 1)
                neighbors.Add((y + 1, x));
            if (x > 0)
                neighbors.Add((y, x - 1));
            if (x < boundX - 1)
                neighbors.Add((y, x + 1));
            return neighbors;
        }
        protected static (uint[] distance, uint[] prev) Dijkstras(int count,
            Func<uint, uint, uint> PathWeight, Func<uint, List<uint>> Neighbors)
        {
            bool[] visited = new bool[count];
            uint[] distance = new uint[count];
            uint[] prev = new uint[count];
            for (int i = 0; i < count; i++)
            {
                //visited[i, j] = false;
                distance[i] = uint.MaxValue;
                prev[i] = uint.MaxValue;
            }
            HashSet<uint> unvisited = new();
            uint current = 0;
            distance[0] = 0;

            do
            {
                foreach (uint nei in Neighbors(current))
                    if (!visited[nei])
                    {
                        unvisited.Add(nei);
                        if (distance[current] + PathWeight(current, nei) < distance[nei])
                        {
                            distance[nei] = distance[current] + PathWeight(current, nei);
                            prev[nei] = current;
                        }
                    }
                visited[current] = true;
                unvisited.Remove(current);

                uint min = uint.MaxValue;
                foreach (var unv in unvisited)
                    if (min > distance[unv])
                    {
                        current = unv;
                        min = distance[unv];
                    }
            } while (unvisited.Count != 0); // !visited[dest]
            return (distance, prev);
        }
    }
}
