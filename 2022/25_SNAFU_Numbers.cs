using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    internal class _25_SNAFU_Numbers : AoCDay
    {
        protected override void Run()
        {
            long sum = 0;
            foreach (string snafu in inputLines)
            {
                long base10 = 0;
                for (int d = 0; d < snafu.Length; d++)
                {
                    char digit = snafu[d];
                    int value = char.IsNumber(digit) ? (int)char.GetNumericValue(digit) :
                        digit == '-' ? -1 : -2;
                    base10 += value * (long)Math.Pow(5, snafu.Length - d - 1);
                }
                sum += base10;
            }

            int pow = (int)Math.Floor(Math.Log(sum, 5));
            List<int> result = new();
            for (int p = pow; p >= 0; p--)
            {
                double decimalValue = Math.Pow(5, p);
                int digit = (int)Math.Floor(sum / decimalValue);
                result.Add(digit);
                sum -= digit * (long)decimalValue;
            }
            for (int d = result.Count - 1; d >= 0; d--)
            {
                if (result[d] <= 2) continue;
                (int div, int rem) = Math.DivRem(result[d], 5);
                if (rem > 2) { div++; rem -= 5; }
                result[d] = rem;
                if (d != 0)
                    result[d - 1] += div;
                else
                {
                    result.Insert(0, div);
                    d++;
                }
            }
            foreach (int digit in result)
                part1_str += digit >= 0 ? digit.ToString() :
                        digit == -1 ? "-" : "=";
        }
    }
}
