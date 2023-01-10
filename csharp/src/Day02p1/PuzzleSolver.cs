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
        .Select(_ => _ switch
        {
            (1, 2) => 8,
            (2, 3) => 9,
            (3, 1) => 7,
            _ when _.p2 == _.p1 => 3 + _.p2,
            _ => _.p2
        })
        .Sum();
}
