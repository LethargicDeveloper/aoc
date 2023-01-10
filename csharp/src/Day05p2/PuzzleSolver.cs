using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public string Solve()
    {
        var (stacks, moves) = input
            .SplitEmptyLines(_ =>
            (
                _[0].ParseStacks(),
                _[1].ParseMoves()
            ));

        return moves.Aggregate(stacks, (acc, cur) =>
        {
            acc[cur[2] - 1].PushRange(
                acc[cur[1] - 1].PopRange(cur[0]).Reverse());
            return acc;
        })
        .Select(_ => _.Pop())
        .CreateString();
    }
}

static partial class Extensions
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumRegex();

    public static List<Stack<char>> ParseStacks(this string input) => input
        .SplitLines()
        .Reverse()
        .Skip(1)
        .SelectMany(_ => _.Select((v, i) => (i, v)).Where(_ => (_.i - 1) % 4 == 0))
        .GroupBy(_ => _.i)
        .Select(_ => new Stack<char>().PushRange(_.Select(c => c.v).Where(_ => _ != ' ')))
        .ToList();

    public static List<List<int>> ParseMoves(this string input) => input
        .SplitLines()
        .Select(_ => NumRegex().Matches(_)
            .Select(m => int.Parse(m.Value))
            .ToList())
        .ToList();
}