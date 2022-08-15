using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _04_Passports : AoCDay
    {
        readonly string[] eyeColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        protected override void Run()
        {
            string[] passports = input.Split(Environment.NewLine + Environment.NewLine);
            Dictionary<string, Predicate<string>> valid = new()
            {
                { "byr", s => int.TryParse(s, out int i) && i >= 1920 && i <= 2002 },
                { "iyr", s => int.TryParse(s, out int i) && i >= 2010 && i <= 2020 },
                { "eyr", s => int.TryParse(s, out int i) && i >= 2020 && i <= 2030 },
                {
                    "hgt",
                    s => int.TryParse(s[0..^2], out int year) && (
                    (s.Substring(s.Length - 2, 2) == "cm" && year >= 150 && year <= 193) ||
                    (s.Substring(s.Length - 2, 2) == "in" && year >= 59 && year <= 76))
                },
                {
                    "hcl",
                    s =>
                    {
                        if (s.Length != 7 || s[0] != '#') return false;
                        for (int i = 1; i < 7; i++)
                            if((s[i] < '0' || s[i] > '9') && (s[i] < 'a' || s[i] > 'f'))
                                return false;
                        return true;
                    }
                },
                { "ecl", s => Array.IndexOf(eyeColors, s) >= 0 },
                { "pid", s => s.Length == 9 && int.TryParse(s, out int i) }
            };

            int haveFieldsCount = 0, validCount = 0;
            foreach (string p in passports)
            {
                int fields = 0; bool allValid = true;
                string[] fieldNamesAndData = p.Split(new char[] { ':', ' ', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fieldNamesAndData.Length; i += 2)
                {
                    if (valid.ContainsKey(fieldNamesAndData[i]))
                    {
                        fields++;
                        allValid &= valid[fieldNamesAndData[i]](fieldNamesAndData[i + 1]);
                    }
                }
                if (fields == 7)
                {
                    haveFieldsCount++;
                    if (allValid) validCount++;
                }
            }
            (part1, part2) = (haveFieldsCount, validCount);
        }
    }
}
