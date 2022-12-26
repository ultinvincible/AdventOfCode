using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _21_MonkeyYells : AoCDay
    {
        string[] name, yell;
        protected override void Run()
        {
            name = new string[inputLines.Length];
            yell = new string[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                string[] split = line.Split(": ");
                name[i] = split[0];
                yell[i] = split[1];
            }

            int rootI = Array.IndexOf(name, "root");
            part1 = Evaluate(rootI);

            if (debug)
            {
                List<string> current = new() { "humn" }, found;
                while (true)
                {
                    found = new();
                    foreach (string c in current)
                    {
                        for (int i = 0; i < yell.Length; i++)
                            if (yell[i].Contains(c))
                            {
                                Console.WriteLine(inputLines[i]);
                                found.Add(name[i]);
                                break;
                            }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    if (found.Count == 0) break;
                    current = found;
                }
            }

            EvaluateP2(rootI);
        }
        long Evaluate(int current)
        {
            long result;
            if (yell[current].Length < 11) result = int.Parse(yell[current]);
            else
            {
                int mk1 = Array.IndexOf(name, yell[current][..4]),
                    mk2 = Array.IndexOf(name, yell[current][7..]);
                result = yell[current][5] switch
                {
                    '+' => Evaluate(mk1) + Evaluate(mk2),
                    '-' => Evaluate(mk1) - Evaluate(mk2),
                    '*' => Evaluate(mk1) * Evaluate(mk2),
                    '/' => Evaluate(mk1) / Evaluate(mk2),
                    _ => throw new Exception("Wot"),
                };
            }
            if (debug) Console.WriteLine($"{current,4}: {result}");
            return result;
        }

        List<string> EvaluateP2(int current)
        {
            if (name[current] == "humn") return new() { "humn" };
            if (yell[current].Length < 11) return new() { yell[current] };
            List<string> result,
                eval1 = EvaluateP2(Array.IndexOf(name, yell[current][..4])),
                eval2 = EvaluateP2(Array.IndexOf(name, yell[current][7..]));
            if (name[current] != "root")
            {
                string op = yell[current][5].ToString();
                if (eval1[0] == "humn")
                {
                    eval1.Add(op);
                    eval1.AddRange(eval2);
                    result = eval1;
                }
                else if (eval2[0] == "humn")
                {
                    eval2.Add(op == "-" ? "--" : op == "/" ? "//" : op);
                    eval2.AddRange(eval1);
                    result = eval2;
                }
                else result = new() { Evaluate(current).ToString() };
                return result;
            }
            else
            {
                if (debug) Console.WriteLine(
                        string.Join("|", eval1) + "|==|" + string.Join("|", eval2));

                if (eval1.Count == 1) (eval1, eval2) = (eval2, eval1); // safety
                part2 = long.Parse(eval2[0]);
                for (int i = eval1.Count - 2; i > 0; i -= 2)
                {
                    string op = eval1[i];
                    long value = long.Parse(eval1[i + 1]);
                    part2 = op switch
                    {
                        "+" => part2 - value,
                        "-" => part2 + value,
                        "--" => value - part2,
                        "*" => part2 / value,
                        "/" => part2 * value,
                        "//" => value / part2,
                        _ => throw new Exception("Wot")
                    };
                }
                return new() { "humn", "==", part2.ToString() };
            }
        }
    }
}
