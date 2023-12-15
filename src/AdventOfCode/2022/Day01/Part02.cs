using AocLib;

namespace AdventOfCode._2022.Day01;

public class Part02 : PuzzleSolver<long>
{
    public override long Solve() => this.input
        .SplitEmptyLines()
        .Select(_ => _.SplitLines()
            .Select(int.Parse)
            .Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
}
