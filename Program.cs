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
            Dictionary<int, MethodInfo> AoC = new();
            for (int i = 0; assembly[i].Name != "Program"; i++)
                AoC.Add(i + 1, assembly[i].GetMethod("Run"));

            AoC[AoC.Count].Invoke(AoC[AoC.Count], null);
            Console.WriteLine();
            while (true)
            {
                Console.Write("Run day: ");
                if (int.TryParse(Console.ReadLine(), out int day) &&
                    AoC.ContainsKey(day))
                {
                    AoC[day].Invoke(AoC[day], null);
                    Console.WriteLine();
                }
                else break;
            }
        }
    }
}
