using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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
            input = File.ReadAllText(inputPath).Replace("\r\n", "\n").TrimEnd('\n');
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
        protected static int debug = 0;
        protected const string block = "\u2588";
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

        protected static string GridPrint<T>(T[,] input,
            Func<int, int, string> ToStr = null, bool numbered = false)
        {
            ToStr ??= (row, col) => input[row, col].ToString();
            StringBuilder print = Header(input.GetLength(1), numbered);
            for (int row = 0; row < input.GetLength(0); row++)
            {
                if (numbered) print.Append($"{row,2} ");
                for (int col = 0; col < input.GetLength(1); col++)
                    print.Append(ToStr(row, col));
                print.AppendLine();
            }
            return print.ToString();
        }
        protected static string GridPrint<T>(T[,] input,
            Func<T, string> ToStr, bool numbered = false)
            => GridPrint(input, (row, col) => ToStr(input[row, col]), numbered);

        protected static string GridPrint<T>(IEnumerable<IEnumerable<T>> input,
            Func<T, string> ToStr = null, bool numbered = false)
        {
            ToStr ??= item => item.ToString();
            StringBuilder print = Header(input.Count(), numbered);

            int row = 0;
            foreach (IEnumerable<T> line in input)
            {
                if (numbered) print.Append($"{row,2} ");
                foreach (T item in line)
                    print.Append(ToStr(item));
                print.AppendLine();
                row++;
            }
            return print.ToString();
        }

        private static StringBuilder Header(int length, bool numbered)
        {
            StringBuilder print = new();
            if (numbered)
            {
                string line0 = "   ", line1 = "   ";
                for (int i = 0; i < length; i++)
                {
                    string lastTwoDigits = (i % 100).ToString();
                    line0 += lastTwoDigits.Length == 1 ? ' ' : lastTwoDigits[0];
                    line1 += lastTwoDigits[^1];
                }
                print.AppendLine(line0);
                print.AppendLine(line1);
            }
            return print;
        }

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
        static protected Dictionary<S, (S prev, int cost)> Dijkstras<S>
            (Func<S, IEnumerable<(S next, int cost)>> NextStates, S start, Func<S, bool> IsDestination = null)
            where S : IEquatable<S>
        {
            // Mark all nodes unvisited.
            // Create a set of all the unvisited nodes called the unvisited set.
            // Assign to every node a tentative distance value:
            // set it to zero for our initial node
            // and to infinity for all other nodes.
            // Set the initial node as current.

            Dictionary<S, (S prev, int cost)> visited = new();
            Dictionary<S, (S prev, int cost)> unvisited = new() { { start, (default, 0) } };
            S current = start;

            // For the current node, consider all of its unvisited neighbors
            // and calculate their tentative distances through the current node.
            // Compare the newly calculated tentative distance to the one currently
            // assigned to the neighbor and assign it the smaller one.
            while (true)
            {
                foreach ((S next, int cost) in NextStates(current))
                {
                    int newCost = unvisited[current].cost + cost;
                    if (!visited.ContainsKey(next) && (!unvisited.ContainsKey(next) || newCost < unvisited[next].cost))
                        unvisited[next] = (current, newCost);
                }

                // Mark the current node as visited and remove it from the unvisited set. 
                visited.Add(current, unvisited[current]);
                if (!unvisited.Remove(current)) throw new Exception("KV pair not found");

                // If the destination node has been marked visited or if the smallest
                // tentative distance among the nodes in the unvisited set is infinity
                // then stop. The algorithm has finished.
                // Otherwise, select the unvisited node that is marked with the smallest
                // tentative distance, set it as the new current node.
                if (IsDestination is not null && IsDestination(current) || unvisited.Count == 0)
                    break;

                int min = int.MaxValue;
                foreach (S state in unvisited.Keys)
                    if (min > unvisited[state].cost)
                    {
                        current = state;
                        min = unvisited[state].cost;
                    }
            }
            return visited;
        }

        static protected (int prevRow, int prevCol, int cost)[,] Dijkstras(int[,] grid,
            Func<(int row, int col), IEnumerable<((int row, int col) pos, int cost)>> Neighbors,
            (int row, int col) start = default, (int row, int col)? destination = null)
        {
            Dictionary<(int row, int col), ((int row, int col) prev, int cost)> tree =
                Dijkstras(pos => Neighbors(pos), start, destination is null ? null : pos => pos == destination);
            (int prevRow, int prevCol, int cost)[,] result =
                new (int prevRow, int prevCol, int cost)[grid.GetLength(0), grid.GetLength(1)];
            for (int row = 0; row < grid.GetLength(0); row++)
                for (int col = 0; col < grid.GetLength(1); col++)
                    result[row, col] = (-1, -1, int.MaxValue);
            foreach (((int row, int col) pos, ((int row, int col) prev, int cost) node) in tree)
                result[pos.row, pos.col] = (node.prev.row, node.prev.col, node.cost);
            return result;
        }
    }
}
