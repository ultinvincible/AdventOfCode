using System;

namespace Advent_of_Code._2020
{
    class _12_FerryNavigation : AoCDay
    {
        int x, y, waypointX, waypointY;
        protected override void Run()
        {
            waypointX = 1; waypointY = 0;
            Navigate(ref x, ref y);
            part1 = Math.Abs(x) + Math.Abs(y);
            waypointX = 10; waypointY = 1;
            Navigate(ref waypointX, ref waypointY);
            part2 = Math.Abs(x) + Math.Abs(y);
        }

        private void Navigate(ref int changeX, ref int changeY)
        {
            x = 0; y = 0;
            foreach (string line in inputLines)
            {
                int value = int.Parse(line[1..]);
                switch (line[0])
                {
                    case 'N':
                        changeY += value;
                        break;
                    case 'E':
                        changeX += value;
                        break;
                    case 'W':
                        changeX -= value;
                        break;
                    case 'S':
                        changeY -= value;
                        break;
                    case 'L':
                        for (int i = 0; i < value / 90; i++)
                        {
                            int oldWPX = waypointX;
                            waypointX = -waypointY;
                            waypointY = oldWPX;
                        }
                        break;
                    case 'R':
                        for (int i = 0; i < value / 90; i++)
                        {
                            int oldWPX = waypointX;
                            waypointX = waypointY;
                            waypointY = -oldWPX;
                        }
                        break;
                    case 'F':
                        x += waypointX * value;
                        y += waypointY * value;
                        break;
                    default:
                        throw new Exception("Unknown action.");
                }
            }
        }
    }
}
