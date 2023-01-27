using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input.SplitLines().ToList();
        var length = lines[0].Length;

        var rates = lines
            .Select(_ => Convert.ToInt32(_, 2))
            .ToList();

        var oxygen = FindRating(rates, length - 1);
        var co2 = FindRating(rates, length - 1, co2: true);

        return oxygen * co2;
    }

    int FindRating(List<int> rates, int position, bool co2 = false)
    {
        if (rates.Count == 1)
            return rates[0];

        var bit = (int)Math.Round(Enumerable
            .Range(0, rates.Count)
            .Select(r => (rates[r] >> position) & 1)
            .Average(), 0, MidpointRounding.AwayFromZero);

        if (co2)
            bit = ~bit & ((1 << 1) - 1);

        var newRates = rates
            .Where(_ => ((_ >> position) & 1) == bit)
            .ToList();

        return FindRating(newRates, --position, co2);
    }
}
