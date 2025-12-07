using AocLib;
using MoreLinq;

namespace _2015.Day02;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(x => x.Split('x').Select(long.Parse).ToArray())
            .Select(x =>
            {
                x.Sort();
                return (2 * x[0]) + (2 * x[1]) + x.Product();
            })
            .Sum();
    }
}
