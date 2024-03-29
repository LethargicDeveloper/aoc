using AocLib;

namespace _2023.Day15;

// 517315
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var hash = input
            .Split(',')
            .Select(_ => _
                .Aggregate(0, (cur, acc) => (acc + cur) * 17 % 256))
            .Sum();

        return hash;
    }
}
