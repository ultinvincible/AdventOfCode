using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    internal class _20_Mixing : AoCDay
    {
        const int decryptionKey = 811589153;
        long[] inputNumbers;
        protected override void Run()
        {
            inputNumbers = Array.ConvertAll(inputLines, long.Parse);
            part1 = Mix();
            part2 = Mix(10);
        }

        long Mix(int times = 1)
        {
            LinkedListNode<long>[] encrypted = Array.ConvertAll
                (inputNumbers, i =>
                {
                    if (times > 1) i *= decryptionKey;
                    return new LinkedListNode<long>(i);
                });
            LinkedList<long> decrypt = new();
            foreach (LinkedListNode<long> node in encrypted)
                decrypt.AddLast(node);

            for (int time = 0; time < times; time++)
                for (int i = 0; i < encrypted.Length; i++)
                {
                    if (encrypted[i].Value == 0) continue;

                    LinkedListNode<long> prev = encrypted[i].Previous ?? decrypt.Last;
                    decrypt.Remove(encrypted[i]);

                    long move = encrypted[i].Value % decrypt.Count;
                    for (int m = 0; m < Math.Abs(move); m++)
                        if (move > 0) prev = prev.Next ?? decrypt.First;
                        else prev = prev.Previous ?? decrypt.Last;
                    decrypt.AddAfter(prev, encrypted[i]);
                }

            int[] coordinates = new int[] { 1000 % decrypt.Count, 2000 % decrypt.Count, 3000 % decrypt.Count };
            int max = Math.Max(Math.Max(coordinates[0], coordinates[1]), coordinates[2]);
            long result = 0;
            LinkedListNode<long> resultNode = decrypt.Find(0);
            for (int i = 0; i <= max; i++)
            {
                if (Array.IndexOf(coordinates, i) != -1)
                    result += resultNode.Value;
                resultNode = resultNode.Next ?? decrypt.First;
            }
            return result;
        }
    }
}
