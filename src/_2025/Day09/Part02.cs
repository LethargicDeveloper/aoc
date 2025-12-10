using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day09;

[Answer(1439894345)]
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var points = input
            .SplitLines()
            .Select(Point<long>.Parse)
            .ToList();

        var polygon = (
            from p1 in points
            from p2 in points
            where p1 != p2 && (p1.X == p2.X || p1.Y == p2.Y)
            select new Line<long>(p1, p2)
        ).DistinctBy(p =>
            string.CompareOrdinal(p.Point1.ToString(), p.Point2.ToString()) < 1
                ? (p.Point1, p.Point2)
                : (p.Point2, p.Point1))
        .ToList();

        var rects = (
            from p1 in points
            from p2 in points
            where p1 != p2
            select (Point1: p1, Point2: p2)
        ).DistinctBy(p => string.CompareOrdinal(p.Point1.ToString(), p.Point2.ToString()) < 1 
                ? (p.Point1, p.Point2)
                : (p.Point2, p.Point1))
        .Select(p => new Rect<long>(p.Point1, p.Point2));

        var validRects = rects
            .Where(rect => !polygon.Any(line => Overlap(rect, line)));
        
        return validRects.Max(r => r.Area);

        bool Overlap(Rect<long> rect, Line<long> line)
            => rect.Top < MathEx.Max(line.Point1.Y, line.Point2.Y) &&
               rect.Bottom > MathEx.Min(line.Point1.Y, line.Point2.Y) &&
               rect.Left < MathEx.Max(line.Point1.X, line.Point2.X) &&
               rect.Right > MathEx.Min(line.Point1.X, line.Point2.X);
    }
}