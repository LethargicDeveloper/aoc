using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day04;

public partial class Part01 : PuzzleSolver<long>
{
    [GeneratedRegex(@"\d+", RegexOptions.Compiled, "en-US")]
    private static partial Regex RangeRegex();

    protected override long InternalSolve() => this.input
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
