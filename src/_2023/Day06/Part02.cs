using AocLib;

namespace _2023.Day06;

// 28545089
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var splitInput = input
            .SplitLines()
            .Select(_ => long.Parse(_.Split(':')[1].Replace(" ", "")))
            .ToList();

        var race = (t: splitInput[0], d: splitInput[1]);

        long winners = 0;
        for (long b = 0; b <= race.t; ++b)
        {
            var dist = (race.t - b) * b;
            if (dist > race.d)
                winners++;
        }

        return winners;
    }
}
