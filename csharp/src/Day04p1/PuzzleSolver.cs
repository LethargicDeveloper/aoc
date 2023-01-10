using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    [GeneratedRegex(@"\d+", RegexOptions.Compiled, "en-US")]
    private static partial Regex RangeRegex();

    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve() => input
        .SplitLines()
        .Select(_ => RangeRegex().Matches(_)
            .Select(m => int.Parse(m.Value))
            .ToArray())
        .Where(InRange)
        .Count();

    bool InRange(int[] r) =>
        (r[0] <= r[2] && r[1] >= r[3]) ||
        (r[2] <= r[0] && r[3] >= r[1]);
}

