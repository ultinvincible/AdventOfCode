using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2022
{
    class _04_Sections : AoCDay
    {
        protected override void Run()
        {
            foreach (string line in inputLines)
            {
                int[] values = Array.ConvertAll
                    (line.Split(new char[] { '-', ',' }), int.Parse);

                if (values[0] <= values[2] && values[3] <= values[1] ||
                    (values[2] <= values[0] && values[1] <= values[3]))
                {
                    part1++;
                    part2++;
                }

                else if (values[0] <= values[2] && values[2] <= values[1] &&
                    values[1] <= values[3] ||
                    (values[2] <= values[0] && values[0] <= values[3] &&
                    values[3] <= values[1]))                    
                    part2++;
            }
        }
    }
}
