using System;
using System.Reflection;
using System.IO;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            const int yearMax = 2021;
            int year = 2021, day = 23;
            Directory.SetCurrentDirectory("../../../");
            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Type[,] Solutions = new Type[yearMax - 2014, 26];
            for (int i = 0; i < assembly.Length; i++)
                if (assembly[i].IsSubclassOf(typeof(AoCDay)))
                    Solutions[int.Parse(assembly[i].Namespace[^4..]) - 2015,
                        int.Parse(assembly[i].Name[1..3])] = assembly[i];

            Console.WriteLine("Run {0} day: {1}", year, day);
            while (true)
            {
                RunSolution(year, day);

                while (true)
                {
                    Console.Write("Run {0} day: ", year);
                    string read = Console.ReadLine();
                    if (read == "all")
                    {
                        for (int d = 1; d <= 25; d++)
                        {
                            Console.WriteLine("Day " + d);
                            RunSolution(year, d);
                        }
                        continue;
                    }
                    else if (int.TryParse(read, out day) && day >= 1 && day <= 25)
                        break;
                    else
                    {
                        Console.Write("Change year to: ");
                        if (!int.TryParse(Console.ReadLine(), out year)
                            || year < 2015 || year > yearMax)
                            return;
                    }
                }
            }

            void RunSolution(int year, int day)
            {
                Type type = Solutions[year - 2015, day];
                if (type is null)
                    Console.WriteLine("{0} day {1} is not done.", year, day);
                else
                {
                    AoCDay solution = (AoCDay)Activator.CreateInstance(type);
                    solution.Run(year + "/Inputs/" + day.ToString("00") + ".txt", day == 25);
                }
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}
