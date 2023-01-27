using BenchmarkDotNet.Attributes;
using System.Text;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input.SplitLines();
        var template = lines.First();
        var pairs = lines
            .Skip(1)
            .Select(_ => _.Split(" -> "))
            .ToDictionary(_ => _[0], _ => _[1]);

        var polymers = template
            .Skip(1)
            .Select((_, i) => $"{template[i]}{_}")
            .GroupBy(_ => _)
            .ToDictionary(_ => _.Key, _ => _.LongCount());

        for (int step = 0; step < 40; ++step)
        {
            var newPolymers = new Dictionary<string, long>();
            foreach (var polymer in polymers)
            {
                if (pairs.TryGetValue(polymer.Key, out string? replace))
                {
                    var amount = polymers[polymer.Key];
                    polymers[polymer.Key] = 0;

                    var newPolymer1 = $"{polymer.Key[0]}{replace}";
                    var newPolymer2 = $"{replace}{polymer.Key[1]}";

                    if (!newPolymers.TryAdd(newPolymer1, amount))
                    {
                        newPolymers[newPolymer1] += amount;
                    }

                    if (!newPolymers.TryAdd(newPolymer2, amount))
                    {
                        newPolymers[newPolymer2] += amount;
                    }
                }
            }

            foreach (var newPolymer in newPolymers)
            {
                if (!polymers.TryAdd(newPolymer.Key, newPolymer.Value))
                {
                    polymers[newPolymer.Key] += newPolymer.Value;
                }
            }
        }

        var letterValues = polymers
            .SelectMany(_ => _.Key.Select(k => (key: k, val: _.Value)))
            .GroupBy(_ => _.key)
            .Select(_ => (key: _.Key, val: _.Sum(_ => _.val)));

        var max = (long)Math.Round(letterValues.MaxBy(_ => _.val).val / 2.0, MidpointRounding.AwayFromZero);
        var min = (long)Math.Round(letterValues.MinBy(_ => _.val).val / 2.0, MidpointRounding.AwayFromZero);
        
        return max - min;
    }
}