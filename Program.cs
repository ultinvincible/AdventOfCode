using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Net.Http;

namespace Advent_of_Code
{
    class Program
    {
        const int firstYear = 2015;
        static int NowYear => DateTime.Now.Year;
        static int NowMonth => DateTime.Now.Month;
        static int NowDay => DateTime.Now.Day;
        static Type[,] Solutions;
        static HttpClient client = new()
        {
            BaseAddress = new Uri("https://adventofcode.com")
        };
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory("../../../");
            int year = NowYear,
                day = Math.Min(NowDay, 25);
            if (NowMonth != 12) { year--; day = 25; }

            Type[] assembly = Assembly.GetExecutingAssembly().GetTypes();
            Solutions = new Type[year - firstYear + 1, 26];
            for (int i = 0; i < assembly.Length; i++)
                if (assembly[i].IsSubclassOf(typeof(AoCDay)))
                    Solutions[int.Parse(assembly[i].Namespace[^4..]) - firstYear,
                        int.Parse(assembly[i].Name[1..3])] = assembly[i];

            client.DefaultRequestHeaders.Add
                ("cookie", "session=" + File.ReadAllText("sessionToken.txt"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                ".NET/6.0 (+via https://github.com/ultinvincible/AdventOfCode by trinhminhkhanh278@gmail.com)");

            Console.WriteLine($"Run {year} day: {day}");
            RunSolution(year, day, false);
            Console.WriteLine(new string('~', Console.BufferWidth) + "\n");
            while (true)
            {
                Console.Write($"Run {year} day: ");
                string read = Console.ReadLine();
                if (read == "all")
                {
                    Console.WriteLine();
                    for (int d = 1; d <= 25 &&
                        DateTime.Now >= new DateTime(year, 12, d); d++)
                    {
                        Console.WriteLine("Day " + d);
                        RunSolution(year, d, false);
                    }
                }
                else if (int.TryParse(read, out day) && day >= 1 && day <= 25)
                {
                    RunSolution(year, day);
                }
                else
                {
                    Console.Write("Change year to: ");
                    if (!int.TryParse("20" + Console.ReadLine(), out year)
                        || year < 2015 || year > NowYear ||
                        (year == NowYear && NowMonth < 12))
                        return;
                }
                Console.WriteLine(new string('~', Console.BufferWidth) + "\n");
            }
        }

        static void RunSolution(int year, int day, bool submit = true)
        {
            Type type = Solutions[year - 2015, day];
            if (type is null)
            {
                Console.WriteLine($"{year} day {day} is not done.");
                return;
            }

            string path = $"{year}/Inputs/{day:00}.txt";
            if (!File.Exists(path) || new FileInfo(path).Length < 6)
            {
                FileStream file = File.Create(path);
                client.Send(new(HttpMethod.Get, $"{year}/day/{day}/input"))
                  .Content.ReadAsStream().CopyTo(file);
                file.Close();
            }
            AoCDay solution = (AoCDay)Activator.CreateInstance(type);
            var (part1, part2, time) = solution.Run(path);
            Console.WriteLine("Part 1: " + part1);
            if (day != 25) Console.WriteLine("Part 2: " + part2);
            Console.WriteLine($"Time: {time} ms");

            if (part1 == "Not done." || !submit) return;
            Console.Write("Enter \"y\" to submit: ");
            if (Console.ReadLine() != "y") return;
            int part = part2 == "Not done." ? 1 : 2;
            string[] result = new string[] { part1, part2 };

            HttpRequestMessage request = new(HttpMethod.Post, $"/{year}/day/{day}/answer")
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new("level", part.ToString()),
                    new("answer", result[part - 1])
                })
            };
            HttpResponseMessage send = client.Send(request);
            string response = new StreamReader(send
                .Content.ReadAsStream()).ReadToEnd();
            File.AppendAllText("responses.txt", response + "\n");
            response = response.Split(new string[] {
                                "<main>\n<article><p>","</p></article></main>" },
                StringSplitOptions.None)[1];
            response = StripHtmlTags(response.Split(
                new string[] { $"<a href=\"/{year}/day/{day}", " You can " },
                StringSplitOptions.None)[0]);
            Console.WriteLine(response);

            if (response != "You don't seem to be solving the right level.  Did you already complete it? ")
                return;
            Console.Write("Results: ");
            response = new StreamReader(client.Send(new(HttpMethod.Get, $"/{year}/day/{day}"))
                .Content.ReadAsStream()).ReadToEnd();
            string[] split = response.Split(new string[] {
               "Your puzzle answer was <code>",
                "</code>.</p><article",
                "</code>.</p><p class=\"day-success" },
                StringSplitOptions.None);
            Console.Write(part1 == split[1] ? "Correct" : "Incorrect");
            if (split.Length > 3)
                Console.Write(" | " + (part2 == split[3] ? "Correct" : "Incorrect"));
            Console.WriteLine();
        }
        static string StripHtmlTags(string html)
        {
            string[] split = html.Split(new char[] { '<', '>' });
            string result = "";
            for (int i = 0; i < split.Length; i += 2)
                result += split[i];
            return result;
        }
    }
}
