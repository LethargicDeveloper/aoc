using AocLib;

namespace _2022.Day01;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve() => input
        .SplitEmptyLines()
        .Select(_ => _.SplitLines()
            .Select(int.Parse)
            .Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
}
