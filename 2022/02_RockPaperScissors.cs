
namespace Advent_of_Code._2022
{
    class _02_RockPaperScissors : AoCDay
    {
        protected override void Run()
        {
            (int, int)[] guide = new (int, int)[inputLines.Length];
            for (int l = 0; l < inputLines.Length; l++)
                guide[l] = (inputLines[l][0] - 'A', inputLines[l][2] - 'X');

            foreach ((int opp, int play) in guide)
            {
                int outcome = (play - opp + 4) % 3;
                part1 += outcome * 3 + play + 1;
            }

            foreach ((int opp, int play) in guide)
            {
                int shape = (opp + play + 2) % 3;
                part2 += play * 3 + shape + 1;
            }
        }
    }
}
