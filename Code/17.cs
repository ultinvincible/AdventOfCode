﻿using System;

namespace Advent_of_Code
{
    class ProbeShot : AoCDay
    {
        readonly int xLeftt, xRight, yDownn, yUpppp;
        public ProbeShot() : base(17, false)
        {
            string[] split = inputString[15..^1].Split(".."),
                split1 = split[1].Split(", y=");
            xLeftt = int.Parse(split[0]);
            xRight = int.Parse(split1[0]);
            yDownn = int.Parse(split1[1]);
            yUpppp = int.Parse(split[2]);
        }

        public override void Run()
        {
            int maxY = yDownn;
            int hitTargetStartVelos = 0;
            for (int startVeloX = 1; startVeloX <= xRight; startVeloX++)
            {
                int veloX = startVeloX, x = veloX,
                    stepMin = 0, stepMax;
                bool straightDown = false;
                for (stepMax = 1; x <= xRight && veloX != 0; stepMax++)
                {
                    if (xLeftt <= x && stepMin == 0)
                        stepMin = stepMax;
                    //if (veloX > 0)
                    //    veloX--;
                    //else if (veloX < 0)
                    //    veloX++;
                    //if (veloX == 0)
                    if (--veloX == 0)
                        straightDown = true;
                    x += veloX;
                }
                stepMax--;

                if (stepMin != 0)
                    for (int startVeloY = yDownn; startVeloY < -yDownn; startVeloY++)
                    {
                        int y = (startVeloY * 2 - stepMin + 1) * stepMin / 2,
                            // sum startVeloY to startVeloY + stepMin inclusive
                            veloY = startVeloY - stepMin;
                        for (int step = stepMin;
                            (step <= stepMax || straightDown) && y >= yDownn; step++)
                        {
                            if (yUpppp >= y)
                            {
                                maxY = Math.Max(maxY, startVeloY * (startVeloY + 1) / 2);
                                hitTargetStartVelos++;
                                break;
                            }
                            y += veloY--;
                        }
                    }
            }
            Console.WriteLine(maxY);
            Console.WriteLine(hitTargetStartVelos);
        }
    }
}