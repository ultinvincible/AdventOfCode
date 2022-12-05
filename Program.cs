using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace Advent_of_Code
{
    class Program
    {
        static HttpClient client = new()
        {
            BaseAddress = new Uri("https://adventofcode.com")
        };
        static async Task Main(string[] args)
        {
            Directory.SetCurrentDirectory("../../../");
            int nowYear = DateTime.Now.Year,
                nowMonth = DateTime.Now.Month,
                nowDay = DateTime.Now.Day,
                year = nowYear,
                day = Math.Min(nowDay, 25);
            if (nowMonth != 12) { year--; day = 25; }

            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Type[,] Solutions = new Type[year - 2014, 26];
            for (int i = 0; i < assembly.Length; i++)
                if (assembly[i].IsSubclassOf(typeof(AoCDay)))
                    Solutions[int.Parse(assembly[i].Namespace[^4..]) - 2015,
                        int.Parse(assembly[i].Name[1..3])] = assembly[i];

            client.DefaultRequestHeaders.Add
                ("cookie", "session=" + File.ReadAllText("sessionToken.txt"));

            Console.WriteLine($"Run {year} day: {day}");
            await RunSolutionAsync(year, day);
            while (true)
            {
                Console.Write($"Run {year} day: ");
                string read = Console.ReadLine();
                if (read == "all")
                {
                    Console.WriteLine(new string('-', Console.BufferWidth));
                    for (int d = 1; d <= 25 &&
                        DateTime.Now >= new DateTime(year, 12, d); d++)
                    {
                        Console.WriteLine("Day " + d);
                        await RunSolutionAsync(year, d, false);
                    }
                }
                else if (int.TryParse(read, out day) && day >= 1 && day <= 25)
                {
                    await RunSolutionAsync(year, day);
                }
                else
                {
                    Console.Write("Change year to: ");
                    if (!int.TryParse(Console.ReadLine(), out year)
                        || year < 2015 || year > nowYear ||
                        (year == nowYear && nowMonth < 12))
                        return;
                }
            }

            async Task RunSolutionAsync(int year, int day, bool submit = true)
            {
                Type type = Solutions[year - 2015, day];
                if (type is null)
                    Console.WriteLine($"{year} day {day} is not done.");
                else
                {
                    string path = $"{year}/Inputs/{day:00}.txt";

                    if (!File.Exists(path) || File.ReadAllText(path) == "")
                    {
                        File.WriteAllText(path, await client.GetStringAsync
                            ($"{year}/day/{day}/input"));
                    }
                    AoCDay solution = (AoCDay)Activator.CreateInstance(type);
                    var (part1, part2, time) = solution.Run(path);
                    Console.WriteLine("Part 1: " + part1);
                    if (day != 25) Console.WriteLine("Part 2: " + part2);
                    Console.WriteLine($"Time: {time} ms");

                    if (submit)
                    {
                        Console.Write("Submit? ");
                        if (Console.ReadLine() == "y")
                        {
                            int part = 1;
                            if (part2 != "0")
                                part = 2;
                            string[] result = new string[] { part1, part2 };
                            FormUrlEncodedContent content = new(new KeyValuePair<string, string>[]
                            {
                                new("level", part.ToString()),
                                new("answer", result[part - 1])
                            });
                            HttpResponseMessage response = await client.PostAsync($"/{year}/day/{day}/answer", content);
                            string responseContent = await response.Content.ReadAsStringAsync();
                            foreach (string rp in responses)
                                if (responseContent.Contains(rp))
                                    Console.WriteLine(rp);
                            //Console.WriteLine($"Level: {part}");
                            //Console.WriteLine(responseContent.Split(new string[] { "<main>\n<article><p>", "</main>" },
                            //     StringSplitOptions.None)[1]);
                        }
                    }
                }
                Console.WriteLine(new string('-', Console.BufferWidth));
            }
        }
        static string[] responses = new string[]
        {
            "Your answer is too high",
            "Your answer is too low",
            "You gave an answer too recently.",
            "You don't seem to be solving the right level.",
            "That's not the right answer.",
            "That's the right answer!",
        };
    }
}
