using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _13_Packets : AoCDay
    {
        class Packet : List<Packet>, IComparable<Packet>
        {
            readonly int? value = null;

            public Packet() { }

            public Packet(int value)
            {
                this.value = value;
            }

            public Packet(string packet)
            {
                Stack<Packet> stack = new();
                int prev = 0;
                for (int chr = 0; chr < packet.Length; chr++)
                {
                    switch (packet[chr])
                    {
                        case '[': stack.Push(new()); prev = chr + 1; break;
                        case ']':
                            if (prev != chr)
                                stack.Peek().Add(new(int.Parse(packet[prev..chr])));
                            Packet pop = stack.Pop();
                            if (stack.Count == 0) AddRange(pop);
                            else stack.Peek().Add(pop);
                            prev = chr + 1;
                            break;
                        case ',':
                            if (prev != chr)
                                stack.Peek().Add(new(int.Parse(packet[prev..chr])));
                            prev = chr + 1;
                            break;
                    }
                }
            }

            public int CompareTo(Packet other)
            {
                int result = 0;
                for (int i = 0; i < Count && i < other.Count; i++)
                {
                    Packet left = this[i], right = other[i];
                    switch (Convert.ToInt32(left.value is null) + Convert.ToInt32(right.value is null) * 2)
                    {
                        case 0: result = (int)(left.value - right.value); break;
                        case 3: result = left.CompareTo(right); break;
                        case 1: result = left.CompareTo(new() { new((int)right.value) }); break;
                        case 2: result = new Packet() { new((int)left.value) }.CompareTo(right); break;
                    }
                    if (result != 0) return result;
                }
                if (result == 0) return Count - other.Count;
                return result;
            }

            public override string ToString() => value is null ? $"[{string.Join(',', this)}]" : value.ToString();
        }
        protected override void Run()
        {
            debug = 0;
            List<Packet> packets = new();
            for (int s = 0; s < inputSections.Length; s++)
            {
                string[] section = inputSections[s];
                Packet left = new(section[0]), right = new(section[1]);
                int compare = left.CompareTo(right);
                if (compare <= 0) part1 += s + 1;

                packets.Add(left);
                packets.Add(right);
            }

            Packet decoder1 = new("[[2]]"), decoder2 = new("[[6]]");
            packets.Add(decoder1);
            packets.Add(decoder2);
            packets.Sort();

            if (debug == 1)
                foreach (Packet packet in packets)
                    Console.WriteLine(packet);

            part2 = (packets.IndexOf(decoder1) + 1) * (packets.IndexOf(decoder2) + 1);
        }
    }
}
