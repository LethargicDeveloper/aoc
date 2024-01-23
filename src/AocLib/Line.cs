using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq.Extensions;
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

    public int MinY => Math.Min(Point1.Y, Point2.Y);
    public int MaxY => Math.Max(Point1.Y, Point2.Y);
    public int MinX => Math.Min(Point1.X, Point2.X);
    public int MaxX => Math.Max(Point1.X, Point2.X);

    public int Slope()
    {
        return TryGetSlope(out var slope)
            ? slope
            : throw new Exception("A vertical line's slope is undefined.");
    }

    public bool TryGetSlope(out int slope)
    {
        if (IsVertical)
        {
            slope = 0;
            return false;
        }

        slope = (Point2.Y - Point1.Y) / (Point2.X - Point1.X);
        return true;
    }

    public int YIntercept()
    {
        return TryGetYIntercept(out var b)
            ? b
            : throw new Exception("A vertical line does not intersect the y-axis.");
    }

    public bool TryGetYIntercept(out int yIntercept)
    {
        if (TryGetSlope(out int slope))
        {
            yIntercept = Point1.Y - (slope * Point1.X);
            return true;
        }

        yIntercept = 0;
        return false;
    }

    public int FindY(int x)
    {
        if (TryGetSlope(out int slope))
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
            for (int y = MinY; y <= MaxY; y++)
                yield return new Point(Point1.X, y);
            yield break;
        }

        var start = Point1.X < Point2.X ? Point1 : Point2;
        var end = Point1.X > Point2.X ? Point1 : Point2;
        
        for (int x = start.X; x <= end.X; x++)
            yield return new Point(x, FindY(x));
    }

    public static bool Intersects(Line line1, Line line2)
        => Intersection(line1, line2).Any();

    public bool Intersects(Line line) => Intersection(line).Any();

    public static IEnumerable<Point> Intersection(Line line1, Line line2)
        => line1.Intersection(line2);

    public IEnumerable<Point> Intersection(Line line)
    {
        //if (IsPoint && line.IsPoint)
        //{
        //    if (Point1 == line.Point1)
        //        yield return Point1;
        //    yield break;
        //}

        //if (IsPoint || line.IsPoint)
        //{
        //    var p = IsPoint ? Point1 : line.Point1;
        //    var l = IsPoint ? line : this;
        //    if (l.Contains(p))
        //        yield return p;
        //    yield break;
        //}

        //bool line1IsVertical = !TryGetSlope(out int slope1);
        //bool line2IsVertical = !line.TryGetSlope(out int slope2);

        //if (line1IsVertical && line2IsVertical)
        //{
        //    if (Point1.X != line.Point1.X)
        //        yield break;

        //    var minY = Math.Max(MinY, line.MinY);
        //    var maxY = Math.Min(MaxY, line.MaxY);
        //    for (int y = minY; y <= maxY; y++)
        //        yield return (Point1.X, y);
        //}

        //if (!line1IsVertical && !line2IsVertical)
        //{
        //    if (GetPoints().All(_ => !line.GetPoints().Contains(_)))
        //        yield break;

        //    var x1min = Math.Min(Point1.X, Point2.X);
        //    var y1min = Math.Min(Point1.Y, Point2.Y);
        //    var x2min = Math.Min(line.Point1.X, line.Point2.X);
        //    var y2min = Math.Min(line.Point1.X, line.Point2.X);

        //    var x1max = Math.Max(Point1.X, Point2.X);
        //    var y1max = Math.Max(Point1.Y, Point2.Y);
        //    var x2max = Math.Max(line.Point1.X, line.Point2.X);
        //    var y2max = Math.Max(line.Point1.X, line.Point2.X);

        //    var newLine = new Line
        //    {
        //        Point1 = new Point(Math.Max(x1min, x2min), Math.Max(y1min, y2min)),
        //        Point2 = new Point(Math.Min(x1max, x2max), Math.Min(y1max, y2max))
        //    };

        //    foreach (var point in newLine.GetPoints())
        //        yield return point;
        //    yield break;
        //}

        var p1s = GetPoints();
        var p2s = line.GetPoints();

        foreach (var p in p1s.Intersect(p2s))
            yield return p;

        //var intersects = ((MinX <= line.MaxX && MaxX >= line.MinX) && (MinY <= line.MaxY && MaxY >= line.MinY))
        //    || ((line.MinX <= MaxX && line.MaxX >= MinX) && (line.MinY <= MaxY && line.MinY >= MaxY));

        //yield return (0, 0);

        //double denominator = ((line.Point2.Y - line.Point1.Y) * (Point2.X - Point1.X)) - ((line.Point2.X - line.Point1.X) * (Point2.Y - Point1.Y));
        //double intersect1 = ((line.Point2.X - line.Point1.X) * (Point1.Y - line.Point1.Y) - (line.Point2.Y - line.Point1.Y) * (Point1.X - line.Point1.X)) / denominator;
        //double intersect2 = ((Point2.X - Point1.X) * (Point1.Y - line.Point1.Y) - (Point2.Y - Point1.Y) * (Point1.X - line.Point1.X)) / denominator;
        //if (intersect1 >= 0 && intersect1 <= 1 && intersect2 >= 0 && intersect2 <= 1)
        //{
        //    int x = (int)(Point1.X + intersect1 * (Point2.X - Point1.X));
        //    int y = (int)(Point1.Y + intersect1 * (Point2.Y - Point1.Y));
        //    yield return new Point(x, y);
        //}
    }

    public bool Contains(Point point)
    {
        if (TryGetSlope(out int slope))
        {
            return point.Y == slope * point.X + YIntercept();
        }

        return point.X == Point1.X && point.Y >= MinY && point.Y <= MaxY;
    }
}
