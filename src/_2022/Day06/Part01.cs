using AocLib;

namespace _2022.Day06;

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve() => input
        .Select((c, i) => (i, v: input[i..(i + 4)]))
        .First(_ => _.v.Distinct().Count() == 4).i + 4;
}
