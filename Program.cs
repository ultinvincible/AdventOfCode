﻿using System;
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
            Type[,] Solutions = new Type[year - firstYear + 1, 26];
            for (int i = 0; i < assembly.Length; i++)
                if (assembly[i].IsSubclassOf(typeof(AoCDay)))
                    Solutions[int.Parse(assembly[i].Namespace[^4..]) - firstYear,
                        int.Parse(assembly[i].Name[1..3])] = assembly[i];

            client.DefaultRequestHeaders.Add
                ("cookie", "session=" + File.ReadAllText("sessionToken.txt"));
            client.DefaultRequestHeaders.Add("User-Agent",
                ".NET/6.0 (https://github.com/ultinvincible/AdventOfCode by trinhminhkhanh278@gmail.com)");

            Console.WriteLine($"Run {year} day: {day}");
            RunSolution(year, day);
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
                    if (!int.TryParse(Console.ReadLine(), out year)
                        || year < 2015 || year > NowYear ||
                        (year == NowYear && NowMonth < 12))
                        return;
                }
            }

            void RunSolution(int year, int day, bool submit = true)
            {
                Type type = Solutions[year - 2015, day];
                if (type is null)
                    Console.WriteLine($"{year} day {day} is not done.");
                else
                {
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

                    if (part1 != "Not done." && submit)
                    {
                        Console.Write("Enter \"y\" to submit: ");
                        if (Console.ReadLine() == "y")
                        {
                            int part = 1;
                            if (part2 != "Not done.")
                                part = 2;
                            string[] result = new string[] { part1, part2 };
                            HttpRequestMessage request = new(HttpMethod.Post, $"/{year}/day/{day}/answer")
                            {
                                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                                {
                                    new("level", part.ToString()),
                                    new("answer", result[part - 1])
                                })
                            };
                            string response = new StreamReader(client.Send(request)
                                .Content.ReadAsStream()).ReadToEnd();
                            string main = response.Split(new string[] {
                                "<main>\n<article><p>","</main>" },
                                StringSplitOptions.None)[1];
                            File.AppendAllText("answers.txt", main + "\n");
                            Console.WriteLine(StripHtmlTags(main.Split(
                                new string[] { $"<a href=\"/{year}/day/{day}", " You can " },
                                StringSplitOptions.None)[0]));
                        }
                    }
                }
                Console.WriteLine(new string('~', Console.BufferWidth));
            }
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
