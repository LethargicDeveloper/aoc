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

        var gamma = Enumerable
            .Range(0, length)
            .Select(c => (int)Math.Round(Enumerable
                .Range(0, lines.Count)
                .Select(r => (rates[r] >> (length - 1) - c) & 1)
                .Average(), 0, MidpointRounding.AwayFromZero))
            .Aggregate(0, (acc, cur) => (acc << 1) + cur);

        var epsilon = ~gamma & ((1 << length) - 1);

        return gamma * epsilon;
    }
}
