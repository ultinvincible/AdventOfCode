using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _23_AmphipodRooms : AoCDay
    {
        static readonly int[] HallwayCols = new int[] { 0, 1, 3, 5, 7, 9, 10 };
        static readonly bool[] IsHallway = new bool[]
        { true, true, false, true, false, true, false, true, false, true, true };
        //static readonly (int row, int col)[] positions = new (int, int)[]
        //{
        //    (0,0), (0,1),    (0,3),    (0,5),    (0,7),    (0,9), (0,10),
        //                (1,2),    (1,4),    (1,6),    (1,8),
        //                (2,2),    (2,4),    (2,6),    (2,8)
        //};

        ulong Hash(int[,] map)
        {
            string result = "";
            foreach (int col in HallwayCols)
                result += map[0, col];
            for (int row = 1; row < map.GetLength(0); row++)
            {
                int rowCode = 0;
                for (int col = 2; col <= 8; col += 2)
                    rowCode += map[row, col] * (int)Math.Pow(5, (8 - col) / 2);
                result += rowCode.ToString("000");
            }
            return ulong.Parse(result);
        }

        int[,] Restore(ulong hash)
        {
            string str = hash.ToString();
            str = str.PadLeft(7 + (depth - 1) * 3, '0');
            int[,] map = new int[depth, 11];
            for (int i = 0; i < HallwayCols.Length; i++)
                map[0, HallwayCols[i]] = (int)char.GetNumericValue(str[i]);
            for (int row = 1; row < map.GetLength(0); row++)
            {
                int rowCode = int.Parse(str[(4 + row * 3)..(7 + row * 3)]);  // 7+(row-1)*3..+3
                int[] ampTypes = new int[4];
                for (int i = 0; i < 4; i++)
                    ampTypes[i] = Math.DivRem(rowCode, (int)Math.Pow(5, 3 - i), out rowCode);

                for (int col = 0; col < map.GetLength(1); col++)
                    if (IsHallway[col])
                        map[row, col] = -1;
                    else
                        map[row, col] = ampTypes[col / 2 - 1];
            }

            return map;
        }

        List<ulong> mapHashes;
        int depth;
        List<(int nei, int weight)> Next(int i)
        {
            List<(int nei, int weight)> result = new();
            int[,] map = Restore(mapHashes[i]);
            //int depth = map.GetLength(0);

            (bool correct, int topRow)[] roomData = new (bool correct, int topRow)[4];
            for (int room = 2; room <= 8; room += 2)
            {
                bool correct = true;
                int topRow = 1;
                for (int r = 1; r < depth; r++)
                    if (map[r, room] != 0)
                        correct &= map[r, room] * 2 == room;
                    else topRow++;
                roomData[room / 2 - 1] = (correct, topRow);
            }
            (bool correct, int topRow) Room(int col) => IsHallway[col] ?
               throw new Exception("Hallway column") : roomData[col / 2 - 1];

            if (debug == 1) Console.WriteLine(PrintMap(map) + "\n");

            for (int col = 0; col < IsHallway.Length; col++)
            {
                int row = 0;
                if (IsHallway[col])
                {
                    if (map[row, col] == 0) continue;
                }
                else
                {
                    (bool correct, int topRow) = Room(col);
                    if (correct) continue;
                    row = topRow;
                }
                int ampType = map[row, col];

                List<int> MovableCols = new() { ampType * 2 };
                if (!IsHallway[col])
                    MovableCols.AddRange(HallwayCols);
                foreach (int hwC in HallwayCols)
                    if (map[0, hwC] != 0 && hwC != col)
                        if (col < hwC) MovableCols.RemoveAll(c => hwC <= c);
                        else MovableCols.RemoveAll(c => c <= hwC);
                MovableCols.Remove(col);

                foreach (int destCol in MovableCols)
                {
                    int destRow = 0;
                    if (destCol == ampType * 2)
                    {
                        (bool correct, int topRow) = Room(destCol);
                        if (!correct || topRow == 1) continue;
                        destRow = topRow - 1;
                    }

                    int[,] newMap = map.Clone() as int[,];
                    newMap[row, col] = 0;
                    newMap[destRow, destCol] = ampType;
                    ulong hash = Hash(newMap);
                    int index = mapHashes.IndexOf(hash);
                    if (index == -1) { index = mapHashes.Count; mapHashes.Add(hash); }
                    int energy = (int)Math.Pow(10, ampType - 1) *
                        (row + Math.Abs(col - destCol) + destRow);

                    if (debug == 1) Console.WriteLine(result.Count + ":\n"
                         + PrintMap(newMap) + "    " + energy);

                    if (destCol == ampType * 2)
                        return new() { (index, energy) };
                    result.Add((index, energy));
                }
            }

            if (debug == 1)
                Console.WriteLine(new string('-', 25) + "\n");
            return result;
        }
        protected override void Run()
        {
            debug = 0;
            if (debug == 1) Console.WriteLine(Hash(new int[,]{
                {  0,  0, 0,  0, 0,  0, 0,  0, 0,  0,  0 },
                { -1, -1, 1, -1, 2, -1, 3, -1, 4, -1, -1 },
                { -1, -1, 1, -1, 2, -1, 3, -1, 4, -1, -1 },
            }).ToString());

            int[,] inputMap = new int[3, 11];
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
            if (debug == 1) Console.WriteLine(PrintMap(inputMap));

            string[] toInsert = new string[] {
            "#D#C#B#A#",
            "#D#B#A#C#"};
            int[,] realMap = new int[5, 11];
            for (int col = 0; col < realMap.GetLength(1); col++)
                if (!IsHallway[col])
                {
                    realMap[1, col] = inputMap[1, col];
                    realMap[2, col] = toInsert[0][col - 1] - 'A' + 1;
                    realMap[3, col] = toInsert[1][col - 1] - 'A' + 1;
                    realMap[4, col] = inputMap[2, col];
                }
                else for (int row = 1; row < realMap.GetLength(0); row++)
                        realMap[row, col] = -1;
            if (debug == 1) Console.WriteLine(PrintMap(realMap));

            mapHashes = new() { Hash(inputMap) };
            depth = 3;

            if (debug == 2)
            {
                int current = 0;
                List<(int nei, int weight)> next;
                do
                {
                    next = Next(current);
                    if (next.Count == 1)
                    {
                        Console.WriteLine("Auto 0");
                        current = next[0].nei;
                    }
                    else
                    {
                        Console.Write("Next: ");
                        current = next[int.Parse(Console.ReadLine())].nei;
                    }
                } while (next.Count != 0);
            }

            var result = Dijkstras(Next, i => mapHashes[i] == 194194);
            int index = mapHashes.IndexOf(194194);
            part1 = result[index].distance;

            if (debug == 1)
            {
                string print = "";
                do
                {
                    print = PrintMap(Restore(mapHashes[index])) + "\n" + print;
                    index = result[index].prev;
                } while (index != 0);
                Console.WriteLine(print);
            }

            mapHashes = new() { Hash(realMap) };
            depth = 5;
            result = Dijkstras(Next, i => mapHashes[i] == 194194194194);
            index = mapHashes.IndexOf(194194194194);
            part2 = result[index].distance;

            if (debug == 1)
            {
                string print = "";
                do
                {
                    print = PrintMap(Restore(mapHashes[index])) + "\n" + print;
                    index = result[index].prev;
                } while (index != 0);
                Console.WriteLine(print);
            }
        }

        string PrintMap(int[,] map)
            => GridStr(map, i => i == -1 ? " " : i == 0 ? "." : char.ConvertFromUtf32('A' + i - 1))[..^1];
    }
}