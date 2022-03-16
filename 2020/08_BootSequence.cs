using System;
using System.Collections.Generic;

namespace Advent_of_Code._2020
{
    class _08_BootSequence : AoCDay
    {
        static (int accumulator, bool terminate) RunSequence
            ((string instruction, int value)[] sequence, int changeLine = -1)
        {
            int line = 0, accumulator = 0;
            bool[] passed = new bool[sequence.Length];
            while (!passed[line])
            {
                passed[line] = true;
                string instruction = sequence[line].instruction;
                if (line == changeLine)
                {
                    if (instruction == "jmp")
                        instruction = "nop";
                    else instruction = "jmp";
                }
                switch (instruction)
                {
                    case "acc":
                        accumulator += sequence[line].value;
                        line++;
                        break;
                    case "jmp":
                        line += sequence[line].value;
                        break;
                    case "nop":
                        line++;
                        break;
                }
                if (line >= sequence.Length)
                    return (accumulator, true);
            }
            return (accumulator, false);
        }
        protected override void Run(out (object part1, object part2) answer)
        {
            (string instruction, int value)[] sequence = Array.ConvertAll(inputLines, line =>
            {
                string[] split = line.Split(' ');
                return (split[0], int.Parse(split[1]));
            });

            var (accumulator, terminate) = RunSequence(sequence);
            answer = (accumulator, default);

            for (int line = 0; line < inputLines.Length; line++)
            {
                if (sequence[line].instruction != "acc")
                {
                    (accumulator, terminate) = RunSequence(sequence, line);
                    if (terminate)
                    {
                        answer.part2 = accumulator;
                        return;
                    }
                }
            }
        }
    }
}

