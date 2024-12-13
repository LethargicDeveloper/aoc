using System.Diagnostics;

namespace AocLib;

[DebuggerDisplay("{Point1}-{Point2}")]
public readonly struct Line
{
    public Line() { }
    public Line(Point point1, Point point2)
    {
        Point1 = point1;
        Point2 = point2;
    }

    public Point Point1 { get; init; }
    public Point Point2 { get; init; }

    public bool IsVertical => Point1.X == Point2.X;

    public bool IsPoint => Point1 == Point2;

    public long MinY => Math.Min(Point1.Y, Point2.Y);
    public long MaxY => Math.Max(Point1.Y, Point2.Y);
    public long MinX => Math.Min(Point1.X, Point2.X);
    public long MaxX => Math.Max(Point1.X, Point2.X);

    public long Slope()
    {
        return TryGetSlope(out var slope)
            ? slope
            : throw new Exception("A vertical line's slope is undefined.");
    }

    public bool TryGetSlope(out long slope)
    {
        if (IsVertical)
        {
            slope = 0;
            return false;
        }

        slope = (Point2.Y - Point1.Y) / (Point2.X - Point1.X);
        return true;
    }

    public long YIntercept()
    {
        return TryGetYIntercept(out var b)
            ? b
            : throw new Exception("A vertical line does not intersect the y-axis.");
    }

    public bool TryGetYIntercept(out long yIntercept)
    {
        if (TryGetSlope(out long slope))
        {
            yIntercept = Point1.Y - (slope * Point1.X);
            return true;
        }

        yIntercept = 0;
        return false;
    }

    public long FindY(long x)
    {
        if (TryGetSlope(out long slope))
        {
            return (slope * x) + YIntercept();
        }

        if (x == Point1.X)
            return x;

        throw new Exception($"X={x} will never fall on a vertical line of x={Point1.X}.");
    }

    public IEnumerable<Point> GetPoints()
    {
        if (IsVertical)
        {
            for (long y = MinY; y <= MaxY; y++)
                yield return new Point(Point1.X, y);
            yield break;
        }

        var start = Point1.X < Point2.X ? Point1 : Point2;
        var end = Point1.X > Point2.X ? Point1 : Point2;
        
        for (long x = start.X; x <= end.X; x++)
            yield return new Point(x, FindY(x));
    }

    public static bool Intersects(Line line1, Line line2)
        => Intersection(line1, line2).Any();

    public bool Intersects(Line line) => Intersection(line).Any();

    public static IEnumerable<Point> Intersection(Line line1, Line line2)
        => line1.Intersection(line2);

    public IEnumerable<Point> Intersection(Line line)
    {
        var p1s = GetPoints();
        var p2s = line.GetPoints();

        foreach (var p in p1s.Intersect(p2s))
            yield return p;
    }

    public bool Contains(Point point)
    {
        if (TryGetSlope(out long slope))
        {
            return point.Y == slope * point.X + YIntercept();
        }

        return point.X == Point1.X && point.Y >= MinY && point.Y <= MaxY;
    }
}
