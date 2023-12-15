using AocLib;

namespace AdventOfCode._2022.Day03;

public class Part01 : PuzzleSolver<long>
{
    public override long Solve() => this.input
        .SplitLines()
        .Select(_ => new[]
        {
            _[..(_.Length / 2)],
            _[^(_.Length / 2)..]
        })
        .Select(_ => _[0].First(v => _[1].Contains(v)))
        .Select(_ => char.IsLower(_) ? _ - 96 : _ - 38)
        .Sum();
}
