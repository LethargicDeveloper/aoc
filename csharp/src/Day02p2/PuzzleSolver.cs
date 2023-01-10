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
        .Select(_ => (p1: _[0] - '@', p2: _[2] - 'W'))
        .Select(_ => _.p2 switch
        {
            1 => --_.p1 < 1 ? 3 : _.p1,
            3 => 6 + (++_.p1 > 3 ? 1 : _.p1),
            _ => 3 + _.p1
        })
        .Sum();
}
