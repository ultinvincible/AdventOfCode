using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            const int yearMax = 2021;
            int year = 2019, day = 1;
            Directory.SetCurrentDirectory("../../../");
            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Type[,] Solutions = new Type[yearMax - 2014, 26];
            for (int i = 0; i < assembly.Length; i++)
            {
                if (!assembly[i].IsSubclassOf(typeof(AoCDay)))
                    continue;
                Solutions[int.Parse(assembly[i].Namespace[^4..]) - 2015,
                    int.Parse(assembly[i].Name[1..3])] = assembly[i];
            }

            Console.WriteLine("Run {0} day: {1}", year, day);
            while (true)
            {
                RunSolution(year, day);

                while (true)
                {
                    Console.Write("Run {0} day: ", year);
                    string read = Console.ReadLine();
                    if (int.TryParse(read, out day) && day >= 1 && day <= 25)
                        break;
                    else
                    {
                        if (read == "all")
                        {
                            for (int d = 1; d <= 25; d++)
                            {
                                Console.WriteLine("Day " + d);
                                RunSolution(year, d);
                            }
                            continue;
                        }
                        Console.Write("Change year to: ");
                        if (!int.TryParse(Console.ReadLine(), out year)
                            || year < 2015 || year > yearMax)
                            return;
                    }
                }
            }

            void RunSolution(int year, int day)
            {
                string msg = "{0} day {1} is not done.";
                Type type = Solutions[year - 2015, day];
                if (type is null)
                    Console.WriteLine(msg, year, day);
                else
                {
                    AoCDay solution = (AoCDay)Activator.CreateInstance(type);
                    solution.input = File.ReadAllText(year + "/Inputs/" + day.ToString("00") + ".txt");
                    solution.inputLines = File.ReadAllLines(year + "/Inputs/" + day.ToString("00") + ".txt");
                    Stopwatch watch = Stopwatch.StartNew();
                    solution.Run();
                    watch.Stop();

                    if (solution.part1 != 0) Console.WriteLine(solution.part1);
                    else Console.WriteLine(solution.part1_str);
                    if (day != 25)
                    {
                        if (solution.part2 != 0) Console.WriteLine(solution.part2);
                        else Console.WriteLine(solution.part2_str);
                    }
                    Console.WriteLine("Time: {0} ms", (decimal)watch.ElapsedMilliseconds);
                }
                Console.WriteLine(new string('-', msg.Length));
            }
        }
    }
}
