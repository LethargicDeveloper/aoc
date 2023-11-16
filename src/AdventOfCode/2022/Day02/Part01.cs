using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day02;

public class Part01 : PuzzleSolver<long>
{
    public override long Solve() => this.input
        .SplitLines()
        .Select(_ => new[] { _[0] - '@', _[2] - 'W' })
        .Select(_ => _ switch
        {
            [1, 2] => 8,
            [2, 3] => 9,
            [3, 1] => 7,
            _ when _[1] == _[0] => 3 + _[1],
            _ => _[1]
        })
        .Sum();
}
