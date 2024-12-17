namespace AocLib;

public class Rect
{
    public long Left { get; init; }
    public long Right { get; init; }
    public long Top { get; init; }
    public long Bottom { get; init; }

    public bool ContainsScreenPoint(Point point)
        => point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;

    public bool ContainsCartesianPoint(Point point)
        => point.X >= Left && point.X <= Right && point.Y <= Top && point.Y >= Bottom;
}