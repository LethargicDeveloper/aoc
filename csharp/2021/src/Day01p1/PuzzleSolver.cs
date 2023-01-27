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
        var depths = input.SplitLines().Select(int.Parse).ToList();
        return depths.Skip(1).Where((d, i) => d > depths[i]).Count();
    }
}
