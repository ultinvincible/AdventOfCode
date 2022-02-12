using System;
using System.Collections.Generic;

namespace Advent_of_Code
{
    class ALU : AoCDay
    {
        public ALU() : base(24) { }


        const int digitsCount = 14, linesEach = 18;
        public override void Run()
        {
            int NumberAt(int digit, int line)
                => int.Parse(input[digit * linesEach + line].Split(' ')[2]);
            int[] minZMod26 = new int[digitsCount], xParam = new int[digitsCount];
            //Console.WriteLine("Digit\tZ % 26 range");
            for (int d = 0; d < digitsCount; d++)
            {
                xParam[d] = NumberAt(d, 5);
                minZMod26[d] = NumberAt(d, 15) + 1;
                //Console.WriteLine(d + "\t" +
                //    minZMod26[d].ToString().PadLeft(2) + ".." + (minZMod26[d] + 8));
            }

            int[] max = new int[digitsCount], min = new int[digitsCount];
            Stack<int> digits = new();
            for (int i = 0; i < digitsCount; i++)
            {
                if (xParam[i] <= 9)
                {
                    // Assumption: xParam[d] <= 9 -> z div 26; else x == 1
                    int p = digits.Pop(), x = xParam[i] + minZMod26[p];
                    max[i] = Math.Min(9, x + 8);
                    max[p] = Math.Min(9, 10 - x); //1 - x + 9
                    min[i] = Math.Max(1, x);
                    min[p] = Math.Max(1, 2 - x); // 1 - x + 1
                }
                else digits.Push(i);
            }
            Console.WriteLine(CollStr(max));
            Console.WriteLine(CollStr(min));

            //// raw brute force 14^9 numbers
            //Console.WriteLine();
            //List<(List<int>, int[])> inputs =
            //    new() { (new(), new int[3]) }; // digits and x, y, z
            //for (int i = 0; i < input.Length; i++)
            //{
            //    string[] split = input[i].Split(' ');
            //    int var = split[1][0] - 'x';
            //    if (split[0] == "inp")
            //    {
            //        List<(List<int>, int[])> copy = new(inputs);
            //        inputs.Clear();
            //        foreach (var (model, xyz) in copy)
            //        {
            //            for (int d = 9; d >= 1; d--)
            //            {
            //                List<int> nextDigit = new(model);
            //                nextDigit.Add(d);
            //                inputs.Add((nextDigit, Array.ConvertAll(xyz, _ => _)));
            //            }

            //            foreach (int d in model)
            //                Console.Write(d);
            //            Console.Write("\t" + xyz[0] + "|" + xyz[2]);
            //            if (xyz[0] == 1)
            //                Console.Write(" = " + Math.DivRem(xyz[2], 26, out int r) + " * 26 + " + r);
            //            Console.WriteLine();
            //        }
            //        Console.WriteLine();
            //    }
            //    else
            //        foreach (var (model, xyz) in inputs)
            //        {
            //            if (!int.TryParse(split[2], out int value))
            //                if (split[2][0] == 'w')
            //                    value = model[^1];
            //                else
            //                    value = xyz[split[2][0] - 'x'];
            //            switch (split[0])
            //            {
            //                case "add":
            //                    xyz[var] += value;
            //                    break;
            //                case "mul":
            //                    xyz[var] *= value;
            //                    break;
            //                case "div":
            //                    var div = (decimal)xyz[var] / value;
            //                    if (div >= 0)
            //                        xyz[var] = (int)Math.Floor(div);
            //                    xyz[var] = (int)Math.Ceiling(div);
            //                    break;
            //                case "mod":
            //                    xyz[var] %= value;
            //                    break;
            //                case "eql":
            //                    xyz[var] = Convert.ToInt32(xyz[var] == value);
            //                    break;
            //            }
            //        }
            //}
        }
    }
}
