namespace AocLib;

public readonly struct Rect
{
    public int Left { get; init; }
    public int Right { get; init; }
    public int Top { get; init; }
    public int Bottom { get; init; }

    public bool ContainsScreenPoint(Point point)
        => point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;

    public bool ContainsCartesianPoint(Point point)
        => point.X >= Left && point.X <= Right && point.Y <= Top && point.Y >= Bottom;
}