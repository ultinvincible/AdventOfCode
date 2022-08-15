using System;

namespace Advent_of_Code._2021
{
    class _25_Cucumbers : AoCDay
    {
        protected override void Run()
        {
            char[,] cucumbers = GridParse(inputLines, _ => _);
            //Console.WriteLine(GridStr(cucumbers));
            int[] directions = new int[] { 1, 0 }; // 1|0 -> east|south
            int[] length = new int[]
            { cucumbers.GetLength(0), cucumbers.GetLength(1) };
            char[] dirChar = new char[] { 'v', '>' };

            bool move;
            int step = 0;
            do
            {
                step++;
                //Console.WriteLine(new string('-', length[1] + 2));
                //Console.WriteLine("Step {0}:", step);
                move = false;
                foreach (int dir in directions)
                {
                    char[,] newCcb = (char[,])cucumbers.Clone();
                    for (int s = 0; s < length[0]; s++) // south
                        for (int e = 0; e < length[1]; e++) // east
                            if (cucumbers[s, e] == dirChar[dir])
                            {
                                int[] to = new int[] { s, e };
                                if (++to[dir] == length[dir])
                                    to[dir] = 0;
                                if (cucumbers[to[0], to[1]] == '.')
                                {
                                    newCcb[s, e] = '.';
                                    newCcb[to[0], to[1]] = dirChar[dir];
                                    move = true;
                                }
                            }
                    //Console.Write(GridStr(cucumbers));
                    cucumbers = newCcb;
                }
            } while (move);
            part1 = step;
            part2 = default;
            //Console.WriteLine(GridStr(cucumbers));
        }
    }
}
