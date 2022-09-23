using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _20_JurassicJigsaw : AoCDay
    {
        readonly string[] inputSeaMonster = new string[] {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   " };
        int BorderCode(char[] border)
        // convert borders into base 2 numbers, just because fancy
        {
            int code = 0;
            for (int i = 0; i < 10; i++)
                if (border[i] == '#')
                    code += (int)Math.Pow(2, 10 - i);
            return code;
        }
        bool[,] Transform(bool[,] tile, int rotation, bool flip = false)
        // rotate x times and flip
        {
            rotation %= 4;
            int length = tile.GetLength(0);
            Func<int, int, (int, int)>[]
                transform = new Func<int, int, (int, int)>[]{
                (row, col) => (row, col),
                (row, col) => (col, length - 1 - row),
                (row, col) => (length - 1 - row, length - 1 - col),
                (row, col) => (length - 1 - col, row) };

            if (flip)
                transform = new Func<int, int, (int, int)>[]{
                (row, col) => (length - 1 - row, col),
                (row, col) => (length - 1 - col, length - 1 - row),
                (row, col) => (row, length - 1 - col),
                (row, col) => (col, row), };

            bool[,] result = new bool[length, length];
            for (int row = 0; row < length; row++)
                for (int col = 0; col < length; col++)
                {
                    (int tfRow, int tfCol) = transform[rotation](row, col);
                    result[tfRow, tfCol] = tile[row, col];
                }
            return result;
        }
        static string Print(bool[,] image, bool gaps = true)
        {
            string print = "";
            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int col = 0; col < image.GetLength(1); col++)
                {
                    if (image[row, col]) print += "#";
                    else print += ".";
                    if (gaps && col % 8 == 7) print += " ";
                }
                print += "\n";
                if (gaps && row % 8 == 7) print += "\n";
            }
            return print;
        }
        protected override void Run()
        {
            Dictionary<int, int[]> borders = new();
            Dictionary<int, bool[,]> tiles = new();
            foreach (string inputTile in input.Split("\n\n"))
            {
                int[] borderCodes = new int[8];
                string[] tile = inputTile.Split("\n")[1..];
                char[][] border = new char[4][];
                for (int i = 0; i < 4; i++)
                    border[i] = new char[10];
                for (int i = 0; i < 10; i++)
                {
                    border[0][i] = tile[0][i];
                    border[1][i] = tile[i][9];
                    border[2][i] = tile[9][9 - i];
                    border[3][i] = tile[9 - i][0];
                }
                for (int i = 0; i < 4; i++)
                {
                    borderCodes[i] = BorderCode(border[i]);
                    Array.Reverse(border[i]);
                    borderCodes[i + 4] = BorderCode(border[i]);
                }
                int ID = int.Parse(inputTile[5..9]);
                borders[ID] = borderCodes;

                bool[,] noBorder = new bool[8, 8];
                for (int i = 1; i < 9; i++)
                    for (int ii = 1; ii < 9; ii++)
                        noBorder[i - 1, ii - 1] = tile[i][ii] == '#';
                tiles[ID] = noBorder;
            }

            Dictionary<int, (int ID, int side)[]> matches = new();
            foreach ((int ID1, int[] tile1) in borders)
            {
                matches[ID1] = new (int, int)[4];
                foreach ((int ID2, int[] tile2) in borders)
                {
                    if (ID1 == ID2) continue;
                    for (int i = 0; i < 4; i++)
                        for (int ii = 0; ii < 8; ii++)
                            if (tile1[i] == tile2[ii])
                                if (ii < 4) matches[ID1][i] = (ID2, ii + 20);
                                // + 20 is a token saying match is flipped
                                // 20 because shows og side in number
                                // and %= 4 returns og
                                else matches[ID1][i] = (ID2, ii - 4);
                }
            }

            part1 = 1;
            int startID = 0;
            foreach (var (ID, match) in matches)
                if (Array.FindAll(match, i => i != default).Length == 2)
                {
                    part1 *= ID;
                    if (startID == 0) startID = ID;
                }


            int length = (int)Math.Sqrt(tiles.Count);
            bool[,] image = new bool[length * 8, length * 8];
            int[,] assembly = new int[length, length];
            assembly[0, 0] = startID;
            bool[,] flipped = new bool[length, length];
            for (int row = 0; row < length; row++)
            {
                for (int col = 0; col < length; col++)
                {
                    (int ID, int side)[] match = matches[assembly[row, col]];
                    int nextSide = 3;
                    if ((row, col) == (0, 0))
                        for (int i = 2; i >= 0; i--)
                        {
                            if (match[i] != default && match[i + 1] != default)
                            { nextSide = i; break; }
                        }
                    else
                    {
                        int prevRow = row, prevCol = col - 1; // left tile
                        if (col == 0) (prevRow, prevCol) = (row - 1, col); // above tile
                        nextSide = Array.Find(matches[assembly[prevRow, prevCol]],
                            m => m.ID == assembly[row, col]).side + 2;
                        // side opposite from side facing prevTile
                        if (nextSide >= 20 != flipped[prevRow, prevCol])
                        // flip
                        {
                            flipped[row, col] = true;
                            if (col == 0) nextSide++; // move 1 side counterclockwise
                        }
                        else if (col == 0) nextSide--; // ^ same but not flipped
                        nextSide %= 4;
                    }

                    bool[,] transform = Transform(tiles[assembly[row, col]],
                        5 - nextSide, flipped[row, col]);
                    for (int r = 0; r < 8; r++)
                        for (int c = 0; c < 8; c++)
                            image[row * 8 + r, col * 8 + c] = transform[r, c];

                    if (match[nextSide] != default)
                        assembly[row, col + 1] = match[nextSide].ID; // add ID to right
                    nextSide++;
                    if (flipped[row, col]) nextSide += 2;
                    nextSide %= 4;
                    if (col == 0 && match[nextSide] != default)
                        assembly[row + 1, 0] = match[nextSide].ID; // add ID below
                }
            }
            //image = Transform(image, 3, true); // test input
            if (debug) Console.WriteLine(Print(image));

            List<(int, int)> seaMonster = new();
            for (int row = 0; row < inputSeaMonster.Length; row++)
                for (int col = 0; col < inputSeaMonster[row].Length; col++)
                    if (inputSeaMonster[row][col] == '#')
                        seaMonster.Add((row, col));
            List<(int, int, int)> found = new();
            foreach (bool flip in new bool[] { false, true })
                for (int rotate = 0; rotate < 4; rotate++)
                {
                    bool[,] transform = Transform(image, rotate, flip);
                    if (debug) Console.WriteLine(flip + "|" + rotate + "\n" + Print(transform));
                    for (int row = 0; row < length * 8 - 3; row++)
                        for (int col = 0; col < length * 8 - 20; col++)
                            if (seaMonster.All(rc =>
                            transform[row + rc.Item1, col + rc.Item2]))
                                found.Add((row, col, Convert.ToInt32(flip) * 4 + rotate));
                }
            part2 = -found.Count * seaMonster.Count;
            foreach (bool point in image)
                if (point) part2++;
        }
    }
}
