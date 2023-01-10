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
        .Select(_ => new[] { _[0] - '@', _[2] - 'W' })
        .Select(_ => _ switch
        {
            [1, 2] => 8,
            [2, 3] => 9,
            [3, 1] => 7,
            _ when _[1] == _[0]  => 3 + _[1],
            _ => _[1]
        })
        .Sum();
}
