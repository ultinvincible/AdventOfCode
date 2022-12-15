﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    abstract class AoCDay
    {
        protected string input;
        protected string[] inputLines, inputSections;
        protected long part1, part2;
        protected string part1_str = "", part2_str = "";
        public (string part1_str, string part2_str, decimal time) Run(string inputPath)
        {
            input = File.ReadAllText(inputPath).Replace("\r\n", "\n");
            inputLines = File.ReadAllLines(inputPath);
            inputSections = input.Split("\n\n");
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
        protected static T[,] GridParse<T>(string[] input, Func<char, T> Converter)
        {
            T[,] result = new T[input.Length, input[0].Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    result[y, x] = Converter(input[y][x]);
            return result;
        }
        protected static T[,] GridParse<T>(string[] input, Func<int, int, T> Converter)
        {
            T[,] result = new T[input.Length, input[0].Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    result[y, x] = Converter(y, x);
            return result;
        }
        protected int[,] GridParse()
            => GridParse(inputLines, c => (int)char.GetNumericValue(c));

        protected static string GridStr<T>(T[,] input,
            Func<T, string> ToStr = null, string pad = "", bool numbered = false)
        {
            ToStr ??= t => t.ToString();
            string result = "";
            for (int row = 0; row < input.GetLength(0); row++)
            {
                if (numbered) result += row + ": ";
                for (int col = 0; col < input.GetLength(1); col++)
                    result += ToStr(input[row, col]) + pad;
                result += "\n";
            }
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
                result += string.Join(pad, row) + "\n";
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

        protected static List<(int row, int col)> Neighbors
            (int row, int col, bool diagonal = false, bool self = false)
        {
            List<(int, int)> neighbors = new();
            for (int neiY = row - 1; neiY <= row + 1; neiY++)
                for (int neiX = col - 1; neiX <= col + 1; neiX++)
                    if ((diagonal || neiY == row || neiX == col) &&
                        (self || (neiY, neiX) != (row, col)))
                        neighbors.Add((neiY, neiX));
            return neighbors;
        }
        protected static List<(int row, int col)> Neighbors
            (int row, int col, int boundRow, int boundCol, bool diagonal = false, bool self = false)
        {
            List<(int row, int col)> neighbors = Neighbors(row, col, self, diagonal);
            neighbors.RemoveAll(nei => OutOfBounds(nei.row, nei.col, boundRow, boundCol));
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
        static protected List<(int prev, int weight)> Dijkstras(
            Func<int, List<(int nei, int weight)>> Neighbors, Func<int, bool> IsDestination = null, int start = 0)
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
            List<(int prev, int weight)> nodes = new();
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
                foreach ((int nei, int weight) in Neighbors(current))
                {
                    if (nei >= visited.Count)
                        // Extend lists
                        for (int i = visited.Count; i <= nei; i++)
                        {
                            visited.Add(false);
                            nodes.Add((int.MaxValue, -1));
                        }
                    if (!visited[nei])
                    {
                        unvisited.Add(nei);
                        int newDist = nodes[current].weight + weight;
                        if (newDist < nodes[nei].weight)
                            nodes[nei] = (current, newDist);
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

        static protected (int prevRow, int prevCol, int weight)[,] Dijkstras(int[,] grid,
            Func<(int row, int col), List<(int row, int col, int weight)>> Neighbors,
            (int row, int col) start = default, (int row, int col)? destination = null)
        {
            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            HashSet<(int row, int col)> unvisited = new();

            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.
            (int prevRow, int prevCol, int weight)[,] nodes =
                new (int prevRow, int prevCol, int weight)[grid.GetLength(0), grid.GetLength(1)];
            for (int row = 0; row < grid.GetLength(0); row++)
                for (int col = 0; col < grid.GetLength(1); col++)
                    nodes[row, col] = (row, col) != start ? (-1, -1, int.MaxValue) : (-1, -1, 0);
            (int row, int col) current = start;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            while (true)
            {
                foreach ((int row, int col, int weight) in Neighbors(current))
                {
                    if (visited[row, col]) continue;
                    unvisited.Add((row, col));
                    int newWeight = nodes[current.row, current.col].weight + weight;
                    if (newWeight < nodes[row, col].weight)
                        nodes[row, col] = (current.row, current.col, newWeight);
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
                    if (min > nodes[row, col].weight)
                    {
                        current = (row, col);
                        min = nodes[row, col].weight;
                    }
                if (min == int.MaxValue) break;
            }
            return nodes;
        }
    }
}
