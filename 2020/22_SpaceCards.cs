using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _22_SpaceCards : AoCDay
    {
        protected override void Run()
        {
            List<int>[] inputDecks = new List<int>[2];
            foreach (string player in inputSections)
            {
                List<int> deck = new(Array.ConvertAll(player.Split
                    ("\n", StringSplitOptions.RemoveEmptyEntries)[1..], int.Parse));
                if (inputDecks[0] is null) inputDecks[0] = deck;
                else inputDecks[1] = deck;
            }

            List<int>[] decks = new List<int>[] { new(inputDecks[0]), new(inputDecks[1]) };
            int winner = Combat(decks);
            part1 = Score(decks, winner);

            decks = new List<int>[] { new(inputDecks[0]), new(inputDecks[1]) };
            winner = Combat(decks, true);
            part2 = Score(decks, winner);
        }

        static int Score(List<int>[] decks, int winner)
        {
            int score = 0;
            for (int i = 0; i < decks[winner].Count; i++)
                score += (decks[winner].Count - i) * decks[winner][i];
            return score;
        }

        static int Combat(List<int>[] decks, bool recursive = false)
        {
            List<List<int>> pastDecks = new();
            int winner, round = 1;
            do
            {
                if (debug)
                {
                    Console.WriteLine("Round " + round++);
                    Console.WriteLine(string.Join(", ", decks[0]));
                    Console.WriteLine(string.Join(", ", decks[1]));
                    Console.WriteLine(decks[0][0]);
                    Console.WriteLine(decks[1][0]);
                }

                if (pastDecks.FindIndex(d => d.SequenceEqual(decks[0])) != -1)
                {
                    if (debug) Console.WriteLine("Loop\n");
                    return 0;
                }

                pastDecks.Add(new(decks[0]));
                winner = 0;
                int win = decks[0][0], lose = decks[1][0];
                decks[0].RemoveAt(0);
                decks[1].RemoveAt(0);

                if (recursive && decks[0].Count >= win && decks[1].Count >= lose)
                {
                    if (debug) Console.WriteLine("New game\n");
                    winner = Combat(new List<int>[]
                    { decks[0].Take(win).ToList(), decks[1].Take(lose).ToList() }, true);
                }
                else if (win < lose) winner = 1;

                if (winner == 1) (win, lose) = (lose, win);
                decks[winner].Add(win);
                decks[winner].Add(lose);
                if (debug) Console.WriteLine("Player " + (winner + 1) + " wins round\n");
            } while (decks[1 - winner].Count != 0);

            return winner;
        }
    }
}
