using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Advent_of_Code._2020
{
    class _04_Passports : AoCDay
    {
        readonly string[] eyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        protected override void Run(out (object part1, object part2) answer)
        {
            string[] passports = input.Split("\n\n");
            Dictionary<string, Predicate<string>> valid = new()
            {
                { "byr", s => int.TryParse(s, out int i) && i >= 1920 && i <= 2002 },
                { "iyr", s => int.TryParse(s, out int i) && i >= 2010 && i <= 2020 },
                { "eyr", s => int.TryParse(s, out int i) && i >= 2020 && i <= 2030 },
                {
                    "hgt",
                    s => int.TryParse(s[0..^2], out int i) && (
                    (s.Substring(s.Length - 2, 2) == "cm" && i >= 150 && i <= 193) ||
                    (s.Substring(s.Length - 2, 2) == "in" && i >= 59 && i <= 76))
                },
                { "hcl", s => s.Length == 7 && Regex.IsMatch(s, @"#\b[0-9a-f]+\b\Z") },
                { "ecl", s => Array.IndexOf(eyeColors, s) >= 0 },
                { "pid", s => s.Length == 9 && int.TryParse(s, out int i) }
            };

            int presentCount = 0, validCount = 0;
            foreach (string p in passports)
            {
                int present = 0; bool allValid = true;
                string[] split = p.Split(new char[] { ':', ' ', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < split.Length; i += 2)
                {
                    if (valid.ContainsKey(split[i]))
                    {
                        present++;
                        allValid &= valid[split[i]](split[i + 1]);
                    }
                }
                if (present == 7)
                {
                    presentCount++;
                    if (allValid) validCount++;
                }
            }
            answer = (presentCount, validCount);
        }
    }
}
