using System;
using System.Collections.Generic;
using System.Reflection;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] days = Assembly.GetExecutingAssembly().GetTypes();
            Dictionary<int, Type> AoC2021 = new();
            for (int i = 0; days[i].Name != "Program"; i++)
                AoC2021.Add(i + 1, days[i]);

            while (true)
            {
                Console.Write("Run day: ");
                if (!int.TryParse(Console.ReadLine(), out int day)
                    || day < 1 || day > AoC2021.Count)
                    break;
                AoC2021[day].GetMethod("Run").Invoke(AoC2021[day], null);
                Console.WriteLine();
            }
        }
    }
}
