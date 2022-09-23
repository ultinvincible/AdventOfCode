using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code._2020
{
    class _24_Bestagons : AoCDay
    {
        (int, int)[] HexagonNeighbors = new (int, int)[]
        {
            (0,-1),(1,-1),(-1,0),(1,0),(-1,1),(0,1)
        };
        protected override void Run()
        {
            (int x, int y, int z)[] tiles = new (int, int, int)[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                for (int c = 0; c < line.Length; c++)
                {
                    switch (line[c])
                    {
                        case 's':
                            if (line[c + 1] == 'e')
                            { tiles[i].y--; tiles[i].z++; }
                            else { tiles[i].x--; tiles[i].z++; }
                            c++;
                            break;
                        case 'n':
                            if (line[c + 1] == 'e')
                            { tiles[i].x++; tiles[i].z--; }
                            else { tiles[i].y++; tiles[i].z--; }
                            c++;
                            break;
                        case 'e':
                            tiles[i].x++;
                            tiles[i].y--;
                            break;
                        case 'w':
                            tiles[i].x--;
                            tiles[i].y++;
                            break;
                        default:
                            throw new Exception("wot");
                    }
                }
            }
            (int x, int y, int z)[] distinct = tiles.Distinct().ToArray();
            const int max = 120;
            bool[,] map = new bool[max * 2 + 1, max * 2 + 1];
            foreach (var tile in distinct)
                if (Array.FindAll(tiles, t => t == tile).Length % 2 == 1)
                {
                    part1++;
                    map[tile.x + max, tile.z + max] = true;
                }

            for (int d = 0; d < 100; d++)
            {
                int[,] neiCount = new int[map.GetLength(0), map.GetLength(1)];
                for (int x = 1; x < map.GetLength(0) - 1; x++)
                    for (int z = 1; z < map.GetLength(1) - 1; z++)
                        if (map[x, z])
                            foreach ((int neiX, int neiZ) in HexagonNeighbors)
                                neiCount[x + neiX, z + neiZ]++;

                bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
                for (int x = 0; x < map.GetLength(0); x++)
                    for (int z = 0; z < map.GetLength(1); z++)
                        newMap[x, z] = map[x, z] &&
                            !(neiCount[x, z] == 0 || neiCount[x, z] > 2) ||
                            (!map[x, z] && neiCount[x, z] == 2);
                map = newMap;
                if (debug) Console.WriteLine(GridStr(map, b => b ? "#" : "."));
            }
            foreach (bool tile in map)
                if (tile) part2++;
        }
    }
}
