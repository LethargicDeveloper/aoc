using AocLib;
using MoreLinq;

namespace _2015.Day01;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .Select(x => x == '(' ? 1 : -1)
            .WithIndex()
            .Scan((Value: 1, Index: -1), (acc, cur) => (acc.Value + cur.Value, cur.Index))
            .First(x => x.Value == -1).Index;
    }
}
