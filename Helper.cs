using System;
using System.Collections.Generic;
using System.IO;

namespace Advent_of_Code
{
    abstract class AoCDay
    {
        public int day;
        readonly public string inputString;
        readonly public string[] input;
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
        protected T[,] GridParse<T>(Func<double, T> converter)
        {
            T[,] result = new T[input.Length, input[0].Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    result[y, x] = converter(char.GetNumericValue(input[y][x]));
            return result;
        }
        protected static string CollStr<T>(IEnumerable<T> coll, int padLength = 2)
            => CollStr(coll, item => item.ToString().PadLeft(padLength));
        protected static string CollStr<T>(IEnumerable<T> coll, string field, int padLength = 2)
            => CollStr(coll, item =>
            typeof(T).GetField(field).GetValue(item).ToString().PadLeft(padLength));
        static string CollStr<T>(IEnumerable<T> coll, Func<T, string> ToStr)
        {
            string result = "";
            foreach (T item in coll)
            {
                result += ToStr(item) + "|";
            }
            return result;
        }
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
        protected static (int[] distance, int[] prev) Dijkstras(int count,
            Func<int, int, int> PathWeight, Func<int, List<int>> Neighbors)
        {
            bool[] visited = new bool[count];
            int[] distance = new int[count];
            int[] prev = new int[count];
            for (int i = 0; i < count; i++)
            {
                //visited[i, j] = false;
                distance[i] = int.MaxValue;
                prev[i] = int.MaxValue;
            }
            HashSet<int> unvisited = new();
            int current = 0;
            distance[0] = 0;

            do
            {
                foreach (int nei in Neighbors(current))
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

                int min = int.MaxValue;
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
