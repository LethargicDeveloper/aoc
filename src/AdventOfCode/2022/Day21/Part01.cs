using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day21;

public partial class Part01 : PuzzleSolver<long>
{
    [GeneratedRegex("(....) ([\\+\\-\\*\\/]) (....)")]
    private static partial Regex MathRegex();

    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();

    protected override long InternalSolve()
    {
        var builder = new CodeBuilder();
        
        foreach (var line in this.input.SplitLines())
        {
            var index = line.IndexOf(":");
            var name = line[..index];

            var outer = line[(index + 2)..];
            var body = MathRegex().Replace(outer, m => $"return {m.Groups[1].Value}() {m.Groups[2].Value} {m.Groups[3].Value}();");
            body = NumberRegex().Replace(body, m => $"return {m.Groups[1].Value};");

            builder.AddFunction(name, body, "long");
        }

        return (long)builder.Build().root();
    }
}
