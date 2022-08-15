using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _06_ExponentialFish : AoCDay
    {
        protected override void Run()
        {
            int[] inputFishes = Array.ConvertAll(input
                .Trim().Split(','), int.Parse);

            int Spawns_Part1(int days) // O ~ exponential
            {
                List<int> fishes = new(inputFishes);
                for (int d = 0; d < days; d++)
                {
                    int add = 0;
                    for (int f = 0; f < fishes.Count; f++)
                    {
                        if (fishes[f] == 0) // 0(spawners)
                        {
                            fishes[f] = 6; //each 0(spawner) change to 6
                            add++;
                        }
                        else fishes[f]--; //others age down
                    }
                    for (int a = 0; a < add; a++)
                        fishes.Add(8); //each spawner add an 8
                }
                return fishes.Count;
            }
            long Spawns_Part2(int days) // O ~ quadratic
            {
                long[] fishes = new long[9];
                foreach (int age in inputFishes)
                {
                    fishes[age]++;
                }
                for (int d = 0; d < days; d++)
                {
                    long spawners = fishes[0]; // 0(spawners)
                    for (int age = 0; age < fishes.Length - 1; age++)
                        fishes[age] = fishes[age + 1]; //others age down
                    fishes[6] += spawners; //each 0(spawner) change to 6
                    fishes[8] = spawners; //each spawner add an 8
                }
                return fishes.Sum();
            }

            (part1,part2) = (Spawns_Part1(80), Spawns_Part2(256));
        }
    }
}
