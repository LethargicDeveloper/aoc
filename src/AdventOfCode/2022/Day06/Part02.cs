using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day06;

public partial class Part02 : PuzzleSolver<long>
{
    public override long Solve() => this.input
        .Select((c, i) => (i, v: this.input[i..(i + 14)]))
        .First(_ => _.v.Distinct().Count() == 14).i + 14;
}
