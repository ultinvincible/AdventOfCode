using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2020
{
    class _13_Buses : AoCDay
    {
        protected override void Run()
        {
            string[] inputBuses = inputLines[1].Split(',');
            int earliestTime = int.Parse(inputLines[0]),
                earliestBus = 0, minWait = int.MaxValue;
            List<(int busID, int time)> buses = new();
            for (int i = 0; i < inputBuses.Length; i++)
            {
                if (int.TryParse(inputBuses[i], out int busID))
                    buses.Add((busID, i));
            }

            foreach ((int busID, _) in buses)
            {
                int wait = busID - earliestTime % busID;
                if (wait < minWait)
                {
                    earliestBus = busID;
                    minWait = wait;
                }
            }
            part1 = earliestBus * minWait;

            long time = buses[0].busID - buses[0].time % buses[0].busID,
                product = buses[0].busID;
            for (int i = 1; i < buses.Count; i++)
            {
                while ((time + buses[i].time) % buses[i].busID != 0)
                    time += product;
                product *= buses[i].busID;
            }
            part2 = time;
        }
    }
}
