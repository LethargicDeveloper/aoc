using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day05;

static partial class Extensions
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumRegex();

    public static List<Stack<char>> ParseStacks(this string input) => input
        .SplitLines()
        .Reverse()
        .Skip(1)
        .SelectMany(_ => _.WithIndex().Where(_ => (_.Index - 1) % 4 == 0))
        .GroupBy(_ => _.Index)
        .Select(_ => new Stack<char>().PushRange(_.Select(c => c.Value).Where(_ => _ != ' ')))
        .ToList();

    public static List<List<int>> ParseMoves(this string input) => input
        .SplitLines()
        .Select(_ => NumRegex().Matches(_)
            .Select(m => int.Parse(m.Value))
            .ToList())
        .ToList();
}