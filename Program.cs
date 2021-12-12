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
            Dictionary<int, Type> AoC = new();
            for (int i = 0; assembly[i].Name != "Program"; i++)
                AoC.Add(i + 1, assembly[i]);

            AoC[AoC.Count].GetMethod("Run").Invoke(AoC[AoC.Count], null);
            Console.WriteLine();
            while (true)
            {
                Console.Write("Run day: ");
                if (int.TryParse(Console.ReadLine(), out int day) &&
                    AoC.ContainsKey(day))
                {
                    AoC[day].GetMethod("Run").Invoke(AoC[day], null);
                    Console.WriteLine();
                }
                else break;
            }
        }
    }
}
