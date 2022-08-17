using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2019
{
    class _04_SecureContainer : AoCDay
    {
        protected override void Run()
        {
            int min = int.Parse(input[0..6]), max = int.Parse(input[7..13]),
                count1 = 0, count2 = 0;
            for (int i = min; i <= max; i++)
            {
                int[] password = Array.ConvertAll(i.ToString().ToCharArray(),
                    c => (int)char.GetNumericValue(c));
                bool valid = false, valid2 = false;
                for (int j = 1; j < 6; j++)
                {
                    if (password[j] < password[j - 1])
                    {
                        valid = false;
                        break;
                    }
                    if (password[j] == password[j - 1])
                    {
                        valid = true;
                        if ((j == 1 || password[j] != password[j - 2]) &&
                            (j == 5 || password[j] != password[j + 1]))
                            valid2 = true;
                    }
                }
                if (valid) { count1++; if (valid2) count2++; }
            }
            part1 = count1;
            part2 = count2;
        }
    }
}
