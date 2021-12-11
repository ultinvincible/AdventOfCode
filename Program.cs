using System;
using System.Collections.Generic;
using System.Reflection;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Dictionary<int, Type> AoC2021 = new();
            for (int i = 0; assembly[i].Name != "Program"; i++)
                AoC2021.Add(i + 1, assembly[i]);

            AoC2021[AoC2021.Count].GetMethod("Run").Invoke(AoC2021[AoC2021.Count], null);
            Console.WriteLine();
            while (true)
            {
                Console.Write("Run day: ");
                if (int.TryParse(Console.ReadLine(), out int day) &&
                    AoC2021.ContainsKey(day))
                {
                    AoC2021[day].GetMethod("Run").Invoke(AoC2021[day], null);
                    Console.WriteLine();
                }
                else break;
            }
        }
    }
}
