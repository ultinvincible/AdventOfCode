using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2019
{
    class _02_1202ProgramAlarm : AoCDay
    {
        int[] inputProgram;
        int RunProgram(int noun, int verb)
        {
            int[] program = Array.ConvertAll(inputProgram, _ => _);
            program[1] = noun;
            program[2] = verb;

            int position = 0;
            while (true)
            {
                if (program[position] == 1)
                    program[program[position + 3]]
                        = program[program[position + 1]] + program[program[position + 2]];
                else if (program[position] == 2)
                    program[program[position + 3]]
                        = program[program[position + 1]] * program[program[position + 2]];
                else if (program[position] == 99)
                    break;
                else throw new Exception("Unknown opcode");
                position += 4;
            }
            return program[0];
        }
        protected override void Run()
        {
            inputProgram = Array.ConvertAll(input.Split(','), int.Parse);
            part1 = RunProgram(12,2);
            for (int n = 0; n < 99; n++)
                for (int v = 0; v < 99; v++)
                    if (RunProgram(n, v) == 19690720)
                    {
                        part2 = 100 * n + v;
                        return;
                    }
        }
    }
}
