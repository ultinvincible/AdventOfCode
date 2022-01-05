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
        protected static T[,] GridParse<T>(string[] input, Func<char, T> Converter)
        {
            T[,] result = new T[input.Length, input[0].Length];
            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[0].Length; x++)
                    result[y, x] = Converter(input[y][x]);
            return result;
        }
        protected static bool[,] GridParse(string[] input)
            => GridParse(input, c => { if (c == '#') return true; return false; });
        protected bool[,] GridParse()
            => GridParse(input);
        protected static int[,] GridParse(string[] input, Func<double, int> Converter)
            => GridParse(input, c => Converter(char.GetNumericValue(c)));
        protected int[,] GridParse(Func<double, int> Converter)
            => GridParse(input, Converter);

        protected string[] GridStr(bool[,] input, char cTrue = '#', char cFalse = '.')
            => GridStr(input, b => { if (b) return cTrue; return cFalse; });
        protected string[] GridStr<T>(T[,] input, Func<T, char> Print)
        {
            string[] result = new string[input.GetLength(0)];
            for (int y = 0; y < input.GetLength(0); y++)
                for (int x = 0; x < input.GetLength(1); x++)
                    result[y] += Print(input[y, x]);
            return result;
        }
        protected static string CollStr<T>(IEnumerable<T> coll, Func<T, string> ToStr)
        {
            string result = "";
            foreach (T item in coll)
            {
                result += ToStr(item);
            }
            return result;
        }

        //protected static List<(int, int)> Neighbors((int, int) yx, int boundY, int boundX)
        //{
        //    (int y, int x) = yx;
        //    return Neighbors(y, x, boundY, boundX);
        //}
        protected static List<(int y, int x)> Neighbors
            (int y, int x, bool self = false, bool diagonal = false)
        {
            List<(int, int)> neighbors = new();
            for (int neiY = y - 1; neiY <= y + 1; neiY++)
                for (int neiX = x - 1; neiX <= x + 1; neiX++)
                    if ((diagonal || neiY == y || neiX == x) &&
                        (self || (neiY, neiX) != (y, x)))
                        neighbors.Add((neiY, neiX));
            return neighbors;
        }
        protected static List<(int, int)> Neighbors
            (int y, int x, int boundY, int boundX, bool self = false, bool diagonal = false)
        {
            List<(int y, int x)> neighbors = Neighbors(y, x, self, diagonal);
            neighbors.RemoveAll(nei => OutOfBounds(nei.y, nei.x, boundY, boundX));
            return neighbors;
        }
        protected static bool OutOfBounds(int y, int x, int boundY, int boundX)
            => y < 0 || y >= boundY || x < 0 || x >= boundX;

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
