using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day09;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var points = input
            .SplitLines()
            .Select(Point<long>.Parse)
            .ToList();

        var pairs = (
            from p1 in points
            from p2 in points
            where p1 != p2
            select (Point1: p1, Point2: p2)
        ).DistinctBy(p => Distinct(p.Point1, p.Point2))
        .ToList();

        var polygon = pairs
            .Where(p => p.Point1.X == p.Point2.X || p.Point1.Y == p.Point2.Y)
            .Select(p => new Line<long>(p.Point1, p.Point2))
            .ToList();

        return pairs
            .Select(p => new Rect<long>(p.Point1, p.Point2))
            .OrderByDescending(r => r.Area)
            .First(rect => !polygon.Any(line => Overlap(rect, line)))
            .Area;

        (Point<long>, Point<long>) Distinct(Point<long> p1, Point<long> p2)
            =>  p1.GetHashCode() < p2.GetHashCode() ? (p1, p2) : (p2, p1);
        
        bool Overlap(Rect<long> rect, Line<long> line)
            => rect.Top < MathEx.Max(line.Point1.Y, line.Point2.Y) &&
               rect.Bottom > MathEx.Min(line.Point1.Y, line.Point2.Y) &&
               rect.Left < MathEx.Max(line.Point1.X, line.Point2.X) &&
               rect.Right > MathEx.Min(line.Point1.X, line.Point2.X);
    }
}