using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2020
{
    class _18_DumbMaths : AoCDay
    {
        void Evaluate(ref List<string> calc, int start = 0, bool advanced = false)
        {
            int end = calc.Count;
            for (int i = start; i < calc.Count; i++)
                if (calc[i] == "(")
                {
                    Evaluate(ref calc, i + 1, advanced);
                    calc.RemoveAt(i);
                }
                else if (calc[i] == ")")
                {
                    calc.RemoveAt(i);
                    end = i;
                    break;
                }
            long value = long.Parse(calc[start]);
            end = Math.Min(end, calc.Count);
            if (!advanced)
            {
                for (int i = start; i < end; i++)
                {
                    if (calc[i] == "+")
                        value += long.Parse(calc[++i]);
                    else if (calc[i] == "*")
                        value *= long.Parse(calc[++i]);
                }
            }
            else
            {
                for (int i = start; i < end; i++)
                    if (calc[i] == "+")
                    {
                        calc[i - 1] = (long.Parse(calc[i - 1]) + long.Parse(calc[i + 1])).ToString();
                        calc.RemoveAt(i);
                        calc.RemoveAt(i);
                        end -= 2;
                        i--;
                    }
                value = 1;
                for (int i = start; i < end; i++)
                    if (long.TryParse(calc[i], out long num))
                        value *= num;
            }
            calc.RemoveRange(start, end - start);
            calc.Insert(start, value.ToString());
            if (debug == 1) Console.Write(PrintCalc(calc));
        }
        string PrintCalc(List<string> calc)
        {
            string print = "";
            foreach (string s in calc)
                print += s;
            print += "\n";
            return print;
        }
        protected override void Run()
        {
            foreach (string line in inputLines)
            {
                List<string> calculation = new();
                foreach (string s in line.Split(' '))
                    if (s[0] == '(' || s[^1] == ')')
                        foreach (char c in s)
                            calculation.Add(c.ToString());
                    else calculation.Add(s);
                if (debug == 1) Console.Write(PrintCalc(calculation));
                List<string> calculation2 = new(calculation);
                Evaluate(ref calculation);
                part1 += long.Parse(calculation[0]);
                Evaluate(ref calculation2, advanced: true);
                part2 += long.Parse(calculation2[0]);
            }
        }
    }
}
