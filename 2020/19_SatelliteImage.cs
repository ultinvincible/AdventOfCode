using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _19_SatelliteImage : AoCDay
    {
        protected override void Run()
        {
            string[][] split = inputSections;
            string[] inputRules = split[0];
            string[][][] rules = new string[inputRules.Length][][];
            List<string>[] completed = new List<string>[rules.Length];
            foreach (string line in inputRules)
            {
                string[] splitLine = Array.ConvertAll(line.Split(new char[] { ':', '|' }), s => s.Trim());
                int index = int.Parse(splitLine[0]);
                if (splitLine[1][0] == '\"')
                {
                    completed[index] = new() { splitLine[1][1].ToString() };
                }
                else
                {
                    rules[index] = new string[splitLine.Length - 1][];
                    for (int i = 1; i < splitLine.Length; i++)
                    {
                        rules[index][i - 1] = splitLine[i].Split(' ');
                    }
                }
            }

            while (completed[0] is null)
            {
                for (int ruleI = 0; ruleI < rules.Length; ruleI++)
                {
                    string[][] rule = rules[ruleI];
                    if (rule is null) continue;
                    List<List<string>> newRule = new();
                    foreach (string[] subRule in rule)
                    {
                        List<string>[] translate = new List<string>[subRule.Length];
                        for (int i = 0; i < subRule.Length; i++)
                        {
                            translate[i] = new() { subRule[i] };
                            if (int.TryParse(subRule[i], out int completedI) &&
                                completed[completedI] is not null)
                                translate[i] = completed[completedI];
                        }

                        if (subRule.Length == 1)
                            foreach (string text in translate[0])
                                newRule.Add(new() { text });
                        else
                        {
                            List<List<string>> copy = new();
                            foreach (string text1 in translate[0])
                                foreach (string text2 in translate[1])
                                    newRule.Add(new() { text1, text2 });

                            if (subRule.Length == 3) // test case only
                                for (int i = 0; i < newRule.Count; i++)
                                    newRule[i].Add(translate[2][0]);
                        }
                    }

                    if (newRule.All(sr => sr.All(s => !int.TryParse(s, out _))))
                    {
                        rules[ruleI] = null;
                        completed[ruleI] = new();
                        foreach (List<string> subRule in newRule)
                        {
                            string text = string.Concat(subRule);
                            //if (!completed[ruleI].Contains(text))
                            completed[ruleI].Add(text);
                        }
                        completed[ruleI] = completed[ruleI];
                    }
                    else
                    {
                        rules[ruleI] = new string[newRule.Count][];
                        for (int i = 0; i < newRule.Count; i++)
                            rules[ruleI][i] = newRule[i].ToArray();
                    }

                    if (debug == 1 && rules[ruleI] is null && ruleI != 0)
                    {
                        Console.Write(ruleI + ": ");
                        if (rules[ruleI] is not null)
                            Console.WriteLine(GridStr(rules[ruleI], s => s + "|"));
                        else
                        {
                            Console.WriteLine(string.Join("|", completed[ruleI]) + "\nCompleted");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
            }

            foreach (string message in split[1])
            {
                if (completed[0].Contains(message))
                    part1++;
                int match42 = 0, match31 = 0;
                for (int i = 0; completed[42].Contains(message[i..(i + 8)]) && i < message.Length - 8; i += 8)
                {
                    match42++;
                }
                if (match42 <= 1) continue;
                for (int i = message.Length - 8; completed[31].Contains(message[i..(i + 8)]) && i >= 0; i -= 8)
                {
                    match31++;
                }
                if (0 < match31 && match31 < match42 && match31 + match42 == message.Length / 8)
                    part2++;
            }
        }
    }
}
