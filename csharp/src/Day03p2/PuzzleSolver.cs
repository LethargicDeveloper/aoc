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
        .Chunk(3)
        .SelectMany(_ => _
            .Aggregate((acc, cur) => acc.Intersect(cur).CreateString())
            .Select(_ => char.IsLower(_) ? _ - 96 : _ - 38))
        .Sum();
}
