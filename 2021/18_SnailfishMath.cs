using System;
using System.Collections.Generic;

namespace Advent_of_Code._2021
{
    class _18_SnailfishMath : AoCDay
    {
        class Regular
        {
            public int number, nested;
            public Regular(int num, int nes)
            {
                number = num;
                nested = nes;
            }
            public override string ToString()
                => number.ToString().PadLeft(2) + "|" + nested;
        }
        List<Regular> Interpret(string line)
        {
            List<Regular> result = new();
            int nested = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '[')
                    nested++;
                else if (line[i] == ']')
                    nested--;
                else if (line[i] != ',')
                    result.Add(new((int)char.GetNumericValue(line[i]), nested));
            }
            return result;
        }
        List<Regular> Add_Reduce(List<Regular> sum, string num)
        {
            sum.AddRange(Interpret(num));
            for (int i = 0; i < sum.Count; i++)
            {
                sum[i].nested++;
                CheckExplode(ref sum, i);
            }
            for (int i = 0; i < sum.Count; i++)
                if (CheckSplit(ref sum, i))
                    i = 0;
            return sum;
        }
        List<Regular> Add_Reduce(string num1, string num2)
                    => Add_Reduce(Interpret(num1), num2);
        bool CheckExplode(ref List<Regular> sum, int i) // i is pair left
        {
            if (sum[i].nested >= 5/* && sum[i].left*/)
            {
                if (i > 0)
                    sum[i - 1].number += sum[i].number;
                if (i < sum.Count - 2)
                    sum[i + 2].number += sum[i + 1].number;

                sum.Insert(i + 2, new(0, sum[i].nested - 1));
                sum.RemoveAt(i);
                sum.RemoveAt(i);
                //Print(sum, i);
                return true;
            }
            return false;
        }
        bool CheckSplit(ref List<Regular> sum, int i)
        {
            if (sum[i].number >= 10)
            {
                sum.Insert(i + 1, new((int)Math.Floor((double)sum[i].number / 2), sum[i].nested + 1));
                sum.Insert(i + 2, new((int)Math.Ceiling((double)sum[i].number / 2), sum[i].nested + 1));
                sum.RemoveAt(i);
                //Print(sum, i);
                if (CheckExplode(ref sum, i) && i > 0)
                    CheckSplit(ref sum, i - 1);
                return true;
            }
            return false;
        }

        protected override void Run()
        {
            List<Regular> sum = Interpret(inputLines[0]);
            foreach (string line in inputLines[1..])
                sum = Add_Reduce(sum, line);
            part1 = Magnitude(sum);

            int max = 0;
            for (int i = 0; i < inputLines.Length; i++)
                for (int j = 0; j < inputLines.Length; j++)
                    if (i != j)
                        max = Math.Max(max, Magnitude(Add_Reduce(inputLines[i], inputLines[j])));
            part2 = max;
        }
        int Magnitude(List<Regular> sum)
        {
            int maxNested = 0;
            foreach (Regular reg in sum)
                maxNested = Math.Max(maxNested, reg.nested);
            for (; maxNested > 0; maxNested--)
                for (int i = 0; i < sum.Count; i++)
                    if (sum[i].nested == maxNested)
                    {
                        int magnitude = 3 * sum[i].number + 2 * sum[i + 1].number;
                        sum.RemoveAt(i);
                        sum.RemoveAt(i);
                        sum.Insert(i, new(magnitude, maxNested - 1));
                    }
            return sum[0].number;
        }
        //void Print(List<Regular> sum, int i)
        //{
        //    Console.Clear();
        //    for (int j = 0; j < sum.Count; j++)
        //        Console.Write(j.ToString().PadLeft(2) + "|");
        //    Console.WriteLine();
        //    foreach (Regular reg in sum)
        //        Console.Write(reg.number.ToString().PadLeft(2) + "|");
        //    Console.WriteLine();
        //    foreach (Regular reg in sum)
        //        Console.Write(reg.nested.ToString().PadLeft(2) + "|");
        //    Console.WriteLine(Environment.NewLine + i);
        //}
    }
}
