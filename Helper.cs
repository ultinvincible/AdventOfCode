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
            => GridParse(input[fromLine..], c => { if (c == cTrue) return true; return false; });
        protected int[,] GridParse(int fromLine = 0)
            => GridParse(input[fromLine..], c => (int)char.GetNumericValue(c));

        protected static string[] GridStr(bool[,] input, char cTrue = '#', char cFalse = '.')
            => GridStr(input, b => { if (b) return cTrue; return cFalse; });
        protected static string[] GridStr<T>(T[,] input, Func<T, char> ToStr)
        {
            string[] result = new string[input.GetLength(0)];
            for (int y = 0; y < input.GetLength(0); y++)
                for (int x = 0; x < input.GetLength(1); x++)
                    result[y] += ToStr(input[y, x]);
            return result;
        }
        protected static string CollStr<T>(IEnumerable<T> coll, string pad = "")
            => CollStr(coll, t => t.ToString() + pad);
        protected static string CollStr<T>(IEnumerable<T> coll, Func<T, string> ToStr)
        {
            string result = "";
            foreach (T item in coll)
            {
                result += ToStr(item);
            }
            return result;
        }

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

        /// <summary>
        /// Generic Dijkstra's algorithm, finds path from start to destination
        /// </summary>
        /// <typeparam name="Key">Unique indentifier for a vertex</typeparam>
        /// <param name="start"></param>
        /// <param name="dest"></param>
        /// <param name="PathWeight"></param>
        /// <param name="Nei_key_dist"></param>
        /// <param name="Heuristic"></param>
        /// <returns></returns>
        protected static List<(Key key, int distance, int prev)> A_Star<Key>(
            Key start, Key dest,
            Func<Key, List<(Key, int)>> Nei_key_dist,
            Func<Key, Key, bool> Equal,
            Func<Key, int> Heuristic = null)
        {
            (Key key, int distance, int) current = (start, 0, -1);
            int curIndex = 0;
            List<(Key key, int distance, int)>
                visited = new(), unvisited = new() { current };

            do
            {
                int min = int.MaxValue;
                for (int i = 0; i < unvisited.Count; i++)
                {
                    var (key, distance, prev) = unvisited[i];
                    int unvDistHeur = distance;
                    if (Heuristic != null)
                        unvDistHeur += Heuristic(key);
                    if (min > unvDistHeur)
                    {
                        current = (key, distance, prev);
                        curIndex = i;
                        min = unvDistHeur;
                    }
                }

                List<(Key, int)> neis = Nei_key_dist(current.key);
                foreach (var (nei, pathWeight) in neis)
                {
                    if (visited.FindIndex(v => Equal(v.key, nei)) != -1) // too costly
                        continue;
                    int newDist = current.distance + pathWeight;
                    int find = unvisited.FindIndex(u => Equal(u.key, nei));
                    if (find == -1)
                        unvisited.Add((nei, newDist, visited.Count));
                    else if (newDist < unvisited[find].distance)
                        unvisited[find] = (nei, newDist, visited.Count);
                }

                visited.Add(current);
                unvisited.RemoveAt(curIndex);
                //if (debug)
                //{
                //Console.WriteLine(current.key.ToString()+ "Energy: " + current.distance);
                //Console.WriteLine(new string('-', 16));
                //Console.WriteLine(CollStr(unvisited,
                //    p => p.key.ToString() + "Energy: " + p.distance + "\n"));
                //Console.WriteLine(new string('\u2588', 16));
                //}
            } while (!Equal(current.key, dest));
            //} while (unvisited.Count != 0);
            return visited;
        }
    }
}
