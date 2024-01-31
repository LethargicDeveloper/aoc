using AocLib;

namespace AdventOfCode._2022.Day02;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve() => this.input
        .SplitLines()
        .Select(_ => new[] { _[0] - '@', _[2] - 'W' })
        .Select(_ => _[1] switch
        {
            1 => --_[0] < 1 ? 3 : _[0],
            3 => 6 + (++_[0] > 3 ? 1 : _[0]),
            _ => 3 + _[0]
        })
        .Sum();
}
