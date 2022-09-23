using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _23_CrabCups : AoCDay
    {
        class Cup
        {
            public Cup Destination;
            public Cup Next;
            public int Value;

            public Cup(int value, Cup dest = null)
            {
                Value = value;
                Destination = dest;
            }

            public override string ToString()
            {
                return Value.ToString();
            }

        }
        class CupsLinkedList : IEnumerable<Cup>
        {
            public Cup First;
            public Cup Last;

            public void AddLast(int value, Cup dest = null)
                => AddLast(new Cup(value, dest));
            void AddLast(Cup cup)
            {
                if (First is null)
                {
                    First = cup;
                    Last = cup;
                }
                else
                {
                    Cup add = cup;
                    Last.Next = add;
                    Last = add;
                }
            }

            public Cup Find(int value)
            {
                foreach (Cup cup in this)
                    if (cup.Value == value)
                        return cup;
                return null;
            }

            public IEnumerator<Cup> GetEnumerator()
                => new CupsEnumerator(this);

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            class CupsEnumerator : IEnumerator<Cup>
            {
                public CupsLinkedList cups;
                Cup current;

                public CupsEnumerator(CupsLinkedList cups)
                {
                    this.cups = cups;
                }

                public Cup Current => current;

                object IEnumerator.Current => current;

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    current = current is null ? cups.First : current.Next;
                    return current is not null;
                }

                public void Reset()
                {
                    current = cups.First;
                }
            }
        }
        protected override void Run()
        {
            int[] cups1 = Array.ConvertAll(input.ToCharArray(),
                c => (int)char.GetNumericValue(c)),
                cups2 = Array.ConvertAll(cups1, _ => _);
            cups1 = Move(cups1, 100);
            char[] result = new char[cups1.Length - 1];
            int i1 = Array.FindIndex(cups1, c => c == 1);
            for (int i = 0; i < cups1.Length - 1; i++)
            {
                result[i] = (char)('0' + cups1[(i1 + 1 + i) % 9]);
            }
            part1_str = string.Join("", result);

            part2 = Move(cups2, (int)Math.Pow(10, 7), (int)Math.Pow(10, 6));
        }

        static int[] Move(int[] cups, int times)
        {
            for (int i = 0; i < times; i++)
            {
                int[] pickedUp = cups[1..4];
                int destination = cups[0];
                do
                {
                    destination--;
                    if (destination < 1) destination = cups.Length;
                }
                while (pickedUp.Contains(destination));

                List<int> newCups = new();
                for (int c = 4; c < cups.Length; c++)
                {
                    newCups.Add(cups[c]);
                    if (cups[c] == destination)
                        newCups.AddRange(pickedUp);
                }
                newCups.Add(cups[0]);
                cups = newCups.ToArray();

                //if (debug) Console.WriteLine(string.Join(" ", cups));
            }
            return cups;
        }

        static long Move(int[] inputCups, int times, int count)
        {
            CupsLinkedList cups = new();
            foreach (int cup in inputCups)
                cups.AddLast(cup);
            cups.AddLast(inputCups.Length + 1);
            Cup cup1 = null;
            foreach (var cup in cups)
            {
                if (cup.Value == 1)
                    cup1 = cup;
                else cup.Destination = cups.Find(cup.Value - 1);
            }
            for (int i = inputCups.Length + 2; i <= count; i++)
                cups.AddLast(i, cups.Last);
            cup1.Destination = cups.Last;

            Cup current = cups.First;
            for (int i = 0; i < times; i++)
            {
                Cup[] pickedUp = new Cup[3];
                var next = current;
                for (int ii = 0; ii < 3; ii++)
                {
                    next = next.Next ?? cups.First;
                    pickedUp[ii] = next;
                }

                Cup destination = current.Destination;
                while (pickedUp.Contains(destination))
                {
                    destination = destination.Destination;
                }

                current.Next = pickedUp[^1].Next;
                pickedUp[^1].Next = destination.Next;
                destination.Next = pickedUp[0];

                current = current.Next;
            }
            return (long)cup1.Next.Value * cup1.Next.Next.Value;
        }
    }
}
