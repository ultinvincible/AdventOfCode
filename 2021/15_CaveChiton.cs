using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _15_CaveChiton : AoCDay
    {
        // All comments are stolen from https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        static (List<int> distance, List<int> prev) Dijkstras(
           Func<int, int, int> PathWeight, Func<int, List<int>> Neighbors, int dest = 0)
        {
            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            List<bool> visited = new() { false };
            HashSet<int> unvisited = new();

            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.
            List<int> distance = new() { 0 };
            List<int> prev = new() { int.MaxValue };
            int current = 0;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            do
            {
                foreach (int nei in Neighbors(current))
                {
                    if (nei >= visited.Count)
                        // Extend lists
                        for (int i = visited.Count; i <= nei; i++)
                        {
                            visited.Add(false);
                            distance.Add(int.MaxValue);
                            prev.Add(int.MaxValue);
                        }
                    if (!visited[nei])
                    {
                        unvisited.Add(nei);
                        int newDist = distance[current] + PathWeight(current, nei);
                        if (newDist < distance[nei])
                        {
                            distance[nei] = newDist;
                            prev[nei] = current;
                        }
                    }
                }

                // Mark the current node as visited and remove it from the unvisited set. 
                visited[current] = true;
                unvisited.Remove(current);

                // If the destination node has been marked visited or if the smallest
                // tentative distance among the nodes in the unvisited set is infinity
                // then stop. The algorithm has finished.
                // Otherwise, select the unvisited node that is marked with the smallest
                // tentative distance, set it as the new current node.
                int min = int.MaxValue;
                foreach (var unv in unvisited)
                    if (min > distance[unv])
                    {
                        current = unv;
                        min = distance[unv];
                    }
            } while (dest == 0 ? unvisited.Count != 0 : dest >= visited.Count || !visited[dest]);
            return (distance, prev);
        }
        int Dijkstras(int[,] cavern)
        {
            int lengthY = cavern.GetLength(0),
                lengthX = cavern.GetLength(1);
            var result = Dijkstras((_, nei) => cavern[Math.DivRem(nei, lengthX, out int j), j],
                index => Neighbors(Math.DivRem(index, lengthX, out int x), x, lengthY, lengthX)
                        .ConvertAll(n => n.y * lengthX + n.x), lengthY * lengthX - 1);
            return result.distance[^1];
        }

        protected override void Run()
        {
            int lengthY = inputLines.Length, lengthX = inputLines[0].Length;
            int[,] cavern1 = new int[lengthY, lengthX];
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                    cavern1[y, x] = (int)char.GetNumericValue(inputLines[y][x]);
            part1 = Dijkstras(cavern1);

            int[,] cavern2 = new int[lengthY * 5, lengthX * 5];
            for (int y = 0; y < lengthY; y++)
                for (int x = 0; x < lengthX; x++)
                    for (int down = 0; down < 5; down++)
                        for (int right = 0; right < 5; right++)
                            cavern2[y + lengthY * down, x + lengthX * right] =
                                (cavern1[y, x] + down + right - 1) % 9 + 1;
            part2 = Dijkstras(cavern2);
        }
    }
}