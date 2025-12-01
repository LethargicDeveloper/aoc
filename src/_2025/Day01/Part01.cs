using MoreLinq.Extensions;

namespace _2025.Day01;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Aggregate((50, 0), (acc, cur) =>
            {
                var rot = cur[0] == 'L'
                    ? -int.Parse(cur[1..])
                    : int.Parse(cur[1..]);

                var pos = MathEx.Mod(acc.Item1 + rot, 100);

                return (pos, pos == 0 ? acc.Item2 + 1 : acc.Item2);
            }).Item2;
    }
}

