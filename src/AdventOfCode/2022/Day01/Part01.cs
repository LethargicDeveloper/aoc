using AocLib;

namespace AdventOfCode._2022.Day01;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve() => this.input
        .SplitEmptyLines()
        .Select(_ => _.SplitLines()
            .Select(int.Parse)
            .Sum())
        .Max();
}
