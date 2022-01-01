using System;
using System.Collections.Generic;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] assembly = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Dictionary<int, AoCDay> AoC = new();
            int day = 0;
            for (; assembly[day].Name != nameof(AoCDay); day++)
            {
                var AoCDay = (AoCDay)Activator.CreateInstance(assembly[day]);
                AoC.Add(AoCDay.day, AoCDay);
            }

            Console.WriteLine("Run 2021 Day: " + day/* + " <press Enter>"*/);
            do
            {
                AoC[day].Run();
                Console.Write(new string('-', 16) + "\nRun 2021 Day: ");
            } while (int.TryParse(Console.ReadLine(), out day) &&
                    AoC.ContainsKey(day));
        }
    }
}
