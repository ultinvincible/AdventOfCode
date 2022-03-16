using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class AmphipodRooms : AoCDay
    {
        public AmphipodRooms() : base(23) { }

        const int length = 11, max_room = 8, max_amp = 1000, max_depth = 2;
        static readonly int[] HallwayIndices = new int[] { 0, 1, 3, 5, 7, 9, 10 },
                     RoomIndices = new int[] { 2, 4, 6, 8 };
        static readonly bool[] isRoom = new bool[11];

        class Map
        {
            public int[] Hallway;
            public (Stack<int> Amps, bool CorrectOrEmpty)[] Rooms;
            public static readonly int[] Correct = new int[] { 1, 10, 100, 1000 };

            public Map(int[] hallway, (Stack<int>, bool)[] rooms)
            {
                Hallway = hallway;
                Rooms = rooms;
            }

            Stack<int> RoomAmps(int pos) => Rooms[pos / 2 - 1].Amps;
            public int this[int pos]
            {
                get
                {
                    if (isRoom[pos])
                        return RoomAmps(pos).Peek();
                    return Hallway[pos];
                }
                set
                {
                    if (isRoom[pos])
                    {
                        RoomAmps(pos).Pop();
                        if (value != 0)
                            RoomAmps(pos).Push(value);
                    }
                    else Hallway[pos] = value;
                }
            }

            public (Map map, int energy) Moved(int from_pos, int to_pos)
            {
                int energy = Energy(from_pos, to_pos);
                Map newMap = NewMap(from_pos, to_pos);
                return (newMap, energy);
            }
            Map NewMap(int from_pos, int to_pos)
            {
                Map newMap = new(Array.ConvertAll(Hallway, _ => _),
                    Array.ConvertAll(Rooms,
                    r => (new Stack<int>(r.Amps), r.CorrectOrEmpty)));
                newMap[to_pos] = this[from_pos];
                newMap[from_pos] = 0;
                return newMap;
            }
            int Energy(int from_pos, int to_pos)
            {
                int energy = Math.Abs(from_pos - to_pos);
                if (isRoom[from_pos])
                    energy += max_depth - RoomAmps(from_pos).Count + 1;
                if (isRoom[to_pos])
                    energy += max_depth - RoomAmps(to_pos).Count;
                return energy;
            }

            //public override string ToString()
            //{
            //    return PrintMap(map);
            //}

            public List<(Map map, int energy)> Next()
            {
                List<(int from, int to)> moves = new(Moves);
                foreach (int hw in Hallway)
                    if (this[hw] != 0)
                        moves.RemoveAll(m => m.to == hw
                        || (m.from < hw && hw < m.to));
                foreach (int to_room in RoomIndices)
                {
                    var (room, canEnter) = Rooms[to_room];
                    if (!canEnter) moves.RemoveAll(m => m.to == to_room);
                }
                return moves;
            }
        }

        (int, int)[] RoomPoints = new (int, int)[8];
        static List<(int, int)> Moves = new();
        public override void Run()
        {
            debug = false;

            for (int from = 0; from <= 11; from++)
                for (int to = 0; to <= 11; to++)
                    if (from != to && (isRoom[from] || isRoom[to]))
                        Moves.Add((from, to));

            int[][] start = new int[input[1].Length - 2][];
            for (int c = 3; c < input[0].Length - 3; c += 2)
            {
                start[c - 1] = new int[3];
                for (int line = 2; line <= input.Length - 2; line++)
                    start[c - 1][line - 1] =
                        (int)Math.Pow(10, input[line][c] - 'A');
                isRoom[c - 1] = true;
            }
            for (int r = 0; r < 11; r++)
                if (isRoom[r])
                {
                    for (int d = 1; d <= max_depth; d++)
                        RoomPoints[r + d - 3] = (r, d);
                }
                else start[r] = new int[1];

            List<(int prevFrom, int prevTo, int corrects)>[,] Paths =
            List < (int, int, int, int) > Paths = new();
            foreach (var (r, d) in RoomPoints)
                if (d == 1)
                    foreach (var h in HallwayIndices)
                    {
                        Paths.Add((r, d, h, 0));
                    }

            List<(int, int, int, int, int prevI)> newPaths = new();
            foreach (var (r, d, h, _) in Paths)
            {
                foreach (int newH in HallwayIndices)
                {
                    if ()
                }
                newPaths.Add(r, d + 1)
                for (int i = r + 1


                    if (i < r) continue;
                    else if (i == r)
                        }



            return;

            int[][] dest = Array.ConvertAll(start, a => Array.ConvertAll(a, _ => _));
            for (int room = 2; room <= max_room; room += 2)
                for (int depth = 1; depth <= 2; depth++)
                    dest[room][depth] = (int)Math.Pow(10, (room / 2) - 1);
            Map startMap = new(start), destMap = new(dest);

            //List<(Map key, int distance, int prev)> result =
            //    A_Star(startMap, destMap, Moves, (m1, m2) => m1.Equals(m2), IncorrectAmps);
            //Console.WriteLine(result[^1].distance);
            //var current = result[^1];
            //while (!current.key.Equals(startMap))
            //{
            //    Console.WriteLine(current.key.ToString() + current.distance);
            //    current = result[current.prev];
            //}

            //int min = int.MaxValue;
            //Queue<(Map, int)> bfsQ = new();
            //bfsQ.Enqueue((startMap, 0));
            //while (bfsQ.Count != 0)
            //{
            //    var (map, energy) = bfsQ.Dequeue();
            //    if (map.Equals(destMap))
            //    {
            //        min = Math.Min(min, energy);
            //        Console.WriteLine(energy);
            //        continue;
            //    }
            //    foreach (var (newMap, newEnergy) in Moves(map))
            //    {
            //        bfsQ.Enqueue((newMap, energy + newEnergy));
            //    }
            //}
        }

        static string AmpStr(int i)
        {
            if (i == 0)
                return ".";
            return char.ConvertFromUtf32('A' + (int)Math.Log10(i));
        }
        static string PrintMap(int[][] map)
        {
            string result = "";
            for (int i = 0; i < map.Length; i++)
                if (Array.IndexOf(RoomIndices, i) != -1)
                    result += " ";
                else result += AmpStr(map[i][0]);
            result += " \n";
            for (int j = 1; j <= 2; j++)
            {
                char[] line = new string('|', 11).ToCharArray();
                foreach (int i in RoomIndices)
                    line[i] = AmpStr(map[i][j])[0];
                result += new string(line) + " \n";
            }
            return result;
        }
        static string PrintMap((Map, int) state)
        {
            var (map, energy) = state;
            return PrintMap(map.map) + "Energy: " + energy + "\n";
        }
    }
}
//static bool CorrectOrEmpty(Map map, int room)
//    => CorrectOrEmpty(map.map, room);
//static bool CorrectOrEmpty(int[][] map, int room)
//{
//    bool result = true;
//    for (int depth = 1; depth <= 2; depth++)
//        if (map[room][depth] != 0 &&
//            !Correct(map[room][depth], room))
//            result = false;
//    return result;
//}
//static int MoveDist(int from_room, int from_depth, int to_room, int to_depth)
//    => from_depth + Math.Abs(from_room - to_room) + to_depth;
//static int Dists(Map map)
//{
//    int result = 0;
//    for (int r = 0; r < map.map.Length; r++)
//        for (int d = 0; d < map.map[r].Length; d++)
//        {
//            int amp = map.map[r][d];
//            if (amp != 0 && !Correct(amp, r))
//            {
//                result += amp * MoveDist(r, d, (int)Math.Log10(amp) * 2 + 2, 1);
//            }
//        }
//    for (int r = 2; r <= max_room; r += 2)
//        if (map.map[r][2] != 0 && !Correct(map.map[r][2], r))
//            result += (int)Math.Pow(10, (r / 2) - 1);
//    return result;
//}
//static int IncorrectAmps(Map map)
//{
//    int result = 0;
//    for (int r = 0; r < map.map.Length; r++)
//        for (int d = 0; d < map.map[r].Length; d++)
//            if (!Correct(map.map[r][d], r)
//                /*|| (d == 2 && !Correct(map.map[r][1], r))*/)
//                result += map.map[r][d] * 4;
//    return result;
//}