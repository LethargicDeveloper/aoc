using AocLib;

namespace _2023.Day04;

// 27845
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(_ => _
                .Split(':')[1]
                .Split('|')
                .Select(r => r.S(' '))
                .ToArray())
            .Select(_ => _[1].Intersect(_[0]))
            .Select(_ => (long)Math.Pow(2, _.Count() - 1))
            .Sum();
    }
}
