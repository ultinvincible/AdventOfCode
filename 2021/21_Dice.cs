using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _21_Dice : AoCDay
    {
        static int[] Move(int[] state, int playerTurn, int move)
        {
            int[] newState = Array.ConvertAll(state, _ => _);
            newState[playerTurn] += move;
            int rem = newState[playerTurn] % 10;
            if (rem == 0)
                newState[playerTurn] = 10;
            else newState[playerTurn] = rem;
            newState[playerTurn + 2] += newState[playerTurn];
            return newState;
        }
        static (int, int) Move(int pos, int score, int move)
        {
            pos += move;
            if (pos > 10) pos -= 10;
            score += pos;
            return (pos, score);
        }
        protected override void Run(out (object part1, object part2) answer)
        {
            int[] state = new int[]{(int)char.GetNumericValue(inputLines[0][^1]),
                                    (int)char.GetNumericValue(inputLines[1][^1]) , 0 , 0 };
            int plrTurn = 0, timesRolled = 0;
            for (int dice = 0; state[2] < 1000 && state[3] < 1000;)
            {
                int move = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (++dice > 100)
                        dice = 1;
                    move += dice;
                }
                state = Move(state, plrTurn, move);
                plrTurn = 1 - plrTurn;
                timesRolled += 3;
            }
            answer.part1 = Math.Min(state[2], state[3]) * timesRolled;

            Dictionary<int, int> moves = new();
            int[] sides = new int[] { 1, 2, 3 };
            foreach (int r1 in sides)
                foreach (int r2 in sides)
                    foreach (int r3 in sides)
                    {
                        int sum = r1 + r2 + r3;
                        if (!moves.ContainsKey(sum))
                            moves.Add(sum, 1);
                        else moves[sum]++;
                    }
            List<(int pos, int score, long count)>[] players = new List<(int, int, long)>[] {
                new() { ((int)char.GetNumericValue(inputLines[0][^1]), 0, 1) },
                new() { ((int)char.GetNumericValue(inputLines[1][^1]), 0, 1) } };
            plrTurn = 0;
            long[] wins = new long[] { 0, 0 };
            List<(int, int, long)> newPlayers;
            do
            {
                newPlayers = new();
                long sumOtherPlr = players[1 - plrTurn].Sum(p => p.count);
                foreach (var (pos, score, count) in players[plrTurn])
                    foreach (var (move, moveCount) in moves)
                    {
                        (int pos, int score) moved = Move(pos, score, move);
                        if (moved.score >= 21)
                            wins[plrTurn] += count * moveCount * sumOtherPlr;
                        else
                            newPlayers.Add((moved.pos, moved.score, count * moveCount));
                    }
                players[plrTurn] = newPlayers;
                plrTurn = 1 - plrTurn;
            } while (newPlayers.Count != 0);
            answer.part2 = Math.Max(wins[0], wins[1]);
        }
    }
}
