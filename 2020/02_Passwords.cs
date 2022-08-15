using System;

namespace Advent_of_Code._2020
{
    class _02_Passwords : AoCDay
    {
        public override void Run()
        {
            int valid1 = 0, valid2 = 0;
            foreach (string entry in inputLines)
            {
                string[] split = entry.Split(':'), policy = split[0]
                    .Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int min = int.Parse(policy[0]), max = int.Parse(policy[1]);
                char policyChar = policy[2][0];
                string password = split[1].Trim();

                int appear = 0;
                foreach (char c in password)
                {
                    if (c == policyChar) appear++;
                }
                if (appear >= min && appear <= max) valid1++;

                if (password[min - 1] == policyChar ^ password[max - 1] == policyChar)
                    valid2++;

                //Console.WriteLine(chars[0] + ";" + appear + ";" + chars[1] + " ");
                //Console.WriteLine(details);
                //Console.WriteLine(password);
            }
            (part1,part2) = (valid1, valid2);
        }
    }
}
