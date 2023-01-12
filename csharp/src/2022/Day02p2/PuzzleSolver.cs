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
        .Select(_ => _[1] switch
        {
            1 => --_[0] < 1 ? 3 : _[0],
            3 => 6 + (++_[0] > 3 ? 1 : _[0]),
            _ => 3 + _[0]
        })
        .Sum();
}
