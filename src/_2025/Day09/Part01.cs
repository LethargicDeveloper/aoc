using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day09;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var points = input
            .SplitLines()
            .Select(Point<long>.Parse)
            .ToList();

        return (
            from p1 in points
            from p2 in points
            where p1 != p2
            select new Rect<long>(p1, p2).Area
        ).Max();
    }
}
