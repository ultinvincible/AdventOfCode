using System;

namespace Advent_of_Code._2020
{
    class _01_ExpenseReport : AoCDay
    {
        protected override void Run(out (object part1, object part2) answer)
        {
            answer = default;
            int[] numbers = Array.ConvertAll(inputLines, int.Parse);
            Array.Sort(numbers);

            bool cont = true;
            for (int i = 0; i < numbers.Length && cont; i++)
                for (int j = i + 1; j < numbers.Length && cont; j++)
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        answer.part1 = numbers[i] * numbers[j];
                        cont = false;
                    }

            cont = true;
            for (int i = 0; i < numbers.Length && cont; i++)
                for (int j = i + 1; j < numbers.Length && cont; j++)
                    for (int k = j + 1; k < numbers.Length && cont; k++)
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            answer.part2 = numbers[i] * numbers[j] * numbers[k];
                            cont = false;
                        }
        }
    }
}
