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
            int day = 1;
            for (int i = 0; assembly[i].Name != nameof(AoCDay); i++)
            {
                var AoCDay = (AoCDay)Activator.CreateInstance(assembly[i]);
                AoC.Add(AoCDay.day, AoCDay);
                day = AoCDay.day;
            }

            Console.WriteLine("Run 2021 Day: " + day/* + " <press Enter>"*/);
            do
            {
                AoC[day].Run();
                Console.Write(new string('\u2582', 16) + "\nRun 2021 Day: ");
            } while (int.TryParse(Console.ReadLine(), out day) &&
                    AoC.ContainsKey(day));
        }
    }
}
