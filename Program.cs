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
            int yearMax = 2021, year = 2020, day = 11;

            Directory.SetCurrentDirectory("../../../");
            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Type[,] Solution = new Type[yearMax - 2014, 26];
            for (int i = 0; i < assembly.Length; i++)
            {
                if (!assembly[i].IsSubclassOf(typeof(AoCDay)))
                    continue;
                Solution[int.Parse(assembly[i].Namespace[^4..]) - 2015,
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
                    if (!int.TryParse(read, out day) || day < 1 || day > 25)
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
                    else break;
                }
            }

            void RunSolution(int year, int day)
            {
                string msg = "{0} day {1} is not done.";
                Type type = Solution[year - 2015, day];
                if (type is null)
                    Console.WriteLine(msg, year, day);
                else
                {
                    AoCDay solution = (AoCDay)Activator.CreateInstance(type);
                    string input = File.ReadAllText(year + "/Inputs/" + day.ToString("00") + ".txt");
                    Stopwatch watch = Stopwatch.StartNew();
                    var (Part1, Part2) = solution.Solve(input);
                    watch.Stop();
                    Console.WriteLine(Part1.ToString());
                    if (day != 25) Console.WriteLine(Part2.ToString());
                    Console.WriteLine("Time: {0}s", (decimal)watch.ElapsedMilliseconds / 1000);
                }
                Console.WriteLine(new string('-', msg.Length));
            }
        }
    }
}
