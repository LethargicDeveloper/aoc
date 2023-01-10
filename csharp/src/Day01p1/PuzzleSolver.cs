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
        .SplitEmptyLines()
        .Select(_ => _.SplitLines()
            .Select(int.Parse)
            .Sum())
        .Max();
}
