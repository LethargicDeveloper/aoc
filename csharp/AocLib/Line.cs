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

    public double Slope
    {
        get
        {
            var xdiff = (Point2.X - Point1.X);
            return (Point2.Y - Point1.Y) / (xdiff == 0 ? 1 : xdiff);
        }
    }
}
