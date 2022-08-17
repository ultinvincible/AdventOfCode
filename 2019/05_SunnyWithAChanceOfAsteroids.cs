using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2019
{
    class _05_SunnyWithAChanceOfAsteroids : AoCDay
    {
        int[] inputProgram;
        List<int> RunProgram(int input)
        {
            int[] program = Array.ConvertAll(inputProgram, _ => _);
            List<int> outputs = new();
            int position = 0;
            bool cont = true;
            while (cont)
            {
                int instruction = program[position];
                string opcode = instruction.ToString();
                if (opcode.Length > 2)
                    instruction = int.Parse(opcode[^2..]);

                int[] parameters = new int[2];
                if (instruction != 3 && instruction != 4 && instruction != 99)
                    for (int i = 0; i < 2; i++)
                        if (opcode.Length - 3 - i < 0 || opcode[opcode.Length - 3 - i] == '0')
                            parameters[i] = program[program[position + 1 + i]];
                        else if (opcode[opcode.Length - 3 - i] == '1')
                            parameters[i] = program[position + 1 + i];

                switch (instruction)
                {
                    case 1:
                        program[program[position + 3]]
                            = parameters[0] + parameters[1];
                        position += 4;
                        break;
                    case 2:
                        program[program[position + 3]]
                            = parameters[0] * parameters[1];
                        position += 4;
                        break;
                    case 3:
                        program[program[position + 1]] = input;
                        position += 2;
                        break;
                    case 4:
                        outputs.Add(program[program[position + 1]]);
                        position += 2;
                        break;
                    case 5:
                        if (parameters[0] != 0) position = parameters[1];
                        else position += 3;
                        break;
                    case 6:
                        if (parameters[0] == 0) position = parameters[1];
                        else position += 3;
                        break;
                    case 7:
                        program[program[position + 3]] =
                        Convert.ToInt32(parameters[0] < parameters[1]);
                        position += 4;
                        break;
                    case 8:
                        program[program[position + 3]] =
                        Convert.ToInt32(parameters[0] == parameters[1]);
                        position += 4;
                        break;
                    case 99:
                        cont = false;
                        break;
                    default: 
                        Console.WriteLine("i = " + input + "\nUnknown opcode"); 
                        return new();
                }
            }
            if (debug)
            {
                Console.WriteLine("i = " + input);
                foreach (int o in outputs)
                    Console.Write(o + "|");
                Console.WriteLine();
            }
            return outputs;
        }
        protected override void Run()
        {
            inputProgram = Array.ConvertAll(input.Split(','), int.Parse);

            if (debug)
                for (int i = 0; i < 10; i++)
                    RunProgram(i);

            part1 = RunProgram(1)[^1];
            part2 = RunProgram(5)[^1];
        }
    }
}
