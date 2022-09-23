using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2020
{
    class _25_CryptographicHandshakes : AoCDay
    {
        protected override void Run()
        {
            int door = int.Parse(inputLines[0]),
                card = int.Parse(inputLines[1]),
                subject = 7, value = 1, loopSize = 0;
            while(true)
            {
                value *= subject;
                value %= 20201227;
                loopSize++;

                if (value == door)
                { subject = card; break; }
                if (value == card)
                { subject = door; break; }
            }
            part1 = 1;
            for (int i = 0; i < loopSize; i++)
            {
                part1 *= subject;
                part1 %= 20201227;
            }
        }
    }
}
