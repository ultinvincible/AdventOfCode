﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace Advent_of_Code
{
    class Program
    {
        const int firstYear = 2015;
        static int nowYear => DateTime.Now.Year;
        static int nowMonth => DateTime.Now.Month;
        static int nowDay => DateTime.Now.Day;
        static HttpClient client = new()
        {
            BaseAddress = new Uri("https://adventofcode.com")
        };
        static async Task Main(string[] args)
        {
            Directory.SetCurrentDirectory("../../../");
            int year = nowYear,
                day = Math.Min(nowDay, 25);
            if (nowMonth != 12) { year--; day = 25; }

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
                        Console.Write("Enter \"y\" to submit: ");
                        if (Console.ReadLine() == "y")
                        {
                            int part = 1;
                            if (part2 != "Not done.")
                                part = 2;
                            string[] result = new string[] { part1, part2 };
                            FormUrlEncodedContent content = new(new KeyValuePair<string, string>[]
                            {
                                new("level", part.ToString()),
                                new("answer", result[part - 1])
                            });
                            HttpResponseMessage response = await client.PostAsync($"/{year}/day/{day}/answer", content);
                            string responseContent = await response.Content.ReadAsStringAsync();
                            //bool answered = false;
                            //foreach (string rp in responses)
                            //    if (responseContent.Contains(rp))
                            //    {
                            //        char[] print = rp.ToCharArray();
                            //        print[0] = char.ToUpper(print[0]);
                            //        Console.WriteLine(string.Join("", print) + '.');
                            //        answered = true;
                            //    }
                            //if (!answered)
                            //{
                            //Console.WriteLine($"Level: {part}");
                            Console.WriteLine(StripHtmlTags(responseContent.Split
                                (new string[] { "<main>\n<article><p>",
                                    $"<a href=\"/{year}/day/{day}",
                                    " You can " },
                                 StringSplitOptions.None)[1]));
                            //}
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
        //static readonly string[] responses = new string[]
        //{
        //    "your answer is too high",
        //    "your answer is too low",
        //    "you gave an answer too recently",
        //    "You don't seem to be solving the right level",
        //    "That's not the right answer",
        //    "That's the right answer",
        //};
    }
}
