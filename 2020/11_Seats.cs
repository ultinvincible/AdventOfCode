using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _11_Seats : AoCDay
    {
        char[,] seats;
        void ApplyRules(Func<int, int, bool> Change)
        {
            for (int row = 0; row < seats.GetLength(0); row++)
                for (int col = 0; col < seats.GetLength(1); col++)
                    seats[row, col] = inputLines[row][col];

            List<(int row, int col)> toChange;
            do
            {
                toChange = new();
                for (int row = 0; row < seats.GetLength(0); row++)
                    for (int col = 0; col < seats.GetLength(1); col++)
                    {
                        if (Change(row, col))
                            toChange.Add((row, col));
                    }
                foreach (var (row, col) in toChange)
                {
                    if (seats[row, col] == '#')
                        seats[row, col] = 'L';
                    else if (seats[row, col] == 'L')
                        seats[row, col] = '#';
                }
                //Console.WriteLine(GridStr(seats));
            } while (toChange.Count > 0);
        }
        readonly (int, int)[] directions = new[]
        { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        List<char> Adjacent(int row, int col)
            => Neighbors(row, col, seats.GetLength(0), seats.GetLength(1), true)
            .ConvertAll(((int row, int col) seat) => seats[seat.row, seat.col]);
        List<char> See(int row, int col)
        {
            List<char> result = new();
            foreach (var (rowDir, colDir) in directions)
            {
                for (int dist = 1; ; dist++)
                {
                    int seeRow = row + rowDir * dist, seeCol = col + colDir * dist;
                    if (OutOfBounds(seeCol, seeRow, seats.GetLength(1), seats.GetLength(0)))
                        break;
                    char s = seats[seeRow, seeCol];
                    if (s != '.') { result.Add(s); break; }
                }
            }
            return result;
        }
        int OccupiedCount()
        {
            int count = 0;
            foreach (char seat in seats)
                if (seat == '#') count++;
            return count;
        }
        protected override void Run()
        {
            seats = new char[inputLines.Length, inputLines[0].Length];

            ApplyRules((row, col) =>
            {
                List<char> adj = Adjacent(row, col);
                return seats[row, col] == 'L' && !adj.Contains('#') ||
                (seats[row, col] == '#' && adj.Where(c => c == '#').Count() >= 4);
            });
            part1 = OccupiedCount();

            ApplyRules((row, col) =>
            {
                List<char> see = See(row, col);
                return seats[row, col] == 'L' && !see.Contains('#') ||
                (seats[row, col] == '#' && see.Where(c => c == '#').Count() >= 5);
            });
            part2 = OccupiedCount();
        }
    }
}
