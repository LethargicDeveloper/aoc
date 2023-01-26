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
    public long Solve() => input
        .SplitLines()
        .Select(_ => new[]
        {
            _[..(_.Length / 2)],
            _[^(_.Length / 2)..]
        })
        .Select(_ => _[0].First(v => _[1].Contains(v)))
        .Select(_ => char.IsLower(_) ? _ - 96 : _ - 38)
        .Sum();
}
