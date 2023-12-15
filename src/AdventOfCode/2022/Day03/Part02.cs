using AocLib;

namespace AdventOfCode._2022.Day03;

public class Part02 : PuzzleSolver<long>
{
    public override long Solve() => this.input
        .SplitLines()
        .Chunk(3)
        .SelectMany(_ => _
            .Aggregate((acc, cur) => acc.Intersect(cur).CreateString())
            .Select(_ => char.IsLower(_) ? _ - 96 : _ - 38))
        .Sum();
}
