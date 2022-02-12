using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            Type[] assembly = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Dictionary<int, AoCDay> AoC = new();
            for (int i = 0; assembly[i].Name != nameof(AoCDay); i++)
            {
                if (!assembly[i].IsSubclassOf(typeof(AoCDay)))
                    continue;
                var AoCDay = (AoCDay)Activator.CreateInstance(assembly[i]);
                AoC.Add(AoCDay.day, AoCDay);
            }

            int day = 24;
            string msg = "Day {0} is not done.";
            Console.WriteLine("Run 2021 Day: " + day/* + " <press Enter>"*/);
            do
            {
                if (!AoC.ContainsKey(day))
                    if (1 <= day && day <= 25)
                        Console.WriteLine(msg, day);
                    else break;
                else
                {
                    Stopwatch watch = Stopwatch.StartNew();
                    AoC[day].Run();
                    watch.Stop();
                    Console.WriteLine("Time: {0} ms", watch.ElapsedMilliseconds);
                }
                Console.WriteLine(new string('-', msg.Length));
                Console.Write("Run 2021 Day: ");
            } while (int.TryParse(Console.ReadLine(), out day));
        }
    }
}
