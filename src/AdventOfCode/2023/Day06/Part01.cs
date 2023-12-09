using AocLib;
using AocLib;

namespace AdventOfCode._2023.Day06;

// 6209190
public class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var splitInput = input
            .SplitLines()
            .Select(_ => _
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..]
                .Select(int.Parse))
            .ToList();

        var races = splitInput[0].Zip(splitInput[1], (t, d) => (t, d));

        var winners = new Dictionary<long, long>();
        foreach (var race in races)
            winners[race.t] = 0;

        foreach (var race in races)
        {
            for (int b = 0; b <= race.t; ++b)
            {
                var dist = (race.t - b) * b;
                if (dist > race.d)
                    winners[race.t]++;
            }
        }

        return winners.Values.Product();
    }
}
