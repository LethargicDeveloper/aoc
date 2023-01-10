using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve() => input
        .Select((c, i) => (i, v: input[i..(i + 14)]))
        .First(_ => _.v.Distinct().Count() == 14).i + 14;
}