using System;

namespace Advent_of_Code._2021
{
    class _04_SquidBingo : AoCDay
    {
        class Cell
        {
            public int number;
            public bool marked;
            public Cell(int n)
            {
                number = n;
                marked = false;
            }
            public override string ToString()
            {
                if (marked) return number.ToString("00\u2588");
                return number.ToString("00 ");
            }
        }
        protected override void Run()
        {
            int[] draws = Array.ConvertAll(inputSections[0][0].Split(','), int.Parse);
            string[][] strBoards = inputSections[1..];
            Cell[][,] boards = new Cell[strBoards.Length][,];
            for (int i = 0; i < strBoards.Length; i++)
            {
                boards[i] = new Cell[5, 5];
                string[] brd = strBoards[i];
                for (int ii = 0; ii < brd.Length; ii++)
                {
                    string[] line = brd[ii].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int iii = 0; iii < line.Length; iii++)
                        boards[i][ii, iii] = new(int.Parse(line[iii]));
                }
            }
            (part1, part2) = (-1, -1);

            void MarkAllBoards(int draw)
            {
                //Console.Clear();
                //Console.Write("Draw: " + draw + "\n\n");
                for (int i = 0; i < boards.Length; i++) //board
                {
                    for (int ii = 0; ii < 5; ii++) //line
                    {
                        for (int iii = 0; iii < 5; iii++) //number
                        {
                            if (boards[i][ii, iii].number == draw)
                                boards[i][ii, iii].marked = true;
                            //Console.Write(boards[i][ii, iii] + "|");
                        }
                        //Console.WriteLine();
                    }
                    //Console.WriteLine();
                }
            }
            bool CheckBingoBoard(int boardNo)
            {
                for (int i = 0; i < 5; i++) //line
                {
                    bool bingoHor = true, bingoVer = true;
                    for (int ii = 0; ii < 5; ii++) //number
                    {
                        if (!boards[boardNo][i, ii].marked)
                            bingoHor = false;
                        if (!boards[boardNo][ii, i].marked)
                            bingoVer = false;
                    }
                    if (bingoHor || bingoVer) return true;
                }
                return false;
            }
            int Result(int boardNo, int draw)
            {
                int unmarkedSum = 0;
                for (int i = 0; i < 5; i++) //line
                    for (int ii = 0; ii < 5; ii++) //number
                        if (!boards[boardNo][i, ii].marked)
                            unmarkedSum += boards[boardNo][i, ii].number;
                return unmarkedSum * draw;
            }

            bool bingo = false;
            for (int d = 0; d < draws.Length && !bingo; d++)
            {
                MarkAllBoards(draws[d]);
                if (d > 4)
                {
                    for (int boardNo = 0; boardNo < boards.Length && !bingo; boardNo++) //board
                    {
                        bingo = CheckBingoBoard(boardNo);
                        if (bingo)
                            part1 = Result(boardNo, draws[d]);
                    }
                }
            }

            foreach (Cell[,] board in boards)
                foreach (Cell c in board)
                    c.marked = false;
            bingo = false;

            bool[] won = new bool[boards.Length];
            int wonBoards = 0, chooseBoard = -1;
            for (int d = 0; d < draws.Length && boards.Length != wonBoards; d++)
            {
                MarkAllBoards(draws[d]);
                if (d > 4)
                {
                    for (int boardNo = 0; boardNo < boards.Length && boards.Length != wonBoards; boardNo++) //board
                    {
                        if (won[boardNo]) continue;
                        bingo = CheckBingoBoard(boardNo);
                        if (bingo)
                        {
                            won[boardNo] = true;
                            wonBoards++;
                        }
                        if (boards.Length == wonBoards + 1) chooseBoard = boardNo;
                    }
                }
                //foreach (bool w in won)
                //    Console.Write(w.ToString() + "|");
                //Console.Write("\n\n");

                if (boards.Length == wonBoards)
                    part2 = Result(chooseBoard, draws[d]);
            }
        }
    }
}
