using AocLib;

namespace AdventOfCode._2022.Day01;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve() => this.input
        .SplitEmptyLines()
        .Select(_ => _.SplitLines()
            .Select(int.Parse)
            .Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
}
