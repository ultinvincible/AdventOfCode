using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    internal class _24_Blizzards : AoCDay
    {
        static (int dimension, int value)[] directions = new (int dimension, int value)[]
        {
            (1, 1), (0, 1), (1, -1), (0, -1), (0, 0)
        };
        static readonly char[] facing = new char[] { '>', 'v', '<', '^' };
        readonly List<List<int>[,]> maps = new();
        int[] length = new int[2];
        List<(int row, int col, int minute)> states = new() { (-1, 0, 0) };
        (int row, int col) start = (-1, 0), dest;
        protected override void Run()
        {
            debug = 0;
            length[0] = inputLines.Length - 2;
            length[1] = inputLines[0].Length - 2;
            maps.Add(new List<int>[length[0], length[1]]);
            for (int row = 0; row < length[0]; row++)
                for (int col = 0; col < length[1]; col++)
                {
                    maps[0][row, col] = new();
                    int dir = Array.IndexOf(facing, inputLines[row + 1][col + 1]);
                    if (dir != -1) maps[0][row, col].Add(dir);
                }
            dest = (length[0], length[1] - 1);

            List<(int prev, int distance)> tree = Dijkstras(Next, i =>
                (states[i].row, states[i].col) == dest);
            int index = states.FindIndex(s => (s.row, s.col) == dest);
            part1 = tree[index].distance;
            if (debug == 2)
            {
                List<int> path = new() { index };
                do path.Add(tree[path[^1]].prev);
                while (path[^1] != 0);
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    Console.WriteLine("Minute " + (path.Count - 1 - i));
                    (int row, int col, int minute) = states[path[i]];
                    PrintMap(minute, row, col);
                }
            }

            states = new() { states[index] };
            Dijkstras(Next, i => (states[i].row, states[i].col) == start);
            index = states.FindIndex(s => (s.row, s.col) == start);
            states = new() { states[index] };
            tree = Dijkstras(Next, i => (states[i].row, states[i].col) == dest);
            part2 = states[states.FindIndex(s => (s.row, s.col) == dest)].minute;
        }
        List<(int prev, int distance)> Next(int i)
        {
            (int posRow, int posCol, int minute) = states[i];
            List<(int nei, int distance)> result = new();
            if (maps.Count == minute + 1)
            {
                maps.Add(new List<int>[length[0], length[1]]);
                for (int row = 0; row < length[0]; row++)
                    for (int col = 0; col < length[1]; col++)
                        maps[^1][row, col] = new();
                for (int row = 0; row < length[0]; row++)
                    for (int col = 0; col < length[1]; col++)
                        foreach (int dir in maps[^2][row, col])
                        {
                            (int dimension, int value) = directions[dir];
                            int[] newPosition = new int[] { row, col };
                            newPosition[dimension] += value;
                            if (newPosition[dimension] == length[dimension]) newPosition[dimension] = 0;
                            else if (newPosition[dimension] == -1) newPosition[dimension] = length[dimension] - 1;
                            maps[^1][newPosition[0], newPosition[1]].Add(dir);
                        }
            }
            if (debug == 1)
            {
                Console.WriteLine((posRow, posCol, minute));
                PrintMap(maps.Count - 1, posRow, posCol);
            }

            List<int>[,] map = maps[^1];
            List<(int row, int col, int minute)> next;
            if ((posRow, posCol) == start) next = new() { (0, 0, minute + 1) };
            else if ((posRow, posCol) == dest) next = new() { (length[0] - 1, length[1] - 1, minute + 1) };
            else
            {
                next = new();
                foreach ((int dimension, int value) in directions)
                {
                    int[] newPosition = new int[] { posRow, posCol }; ;
                    newPosition[dimension] += value;
                    if ((newPosition[0], newPosition[1]) == start || (newPosition[0], newPosition[1]) == dest)
                    {
                        next.Add((newPosition[0], newPosition[1], minute + 1));
                        continue;
                    }
                    if (newPosition[dimension] == length[dimension] ||
                       newPosition[dimension] == -1 ||
                       map[newPosition[0], newPosition[1]].Count != 0) // hit a wall or blizzard
                        continue;
                    next.Add((newPosition[0], newPosition[1], minute + 1));
                }
            }

            foreach ((int row, int col, int min) in next)
            {
                int index = states.IndexOf((row, col, min));
                if (index == -1)
                {
                    states.Add((row, col, min));
                    index = states.Count - 1;
                }
                result.Add((index, 1));
            }
            return result;
        }

        private void PrintMap(int minute, int row, int col)
        {
            Console.WriteLine(GridPrint(maps[minute], (r, c) =>
            {
                List<int> l = maps[minute][r, c];
                return (r, c) == (row, col) ? "E" :
                    l.Count == 0 ? "." :
                    l.Count == 1 ? facing[l[0]].ToString() :
                    l.Count.ToString();
            }));
        }
    }
}
