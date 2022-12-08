using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    abstract class AoCDay
    {
        protected string input;
        protected string[] inputLines;
        protected long part1, part2;
        protected string part1_str = "", part2_str = "";
        public (string part1_str, string part2_str, decimal time) Run(string inputPath)
        {
            input = File.ReadAllText(inputPath).Replace("\r\n", "\n");
            inputLines = File.ReadAllLines(inputPath);
            Stopwatch watch = Stopwatch.StartNew();
            Run();
            watch.Stop();

            if (part1 != 0) part1_str = part1.ToString();
            else if (part1_str == "") part1_str = "Not done.";
            if (part2 != 0) part2_str = part2.ToString();
            else if (part2_str == "") part2_str = "Not done.";
            return (part1_str, part2_str, watch.ElapsedMilliseconds);
        }
        protected abstract void Run();

        public ValueTask<string> Solve(IAsyncEnumerable<string> entries)
        {
            throw new NotImplementedException();
        }

        // Helper functions
        protected static bool debug = false;
        protected static T[,] GridParse<T>(string[] input, Func<char, T> Converter)
        {
            T[,] result = new T[input.Length, input[0].Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    result[y, x] = Converter(input[y][x]);
            return result;
        }
        protected bool[,] GridParse(char cTrue, int fromLine = 0)
            => GridParse(inputLines[fromLine..], c => c == cTrue);
        protected int[,] GridParse(int fromLine = 0)
            => GridParse(inputLines[fromLine..], c => (int)char.GetNumericValue(c));

        protected static string GridStr<T>(T[,] input,
            Func<T, string> ToStr = null, string pad = "", bool numbered = false)
        {
            if (ToStr is null) ToStr = t => t.ToString();
            string result = "";
            for (int y = 0; y < input.GetLength(0); y++)
            {
                if (numbered) result += y + ": ";
                for (int x = 0; x < input.GetLength(1); x++)
                    result += ToStr(input[y, x]) + pad;
            }
            result += "\n";
            return result;
        }
        protected static string GridStr<T>(IEnumerable<IEnumerable<T>> input,
            Func<T, string> ToStr = null, string pad = "", bool numbered = false)
        {
            ToStr ??= t => t.ToString();
            string result = "";
            int i = 0;
            foreach (IEnumerable<T> row in input)
            {
                if (numbered) { result += i + ": "; i++; }
                foreach (T item in row)
                    result += ToStr(item) + pad;
                result += "\n";
            }
            return result;
        }
        // Forgot string.Join() exists, kekw
        //protected static string CollStr<T>(IList<T> coll, Func<T, string> ToStr, string pad = "")
        //{
        //    string result = "";
        //    for (int i = 0; i < coll.Count - 1; i++)
        //        result += ToStr(coll[i]) + pad;
        //    return result + ToStr(coll[^1]);
        //}
        //protected static string CollStr<T>(IList<T> coll, string pad = "")
        //    => CollStr(coll, t => t.ToString(), pad);

        protected static List<(int y, int x)> Neighbors
            (int y, int x, bool diagonal = false, bool self = false)
        {
            List<(int, int)> neighbors = new();
            for (int neiY = y - 1; neiY <= y + 1; neiY++)
                for (int neiX = x - 1; neiX <= x + 1; neiX++)
                    if ((diagonal || neiY == y || neiX == x) &&
                        (self || (neiY, neiX) != (y, x)))
                        neighbors.Add((neiY, neiX));
            return neighbors;
        }
        protected static List<(int y, int x)> Neighbors
            (int y, int x, int boundY, int boundX, bool diagonal = false, bool self = false)
        {
            List<(int y, int x)> neighbors = Neighbors(y, x, self, diagonal);
            neighbors.RemoveAll(nei => OutOfBounds(nei.y, nei.x, boundY, boundX));
            return neighbors;
        }
        protected static bool OutOfBounds(int y, int x, int boundY, int boundX)
            => y < 0 || y >= boundY || x < 0 || x >= boundX;

        /// <summary>
        /// Dijkstra's algorithm
        /// </summary>
        /// <param name="Neighbors"></param>
        /// <param name="IsDestination"></param>
        /// <returns></returns>
        // All comments are stolen from https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        // "distance" is named weight
        static protected List<(int weight, int prev)> Dijkstras
            (Func<int, List<(int, int)>> Neighbors, Func<int, bool> IsDestination = null)
        {
            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            List<bool> visited = new() { false };
            HashSet<int> unvisited = new();
            IsDestination ??= _ => unvisited.Count == 0;

            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.
            List<(int weight, int prev)> nodes = new() { (0, int.MaxValue) };
            int current = 0;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            while (true)
            {
                foreach ((int nei, int weight) in Neighbors(current))
                {
                    if (nei >= visited.Count)
                        // Extend lists
                        for (int i = visited.Count; i <= nei; i++)
                        {
                            visited.Add(false);
                            nodes.Add((int.MaxValue, int.MaxValue));
                        }
                    if (!visited[nei])
                    {
                        unvisited.Add(nei);
                        int newDist = nodes[current].weight + weight;
                        if (newDist < nodes[nei].weight)
                            nodes[nei] = (newDist, current);
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
                if (IsDestination(current)) break;
                int min = int.MaxValue;
                foreach (int unv in unvisited)
                    if (min > nodes[unv].weight)
                    {
                        current = unv;
                        min = nodes[unv].weight;
                    }
            }
            return nodes;
        }
    }
}
