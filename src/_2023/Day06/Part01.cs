using AocLib;

namespace _2023.Day06;

// 6209190
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var splitInput = input
            .SplitLines()
            .Select(_ => _
                .S(' ')[1..]
                .Select(int.Parse))
            .ToList();

        var races = splitInput[0].Zip(splitInput[1], (t, d) => (t, d));
        var winners = races.ToDictionary(k => k.t, v => 0);

        foreach (var (t, d) in races)
        {
            for (int b = 0; b <= t; ++b)
            {
                var dist = (t - b) * b;
                if (dist > d)
                    winners[t]++;
            }
        }

        return winners.Values.Product();
    }
}
