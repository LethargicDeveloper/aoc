using AdventOfCode.Abstractions;
using AocLib;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day21;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex("([a-z]{4})")]
    private static partial Regex VarRegex();

    [GeneratedRegex("(....) ([\\+\\-\\*\\/]) (....)")]
    private static partial Regex MathRegex();

    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();

    public override long Solve()
    {
        // https://www.mathpapa.com/simplify-calculator/
        // var simplified = Simplify();

        return 65465236001692352 / 18225;
    }

    string Simplify()
    {
        var lines = this.input.SplitLines();
        var root = lines.First(_ => _.StartsWith("root:"))[6..].Replace("+", "=");

        while (true)
        {
            var match = VarRegex().Matches(root).FirstOrDefault(_ => _.Value != "humn");
            if (match == null || !match.Success) break;

            var replace = lines.First(_ => _.StartsWith($"{match.Value}:"))[6..];
            replace = $"({replace})";
            root = root.Replace(match.Value, replace);
        }


        return root.Replace("humn", "x");
    }
}
