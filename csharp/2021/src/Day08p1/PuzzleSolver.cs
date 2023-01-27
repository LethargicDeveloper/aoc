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
    public long Solve()
    {
        var inputs = input
            .SplitLines()
            .Select(_ =>
            {
                var entry = _.Split(" | ");
                var signals = entry[0].Split(' ');
                var output = entry[1].Split(' ');
                return (signals, output);
            }).ToList();

        return inputs
            .SelectMany(_ => _.output.Select(o => o.Length switch
            {
                2 => 1,
                4 => 1,
                3 => 1,
                7 => 1,
                _ => 0
            })).Sum();
    }
}