using System.Numerics;

namespace AocLib;

public class Line<T>(Point<T> point1, Point<T> point2)
    where T : INumber<T>
{
    public Point<T> Point1 { get; } = point1;
    public Point<T> Point2 { get; } = point2;
    
    public override string ToString() => $"({Point1} -> {Point2})";
}