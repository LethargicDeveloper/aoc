using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day04;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex(@"\d+", RegexOptions.Compiled, "en-US")]
    private static partial Regex RangeRegex();

    public override long Solve() => this.input
        .SplitLines()
        .Select(_ => RangeRegex().Matches(_)
            .Select(m => int.Parse(m.Value))
            .ToArray())
        .Where(Overlap)
        .Count();

    bool Overlap(int[] r) =>
        (r[0] <= r[3] && r[1] >= r[2]) ||
        (r[3] <= r[0] && r[2] >= r[1]);
}
