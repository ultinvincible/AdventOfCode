using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Advent_of_Code
{
    abstract class AoCDay
    {
        protected string input;
        protected string[] inputLines;
        protected string[][] inputSections;
        protected long part1, part2;
        protected string part1_str = "", part2_str = "";
        public (string part1_str, string part2_str, decimal time) Run(string inputPath)
        {
            input = File.ReadAllText(inputPath).Replace("\r\n", "\n");
            inputLines = File.ReadAllLines(inputPath);
            inputSections = Array.ConvertAll(input.Split("\n\n"),
                s => s.Split("\n", StringSplitOptions.RemoveEmptyEntries));
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

        // Helper functions
        protected static bool debug = false;
        protected const char blockCharacter = '\u2588';
        protected T[,] GridParse<T>(Func<int, int, T> Converter, string[] lines = null)
        {
            lines ??= inputLines;
            T[,] result = new T[lines.Length, lines[0].Length];
            for (int row = 0; row < lines.Length; row++)
                for (int col = 0; col < lines[row].Length; col++)
                    result[row, col] = Converter(row, col);
            return result;
        }
        protected T[,] GridParse<T>(Func<char, T> Converter, int fromLine = 0)
            => GridParse((row, col) => Converter(inputLines[fromLine..][row][col]),
                inputLines[fromLine..]);
        protected int[,] GridParse()
            => GridParse(c => (int)char.GetNumericValue(c));

        protected static string GridStr(int length0, Func<int, int> GetLength1,
            Func<int, int, string> ToStr, bool numbered = false)
        {
            string result = "";
            for (int row = 0; row < length0; row++)
            {
                if (numbered) result += row + ": ";
                for (int col = 0; col < GetLength1(row); col++)
                    result += ToStr(row, col);
                result += "\n";
            }
            return result;
        }

        protected static string GridStr<T>(T[,] input,
            Func<int, int, string> ToStr = null, bool numbered = false)
        {
            ToStr ??= (row, col) => input[row, col].ToString();
            return GridStr(input.GetLength(0), _ => input.GetLength(1), ToStr, numbered);
        }
        protected static string GridStr<T>(T[,] input,
            Func<T, string> ToStr, bool numbered = false)
            => GridStr(input, (row, col) => ToStr(input[row, col]), numbered);

        protected static string GridStr<T>(IList<IList<T>> input,
            Func<int, int, string> ToStr = null, bool numbered = false)
        {
            ToStr ??= (row, col) => input[row][col].ToString();
            return GridStr(input.Count, row => input[row].Count, ToStr, numbered);
        }
        protected static string GridStr<T>(IList<IList<T>> input,
            Func<T, string> ToStr, bool numbered = false)
            => GridStr(input, (row, col) => ToStr(input[row][col]), numbered);

        protected static List<(int row, int col)> Neighbors
            (int row, int col, Func<int, int, bool> OutOfBounds = null,
            bool diagonal = false, bool self = false)
        {
            OutOfBounds ??= (_, _) => false;
            List<(int, int)> neighbors = new(9);
            for (int neiRow = row - 1; neiRow <= row + 1; neiRow++)
                for (int neiCol = col - 1; neiCol <= col + 1; neiCol++)
                    if ((diagonal || neiRow == row || neiCol == col) &&
                        (self || (neiRow, neiCol) != (row, col)) &&
                        !OutOfBounds(neiRow, neiCol))
                        neighbors.Add((neiRow, neiCol));
            return neighbors;
        }
        protected static List<(int row, int col)> Neighbors<T>
            (int row, int col, T[,] grid, bool diagonal = false, bool self = false)
            => Neighbors(row, col, (r, c) => OutOfBounds(r, c, grid), diagonal, self);
        protected static bool OutOfBounds<T>(int row, int col, T[,] grid)
            => row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1);

        // All comments are stolen from https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        static protected List<(int prev, int distance)> Dijkstras(
            Func<int, List<(int nei, int distance)>> Neighbors, Func<int, bool> IsDestination = null, int start = 0)
        {
            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            List<bool> visited = new();
            HashSet<int> unvisited = new();
            IsDestination ??= _ => unvisited.Count == 0;

            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.
            List<(int prev, int distance)> nodes = new();
            for (int i = 0; i < start; i++)
            {
                visited.Add(false);
                nodes.Add((-1, int.MaxValue));
            }
            visited.Add(false);
            nodes.Add((-1, 0));
            int current = start;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            while (true)
            {
                foreach ((int nei, int distance) in Neighbors(current))
                {
                    if (nei >= visited.Count)
                        // Extend lists
                        for (int i = visited.Count; i <= nei; i++)
                        {
                            visited.Add(false);
                            nodes.Add((int.MaxValue, -1));
                        }
                    if (visited[nei]) continue;
                    unvisited.Add(nei);
                    int newDist = nodes[current].distance + distance;
                    if (newDist < nodes[nei].distance)
                        nodes[nei] = (current, newDist);
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
                    if (min > nodes[unv].distance)
                    {
                        current = unv;
                        min = nodes[unv].distance;
                    }
            }
            return nodes;
        }

        static protected (int prevRow, int prevCol, int distance)[,] Dijkstras(int[,] grid,
            Func<(int row, int col), List<(int row, int col, int distance)>> Neighbors,
            (int row, int col) start = default, (int row, int col)? destination = null)
        {
            int length0 = grid.GetLength(0), length1 = grid.GetLength(1);

            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            bool[,] visited = new bool[length0, length1];
            HashSet<(int row, int col)> unvisited = new();
            destination ??= (-1, -1);

            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.
            (int prevRow, int prevCol, int distance)[,] nodes =
                new (int prevRow, int prevCol, int distance)[length0, length1];
            for (int row = 0; row < length0; row++)
                for (int col = 0; col < length1; col++)
                    nodes[row, col] = (row, col) != start ? (-1, -1, int.MaxValue) : (-1, -1, 0);
            (int row, int col) current = start;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            while (true)
            {
                foreach ((int row, int col, int distance) in Neighbors(current))
                {
                    if (visited[row, col]) continue;
                    unvisited.Add((row, col));
                    int newdistance = nodes[current.row, current.col].distance + distance;
                    if (newdistance < nodes[row, col].distance)
                        nodes[row, col] = (current.row, current.col, newdistance);
                }

                // Mark the current node as visited and remove it from the unvisited set. 
                visited[current.row, current.col] = true;
                unvisited.Remove(current);

                // If the destination node has been marked visited or if the smallest
                // tentative distance among the nodes in the unvisited set is infinity
                // then stop. The algorithm has finished.
                // Otherwise, select the unvisited node that is marked with the smallest
                // tentative distance, set it as the new current node.
                if (current == destination) break;
                int min = int.MaxValue;
                foreach ((int row, int col) in unvisited)
                    if (min > nodes[row, col].distance)
                    {
                        current = (row, col);
                        min = nodes[row, col].distance;
                    }
                if (min == int.MaxValue) break;
            }
            return nodes;
        }
    }
}
