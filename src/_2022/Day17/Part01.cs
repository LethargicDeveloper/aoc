﻿using AocLib;

namespace _2022.Day17;

public partial class Part01 : PuzzleSolver<long>
{
    static readonly int Floor = 0b111111111;
    static readonly int Wall = 0b100000001;
    static readonly int[][] Pieces = new[]
    {
        new int[] { 0b000111100 },
        new int[] { 0b000010000, 0b000111000, 0b000010000 },
        new int[] { 0b000001000, 0b000001000, 0b000111000 },
        new int[] { 0b000100000, 0b000100000, 0b000100000, 0b000100000 },
        new int[] { 0b000110000, 0b000110000 }
    };

    static bool IsCollision(List<int> board, int[] piece, int y)
    {
        for (int i = 0; i < piece.Length; ++i)
        {
            var p = piece[i];
            var index = y + piece.Length - i - 1;
            var line = board.Count > index ? board[index] : Wall;
            if ((line ^ p) != (line | p))
                return true;
        }

        return false;
    }

    static int[] ShiftLeft(int[] piece)
    {
        var newPiece = new int[piece.Length];
        for (int i = 0; i < piece.Length; ++i)
        {
            newPiece[i] = piece[i] << 1;
        }

        return newPiece;
    }

    static int[] ShiftRight(int[] piece)
    {
        var newPiece = new int[piece.Length];
        for (int i = 0; i < piece.Length; ++i)
        {
            newPiece[i] = piece[i] >> 1;
        }

        return newPiece;
    }

    static int[] SetPiece(int[] piece)
    {
        var line = new int[piece.Length];
        for (int i = 0; i < piece.Length; ++i)
            line[i] = Wall | piece[i];

        return line;
    }

    protected override long InternalSolve()
    {
        var board = new List<int>() { Floor };
        var heightMap = new Dictionary<int, int>();

        PlayGame(board, heightMap);

        return board.Count - 1;
    }

    void PlayGame(List<int> board, Dictionary<int, int> heightMap)
    {
        int top = 1;
        int currentJet = 0;

        for (int round = 0; round < 4000; ++round)
        {
            var piece = Pieces[round % 5];
            var y = top + 3;

            while (true)
            {
                if (input[currentJet++ % input.Length] == '<')
                {
                    var left = ShiftLeft(piece);
                    if (!IsCollision(board, left, y))
                        piece = left;
                }
                else
                {
                    var right = ShiftRight(piece);
                    if (!IsCollision(board, right, y))
                        piece = right;
                }

                if (IsCollision(board, piece, y - 1))
                {
                    var setPiece = SetPiece(piece);
                    for (int i = setPiece.Length - 1; i >= 0; --i)
                    {
                        var index = y + (setPiece.Length - i) - 1;
                        if (index >= board.Count)
                        {
                            board.Add(setPiece[i]);
                        }
                        else
                        {
                            board[index] |= setPiece[i];
                        }
                    }

                    break;
                }

                y--;
            }

            top = board.Count;

            heightMap[round + 1] = top - 1;
        }
    }
}
