using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2021
{
    class _16_Packets : AoCDay
    {
        class Packet
        {
            public static string Transmission;
            public int start, version, typeID, end;
            public long value;
            public List<Packet> subs = new();

            public Packet(int s)
            {
                start = s;
                version = Convert.ToInt32(Transmission[start..(start + 3)], 2);
                typeID = Convert.ToInt32(Transmission[(start + 3)..(start + 6)], 2);
            }
        }
        Packet outermost;
        protected override void Run(out (object part1, object part2) answer)
        {
            Packet.Transmission = "";
            foreach (char hex in input[..^1])
                Packet.Transmission += Convert.ToString(Convert.ToInt32(hex.ToString(), 16), 2).PadLeft(4, '0');
            //for (int i = 0; i < Packet.Transmission.Length; i++)
            //    Console.WriteLine(i.ToString("0 \u2588 ").PadLeft(7) + char.GetNumericValue(Packet.Transmission[i]));

            outermost = Decode(0);
            answer = (VersionSum(outermost), outermost.value);
        }
        Packet Decode(int start)
        {
            Packet result = new(start);
            if (result.typeID == 4)
            {
                string value = "";
                result.end = start + 6;
                do
                {
                    result.end += 5;
                    value += Packet.Transmission[(result.end - 4)..result.end];
                }
                while (Packet.Transmission[result.end - 5] == '1');
                result.value = Convert.ToInt64(value, 2);
            }
            else
            {
                result.end = start;
                Func<int> CalcLimit;
                if (char.GetNumericValue(Packet.Transmission[start + 6]) == 0)
                {
                    result.end += 7 + 15;
                    CalcLimit = () => result.subs.Sum(p => p.end - p.start);
                }
                else
                {
                    result.end += 7 + 11;
                    CalcLimit = () => result.subs.Count;
                }
                int limit = Convert.ToInt32(
                    Packet.Transmission[(start + 7)..result.end], 2);
                switch (result.typeID)
                {
                    case 1: result.value = 1; break;
                    case 2: result.value = int.MaxValue; break;
                }
                while (CalcLimit() < limit)
                {
                    result.subs.Add(Decode(result.end));
                    result.end = result.subs[^1].end;
                    long value = result.subs[^1].value;
                    switch (result.typeID)
                    {
                        case 0: result.value += value; break;
                        case 1: result.value *= value; break;
                        case 2: result.value = Math.Min(result.value, value); break;
                        case 3: result.value = Math.Max(result.value, value); break;
                    }
                }
                switch (result.typeID)
                {
                    case 5:
                        result.value = Convert.ToInt64(result.subs[0].value > result.subs[1].value);
                        break;
                    case 6:
                        result.value = Convert.ToInt64(result.subs[0].value < result.subs[1].value);
                        break;
                    case 7:
                        result.value = Convert.ToInt64(result.subs[0].value == result.subs[1].value);
                        break;
                }
            }
            return result;
        }
        int VersionSum(Packet p)
        {
            int result = p.version;
            foreach (Packet s in p.subs)
                result += VersionSum(s);
            return result;
        }
    }
}
