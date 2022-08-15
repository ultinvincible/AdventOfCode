using System;

namespace Advent_of_Code._2021
{
    class _03_BitCriteria : AoCDay
    {
        public override void Run()
        {
            string gamma = "", epsilon = "";
            for (int i = 0; i < inputLines[0].Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < inputLines.Length; j++)
                    sum += int.Parse(inputLines[j][i].ToString());
                bool moreZeros = sum * 2 < inputLines.Length;
                gamma += Convert.ToInt32(moreZeros);
                epsilon += Convert.ToInt32(!moreZeros);
            }
            part1 = Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);

            string BitCriteria(bool moreCommon)
            {
                string result = "";
                string[] numbers = Array.ConvertAll(inputLines, _ => _);
                for (int i = 0; i < inputLines[0].Length; i++)//O(n)
                {
                    if (numbers.Length > 1)
                    {
                        Array.Sort(numbers, (s1, s2) => s1[i] - s2[i]);//O(n^2*Log(2,n))
                        int j = 0;
                        while (numbers[j][i] != '1' && j < numbers.Length)
                            j++;//O(n^2)
                        bool one = (j * 2 <= numbers.Length) == moreCommon;
                        result += Convert.ToInt32(one).ToString();
                        if (one)
                            numbers = numbers[j..^0];
                        else
                            numbers = numbers[0..j];
                    }
                    else result = numbers[0];
                }
                return result;
            }
            string oxy = BitCriteria(true), co2 = BitCriteria(false);
            //Console.WriteLine(oxy + "|" + co2);
            part2 = Convert.ToInt32(oxy, 2) * Convert.ToInt32(co2, 2);
        }
    }
}
