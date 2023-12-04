using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2023.Day04;

// 27845
public class Part01 : PuzzleSolver<long>
{
    long Calculate(IEnumerable<string> str)
        => str.Count() switch
        {
            0 => 0,
            1 => 1,
            var c => (long)Math.Pow(2, c - 1)
        };

    public override long Solve()
    {
        return input
            .SplitLines()
            .Select(_ => _
                .Split(':')[1]
                .Split('|')
                .Select(r => r.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray())
            .Select(_ => _[1].Intersect(_[0]))
            .Select(Calculate)
            .Sum();
    }
}
