﻿using AdventOfCode._2022.Day05;
using AocLib;

namespace _2022.Day05;

public partial class Part01 : PuzzleSolver<string>
{
    protected override string InternalSolve()
    {
        var (stacks, moves) = input
            .SplitEmptyLines(_ =>
            (
                _[0].ParseStacks(),
                _[1].ParseMoves()
            ));

        moves.ForEach(m => stacks[m[2] - 1].PushRange(
            stacks[m[1] - 1].PopRange(m[0])
        ));

        return stacks.Select(_ => _.Pop()).AsString();

    }
}
