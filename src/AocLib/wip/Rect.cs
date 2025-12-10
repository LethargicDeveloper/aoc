using System.Numerics;

namespace AocLib;

public class Rect<T>
    where T : INumber<T>
{
    public Rect(Point<T> corner1, Point<T> corner2)
    {
        Left = MathEx.Min(corner1.X, corner2.X);
        Right = MathEx.Max(corner1.X, corner2.X);
        Top = MathEx.Min(corner1.Y, corner2.Y);
        Bottom = MathEx.Max(corner1.Y, corner2.Y);
    }
    
    public T Left { get; }
    public T Right { get; }
    public T Top { get; }
    public T Bottom { get; }

    public Point<T> TopLeft => (Left, Top);
    public Point<T> TopRight => (Right, Top);
    public Point<T> BottomLeft => (Left, Bottom);
    public Point<T> BottomRight => (Right, Bottom);

    public bool ContainsScreenPoint(Point<T> point)
        => point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;

    public bool ContainsCartesianPoint(Point<T> point)
        => point.X >= Left && point.X <= Right && point.Y <= Top && point.Y >= Bottom;
    
    public T Area => (Right - Left + T.One) * (Bottom - Top + T.One);
    
    public override string ToString() => $"[({Left}, {Top}) -> ({Right}, {Bottom})]";
}