using MoreLinq.Extensions;

namespace _2025.Day01;

public class Part02 : PuzzleSolver<long>
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
                
                var fullRots = Math.Abs(rot) / 100;
                var partialRots = Math.Abs(rot) % 100;

                var partialZeros = (rot < 0 && acc.Item1 != 0 && partialRots >= acc.Item1)
                                   || (rot > 0 && partialRots >= 100 - acc.Item1) ? 1 : 0;


                var pos = MathEx.Mod(acc.Item1 + rot, 100);
                var zero = fullRots + partialZeros;

                return (pos, acc.Item2 + zero);
            }).Item2;
    }
}