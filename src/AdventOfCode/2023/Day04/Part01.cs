using AocLib;
using AocLib;

namespace AdventOfCode._2023.Day04;

// 27845
public class Part01 : PuzzleSolver<long>
{
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
            .Select(_ => (long)Math.Pow(2, _.Count() - 1))
            .Sum();
    }
}
