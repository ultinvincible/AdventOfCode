using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _22_Reactor : AoCDay
    {
        List<(int, int)[]> Divide_Except
            ((int min, int max)[] cuboid, (int min, int max)[] except)
        {
            for (int axis = 0; axis < 3; axis++)
                if (except[axis].max < cuboid[axis].min ||
                    cuboid[axis].max < except[axis].min)
                    return new() { cuboid };

            List<(int, int)[]> result = new();
            for (int axis = 0; axis < 3; axis++)
                foreach (var (check, minSide) in new (bool, bool)[]{
                (cuboid[axis].min < except[axis].min, true),
                (except[axis].max < cuboid[axis].max, false)})
                    if (check)
                    {
                        (int min, int max)[] div = Array.ConvertAll(cuboid, _ => _);
                        for (int a = 0; a < axis; a++)
                        {
                            div[a] = (Math.Max(div[a].min, except[a].min),
                                      Math.Min(div[a].max, except[a].max));
                        }
                        if (minSide)
                            div[axis].max = except[axis].min - 1;
                        else div[axis].min = except[axis].max + 1;
                        result.Add(div);
                    }
            return result;
        }
        public override void Run()
        {
            //debug = true;
            HashSet<(int x, int y, int z)> initial = new();
            List<(int, int)[]> cuboids = new();
            foreach (string line in inputLines)
            {
                string[] split = line.Split(' '),
                         split1 = split[1].Split(',');
                bool state = true;
                if (split[0][1] == 'f') state = false;
                (int min, int max)[] coords = new (int, int)[3];
                for (int i = 0; i < 3; i++)
                {
                    int[] axis = Array.ConvertAll
                        (split1[i].Split('=')[1].Split(".."), s => int.Parse(s));
                    coords[i] = (Math.Min(axis[0], axis[1]),
                                 Math.Max(axis[0], axis[1]));
                }

                if (Math.Abs(coords[0].min) <= 50)
                {
                    Every_Points(ref initial, coords, state);
                }

                List<(int, int)[]> newCuboids = new();
                foreach ((int, int)[] cub in cuboids)
                {
                    var divide = Divide_Except(cub, coords);
                    newCuboids.AddRange(divide);

                    if (debug)
                    {
                        if (divide.Any(div => Volume(div) < 1))
                            throw new Exception("Negative volume.");
                        Console.WriteLine("Except:" + Environment.NewLine + PrintCuboid(coords) + state);
                        Console.WriteLine("Old:" + Environment.NewLine + PrintCuboid(cub));
                        if (divide.Count != 0)
                        {
                            Console.WriteLine("New:");
                            foreach (var div in divide)
                                Console.WriteLine(PrintCuboid(div));
                        }
                        HashSet<(int x, int y, int z)> divPoints = new();
                        Every_Points(ref divPoints, cub, true);
                        Every_Points(ref divPoints, coords, false);
                        long volume = divide.Sum(c => Volume(c));
                        Console.WriteLine("Total:      " + volume);
                        Console.WriteLine("Inaccuracy: " + (volume - divPoints.Count));
                        Console.WriteLine(new string('-', 40));
                    }
                }
                if (state)
                    newCuboids.Add(coords);
                cuboids = newCuboids;

                if (debug)
                {
                    long volume = cuboids.Sum(c => Volume(c));
                    Console.WriteLine("Total:      " + volume);
                    Console.WriteLine("Inaccuracy: " + (volume - initial.Count));
                    Console.WriteLine(new string('~', 40));
                }
            }
            (part1, part2) = (initial.Count, cuboids.Sum(c => Volume(c)));
        }

        static void Every_Points(ref HashSet<(int x, int y, int z)> points,
            (int min, int max)[] coords, bool state)
        {
            for (int x = coords[0].min; x <= coords[0].max; x++)
                for (int y = coords[1].min; y <= coords[1].max; y++)
                    for (int z = coords[2].min; z <= coords[2].max; z++)
                        if (state)
                            points.Add((x, y, z));
                        else
                            points.Remove((x, y, z));
        }
        static long Volume((int min, int max)[] cuboid)
        {
            long volume = 1;
            foreach (var (min, max) in cuboid)
                volume *= max - min + 1;
            return volume;
        }
        static string PrintCuboid((int, int)[] cuboid)
        {
            string result = "";
            foreach (var (min, max) in cuboid)
            {
                result += "  |" + min.ToString().PadLeft(3)
                    + "|.(" + (max - min + 1).ToString().PadLeft(2)
                    + ").|" + max.ToString().PadLeft(3)
                    + "|" + Environment.NewLine;
            }
            result += "Volume: " + Volume(cuboid) + Environment.NewLine;
            return result;
        }
    }
}

//class CuboidExcept // rabbit hole to recursive madness
//{
//    public (int min, int max)[] cub = new (int min, int max)[3];
//    public (int min, int max) this[int i]
//    { get => cub[i]; set => cub[i] = value; }
//    public List<(int min, int max)[]> except = new();
//    public bool Except(CuboidExcept e, out CuboidExcept result)
//    {

//    }
//    public bool Intersect(CuboidExcept other, out CuboidExcept result)
//        => Intersect(this, other, out result);
//    public static bool Intersect(CuboidExcept c1, CuboidExcept c2, out CuboidExcept result)
//    {
//        result = new();
//        for (int c = 0; c < 3; c++)
//        {
//            result[c] = (Math.Max(c1[c].min, c2[c].min),
//                         Math.Min(c1[c].max, c2[c].max));
//            if (result[c].min > result[c].max)
//                return false;
//        }
//        foreach (var except in c1.except.Concat(c2.except))
//            if(Intersect(result,except,out var excInter))
//        {

//        }
//        return true;
//    }
//    public int Volume()
//    {
//        int volume = Volume(cub);
//        foreach (var except in except)
//            volume -= Volume(except);
//        return volume;
//    }
//}