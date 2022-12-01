using System;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace Advent_of_Code
{
    class Program
    {
        static string sessionToken = "53616c7465645f5f79da79a4597ef9c99bc08fb9368608c9fa042951abac73ab99a7b2330fcd769a77ffc2fbbc6b674b8ceab9533075b264528dfe81dbe0c317";
        static void Main(string[] args)
        {
            int year = DateTime.Now.Year,
                day = Math.Min(DateTime.Now.Day, 25);
            if (DateTime.Now.Month != 12) { year--; day = 25; }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://adventofcode.com");
            client.DefaultRequestHeaders.Add("cookie", "session=" + sessionToken);

            Directory.SetCurrentDirectory("../../../");
            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Type[,] Solutions = new Type[year - 2014, 26];
            for (int i = 0; i < assembly.Length; i++)
                if (assembly[i].IsSubclassOf(typeof(AoCDay)))
                    Solutions[int.Parse(assembly[i].Namespace[^4..]) - 2015,
                        int.Parse(assembly[i].Name[1..3])] = assembly[i];

            Console.WriteLine("Run {0} day: {1}", year, day);
            while (true)
            {
                RunSolutionAsync(year, day);

                while (true)
                {
                    Console.Write("Run {0} day: ", year);
                    string read = Console.ReadLine();
                    if (read == "all")
                    {
                        for (int d = 1; d <= 25; d++)
                        {
                            Console.WriteLine("Day " + d);
                            RunSolutionAsync(year, d);
                        }
                        continue;
                    }
                    else if (int.TryParse(read, out day) && day >= 1 && day <= 25)
                        break;
                    else
                    {
                        Console.Write("Change year to: ");
                        if (!int.TryParse(Console.ReadLine(), out year)
                            || year < 2015 || year > DateTime.Now.Year)
                            return;
                    }
                }
            }

            async void RunSolutionAsync(int year, int day)
            {
                Type type = Solutions[year - 2015, day];
                if (type is null)
                    Console.WriteLine("{0} day {1} is not done.", year, day);
                else
                {
                    string path = year + "/Inputs/" + day.ToString("00") + ".txt";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                        File.WriteAllText(path, await
                            client.GetStringAsync(year + "/day/" + day + "/input"));
                    }
                    AoCDay solution = (AoCDay)Activator.CreateInstance(type);
                    var (part1, part2, time) = solution.Run(path);
                    Console.WriteLine(part1);
                    if (day != 25)
                        Console.WriteLine(part2);
                    Console.WriteLine("Time: {0} ms", time);
                }
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}
