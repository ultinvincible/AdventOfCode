using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _23_AmphipodRooms : AoCDay
    {
        static readonly int[] HallwayCols = new int[] { 0, 1, 3, 5, 7, 9, 10 };
        static readonly (int row, int col)[] positions = new (int, int)[]
        {
            (0,0), (0,1),    (0,3),    (0,5),    (0,7),    (0,9), (0,10),
                        (1,2),    (1,4),    (1,6),    (1,8),
                        (2,2),    (2,4),    (2,6),    (2,8)
        };

        long Hash(int[,] map)
        {
            string result = "";
            foreach ((int row, int col) in positions)
                result += map[row, col];
            return long.Parse(result);
        }

        int[,] Restore(long hash)
        {
            string str = hash.ToString().PadLeft(15, '0');
            int[,] map = inputMap.Clone() as int[,];
            for (int c = 0; c < 15; c++)
                map[positions[c].row, positions[c].col] = (int)char.GetNumericValue(str[c]);
            return map;
        }
        int[,] inputMap = new int[3, 11]; 
        List<long> mapHashes;
        List<(int nei, int weight)> Next(int i)
        {
            List<(int nei, int weight)> result = new();
            int[,] map = Restore(mapHashes[i]);

            if (debug) Console.WriteLine(PrintHash(i) + "\n");

            foreach ((int row, int col) in positions)
            {
                if (map[1, col] * 2 == col && map[2, col] * 2 == col
                    || map[row, col] == 0
                    || (row == 2 && map[1, col] != 0))
                    continue;
                int ampType = map[row, col];

                List<int> MovableCols = new(HallwayCols);
                if (Array.FindIndex(HallwayCols, c => c == col) != -1)
                    MovableCols = new();
                MovableCols.Insert(0, ampType * 2);
                foreach (int hwC in HallwayCols)
                    if (map[0, hwC] != 0 && hwC != col)
                        if (col < hwC) MovableCols.RemoveAll(c => hwC <= c);
                        else MovableCols.RemoveAll(c => c <= hwC);

                foreach (int destCol in MovableCols)
                {
                    int destRow = 0;
                    if (destCol == ampType * 2)
                    {
                        if (map[2, destCol] != ampType && map[2, destCol] != 0
                            || map[1, destCol] != 0) continue;
                        destRow = map[2, destCol] == 0 ? 2 : 1;
                    }

                    int[,] newMap = map.Clone() as int[,];
                    newMap[row, col] = 0;
                    newMap[destRow, destCol] = ampType;
                    long hash = Hash(newMap);
                    int index = mapHashes.IndexOf(hash);
                    if (index == -1) { mapHashes.Add(hash); index = mapHashes.Count - 1; }
                    int energy = (int)Math.Pow(10, ampType - 1) *
                        (row + Math.Abs(col - destCol) + destRow);
                    //if (destCol == ampType * 2)
                    //    return new() { (index, energy) };
                    result.Add((index, energy));

                    if (debug) Console.WriteLine((result.Count - 1).ToString().PadLeft(2)
                         + ": " + PrintHash(index) + "    " + energy);
                }
            }

            if (debug)
                Console.WriteLine(new string('-', 25) + "\n");
            return result;
        }
        protected override void Run()
        {
            //debug = true;
            for (int row = 1; row < inputLines.Length - 1; row++)
                for (int col = 1; col < inputLines[0].Length - 1; col++)
                {
                    int value;
                    if (col >= inputLines[row].Length
                        || inputLines[row][col] == '#'
                        || inputLines[row][col] == ' ')
                        value = -1;
                    else if (inputLines[row][col] == '.')
                        value = 0;
                    else value = inputLines[row][col] - 'A' + 1;
                    inputMap[row - 1, col - 1] = value;
                }
            if (debug) Console.WriteLine(GridStr(inputMap, i => i == -1 ? " " : i.ToString()));
            mapHashes = new() { Hash(inputMap) };

            if (!debug)
            {
                var result = Dijkstras(Next, i => mapHashes[i] == 12341234);
                int index = mapHashes.IndexOf(12341234);
                part1 = result[index].weight;

                string print = "";
                do
                {
                    print = PrintHash(index) + "\n" + print;
                    index = result[index].prev;
                } while (index != 0);
                Console.WriteLine(print);
            }
            else
            {
                int current = 0;
                do
                {
                    List<(int nei, int weight)> next = Next(current);
                    if (next.Count == 1)
                    {
                        current = next[0].nei;
                        Console.WriteLine("Next: 0");
                    }
                    else
                    {
                        Console.Write("Next: ");
                        current = next[int.Parse(Console.ReadLine())].nei;
                    }
                } while (mapHashes[current] != 12341234);
            }
        }

        string PrintHash(int index)
        {
            char[] a = mapHashes[index].ToString().Replace('0', '.').PadLeft(15, '.').ToCharArray();
            for (int c = 0; c < a.Length; c++)
                if (a[c] != '.')
                    a[c] = (char)(char.GetNumericValue(a[c]) + 'A' - 1);
            string s = string.Join("", a);
            return string.Join("-", new object[] { s[..2], s[2], s[3], s[4], s[5..7] })
                + "\n  " + string.Join(" ", a[7..11]) + "\n  " + string.Join(" ", a[11..]);
        }
    }
}