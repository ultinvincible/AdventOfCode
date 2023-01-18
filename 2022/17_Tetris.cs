using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2022
{
    internal class _17_Tetris : AoCDay
    {
        List<(int row, int col)>[] shapes;
        int[] heights, widths;
        // bottom checks, left checks, right checks, the rest
        readonly int[][][] checks = new int[][][]
        {
            new int[][] // -
            {
                new int[] { 0, 1, 2, 3 },
                new int[] { 0 },
                new int[] { 3 }
            },
            new int[][] // +
            {
                new int[] { 0, 1, 3 },
                new int[] { 0, 1, 4 },
                new int[] { 0, 3, 4 }
            },
            new int[][] // _|
            {
                new int[] { 0, 1, 2 },
                new int[] { 0, 3, 4 },
                new int[] { 2, 3, 4 }
            },
            new int[][] // |
            {
                new int[] { 0 },
                new int[] { 0, 1, 2, 3 },
                new int[] { 0, 1, 2, 3 }
            },
            new int[][] // O
            {
                new int[] { 0, 1 },
                new int[] { 0, 2 },
                new int[] { 1, 3 }
            },
        };

        void AddLayer(List<bool[]> map, int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool[] layer = new bool[9];
                layer[0] = true; layer[8] = true;
                map.Add(layer);
            }
        }

        const long stupidElephants = 1000000000000;
        protected override void Run()
        {
            debug = 0;

            shapes = new List<(int row, int col)>[inputSections.Length - 1];
            heights = new int[inputSections.Length - 1];
            widths = new int[inputSections.Length - 1];
            for (int s = 0; s < inputSections.Length - 1; s++)
            {
                string[] shape = inputSections[s + 1];
                shapes[s] = new();
                for (int row = shape.Length - 1; row >= 0; row--)
                    for (int col = 0; col < shape[row].Length; col++)
                        if (shape[row][col] == '#')
                            shapes[s].Add((shape.Length - 1 - row, col));
                heights[s] = shape.Length;
                widths[s] = shape[0].Length;
            }
            string jetPattern = inputSections[0][0];

            List<bool[]> map = new() { new bool[9] };
            for (int i = 0; i < 9; i++)
                map[0][i] = true;
            AddLayer(map, 7);

            int height = 0, jet = 0;
            int repeat1 = 0, repeat2 = 0, jetAtRep1 = 0;
            List<int> heightAt = new();
            for (int i = 0; i < 2022 || repeat2 == 0 || i < repeat2 * 2 - repeat1 + 10; i++)
            {
                int shapeIndex = i % shapes.Length;
                List<(int row, int col)> shape = shapes[shapeIndex];
                int[][] check = checks[shapeIndex];

                if (debug == 1) Print(map, shapeIndex, height + 4, 3);

                (int row, int col) = (height + 1, 3);
                for (int ii = 0; ii < 4; ii++)
                {
                    char direction = jetPattern[jet++];
                    if (jet == jetPattern.Length) jet = 0;
                    if (direction == '<') { if (col != 1) col--; }
                    else if (direction == '>') { if (col != 8 - widths[shapeIndex]) col++; }
                    else throw new Exception("Unexpected direction"); // safety
                }
                while (true)
                {
                    if (Array.TrueForAll(check[0], ii => !map[row + shape[ii].row - 1][col + shape[ii].col])) row--;
                    else break;

                    char direction = jetPattern[jet++];
                    if (jet == jetPattern.Length) jet = 0;
                    if (direction == '<')
                    {
                        if (Array.TrueForAll(check[1], ii => !map[row + shape[ii].row][col + shape[ii].col - 1])) col--;
                    }
                    else if (direction == '>')
                    {
                        if (Array.TrueForAll(check[2], ii => !map[row + shape[ii].row][col + shape[ii].col + 1])) col++;
                    }
                }
                foreach ((int r, int c) in shape)
                    map[row + r][col + c] = true;
                height = Math.Max(height, row + heights[shapeIndex] - 1);
                AddLayer(map, height + 8 - map.Count);

                if (repeat2 == 0)
                    heightAt.Add(height);
                if (height != 1 && Enumerable.SequenceEqual(map[height], map[1]))
                {
                    if (repeat1 == 0)
                    {
                        repeat1 = i;
                        jetAtRep1 = jet;
                    }
                    else if (repeat2 == 0 && jet == jetAtRep1)
                    {
                        repeat2 = i;
                    }
                }
                if (i == 2021) part1 = height;
            }

            for (int row = heightAt[repeat2] - heightAt[repeat1]; row >= 1; row--)
            {
                for (int col = 1; col <= 7; col++)
                    if (map[heightAt[repeat1] + row][col] != map[heightAt[repeat2] + row][col])
                    { }

                if (debug == 2) Console.WriteLine(
                    string.Concat(Array.ConvertAll(map[heightAt[repeat1] + row][1..8], b => b ? block : " ")) + "    " +
                    string.Concat(Array.ConvertAll(map[heightAt[repeat2] + row][1..8], b => b ? block : " ")));
            }
            part2 = heightAt[repeat1]
                + Math.DivRem(stupidElephants - repeat1 - 1, repeat2 - repeat1, out long rem)
                * (heightAt[repeat2] - heightAt[repeat1])
                + heightAt[repeat1 + (int)rem] - heightAt[repeat1];
        }

        void Print(List<bool[]> map, int sI = -1, int row = -1, int col = -1)
        {
            Console.WriteLine();
            char[][] print = map.ConvertAll(row => Array.ConvertAll(row, b => b ? '#' : '.')).ToArray();
            if (sI >= 0)
                foreach ((int r, int c) in shapes[sI])
                    print[row + r][col + c] = '@';
            for (int line = map.Count - 1; line >= 1; line--)
            {
                print[line][0] = '|'; print[line][8] = '|';
                Console.WriteLine(string.Concat(print[line]));
            }
            Console.WriteLine("+-------+");
            Console.WriteLine("Height: " + (map.Count - 8));
        }
    }
}
