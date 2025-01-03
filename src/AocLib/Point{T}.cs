using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AocLib;

public readonly record struct Point<T>(T X, T Y) : ISpanParsable<Point<T>>
    where T : INumber<T>
{
    public Point() : this(T.Zero, T.Zero) {}
    
    public void Deconstruct(out T x, out T y) { x = X;  y = Y; }

    public Point<T> MoveUp() => this + Up;
    public Point<T> MoveRight() => this + Right;
    public Point<T> MoveDown() => this + Down;
    public Point<T> MoveLeft() => this + Left;

    public Point<T> MoveToward(Point<T> point)
    {
        int xSign = T.Sign(point.X - X);
        int ySign = T.Sign(point.Y - Y);
        
        return new(X + Sign(xSign), Y + Sign(ySign));

        static T Sign(int value) => value switch
        {
            > 0 => T.One,
            0 => T.Zero,
            < 0 => -T.One,
        };
    }

    public T ManhattanDistance(Point<T> dest) =>
        T.Abs(X - dest.X) + T.Abs(Y - dest.Y);

    public bool InBounds(T min, T max) =>
        InBounds(min, min, max, max);
    
    public bool InBounds(T xMin, T yMin, T xMax, T yMax) =>
        X >= xMin && X <= xMax && Y >= yMin && Y <= yMax;

    public IEnumerable<Point<T>> OrthogonalNeighbors()
    {
        foreach (var dir in OrthogonalDirections)
            yield return this + dir;
    }
    
    public IEnumerable<Point<T>> DiagonalNeighbors()
    {
        foreach (var dir in DiagonalDirections)
            yield return this + dir;
    }

    public IEnumerable<Point<T>> Neighbors()
    {
        foreach (var dir in Directions)
            yield return this + dir;
    }

    public bool IsOrthogonalNeighborOf(Point<T> point) =>
        OrthogonalNeighbors().Contains(point);
    
    public bool IsDiagonalNeighborOf(Point<T> point) =>
        DiagonalNeighbors().Contains(point);
    
    public bool IsNeighborOf(Point<T> point) =>
        Neighbors().Contains(point);

    public override string ToString() => $"({X}, {Y})";

    public static Point<T> operator +(Point<T> point1, Point<T> point2) =>
        new(point1.X + point2.X, point1.Y + point2.Y);
    
    public static Point<T> operator +(Point<T> point, T value) =>
        new(point.X + value, point.Y + value);
    
    public static Point<T> operator -(Point<T> point1, Point<T> point2) =>
        new(point1.X - point2.X, point1.Y - point2.Y);
    
    public static Point<T> operator -(Point<T> point, T value) =>
        new(point.X - value, point.Y - value);
    
    public static Point<T> operator *(Point<T> point1, Point<T> point2) =>
        new(point1.X * point2.X, point1.Y * point2.Y);
    
    public static Point<T> operator *(Point<T> point, T value) =>
        new(point.X * value, point.Y * value);
    
    public static implicit operator Point<T>((T, T) point) =>
        new(point.Item1, point.Item2);
    
    public static implicit operator (T, T)(Point<T> point) =>
        (point.X, point.Y);

    public static Point<T> TurnRight(Point<T> facing) =>
        InternalTurn(facing, Directions, Right);
    
    public static Point<T> TurnLeft(Point<T> facing) =>
        InternalTurn(facing, Directions, Left);
    
    public static Point<T> TurnRight90(Point<T> facing) =>
        InternalTurn(facing, OrthogonalDirections, Right);
    
    public static Point<T> TurnLeft90(Point<T> facing) =>
        InternalTurn(facing, OrthogonalDirections, Left);

    private static Point<T> InternalTurn(Point<T> facing, Point<T>[] directions, Point<T> turnDirection)
    {
        Debug.Assert(turnDirection == Left || turnDirection == Right,
            $"{nameof(turnDirection)} must be either 'Left' or 'Right'");
        
        if ((facing.X != T.Zero && facing.X != T.One && facing.X != -T.One) &&
            (facing.Y != T.Zero && facing.Y != T.One && facing.Y != -T.One))
        {
            throw new ArgumentException("Invalid facing direction.");
        }
        
        if (facing == Zero) return facing;

        var index = Array.IndexOf(directions, facing);
        
        if (turnDirection == Right)
            index = (index + 1) % directions.Length;
        else
            index = (index - 1 + directions.Length) % directions.Length;
        
        return directions[index];
    }

    public static IEnumerable<Point<T>> GetPointsBetween(Point<T> start, Point<T> end, bool includeDiagonals = false)
    {
        T x = start.X;
        T y = start.Y;

        while (x != end.X || y != end.Y)
        {
            bool changed = x != end.X;
            
            if (x < end.X) x++;
            else if (x > end.X) x--;

            if (!changed || includeDiagonals)
            {
                if (y < end.Y) y++;
                else if (y > end.Y) y--;

            }
            
            if ((x, y) == end)
                yield break;
            
            yield return new Point<T>(x, y);
        }
    }
    
    public static Point<T> Zero { get; } = (T.Zero, T.Zero);
    public static Point<T> Up { get; } = (T.Zero, -T.One);
    public static Point<T> Down { get; } = (T.Zero, T.One);
    public static Point<T> Left { get; } = (-T.One, T.Zero);
    public static Point<T> Right { get; } = (T.One, T.Zero);
    public static Point<T> UpLeft { get; } = (-T.One, -T.One);
    public static Point<T> UpRight { get; } = (T.One, -T.One);
    public static Point<T> DownLeft { get; } = (-T.One, T.One);
    public static Point<T> DownRight { get; } = (T.One, T.One);
    
    public static Point<T>[] OrthogonalDirections { get; } =
        [Up, Right, Down, Left];
    
    public static Point<T>[] DiagonalDirections { get; } =
        [UpLeft, UpRight, DownRight, DownLeft];
    
    public static Point<T>[] Directions { get; } =
        [UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft, Left];

    public static Point<T> Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var point))
            return point;
        
        throw new FormatException($"Unable to parse '{s}'.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Point<T> result)
    {
        result = default;
        
        var numbers = s?.ParseNumbers<T>()[0] ?? [];
        if (numbers.Count != 2)
            return false;

        result = new(numbers[0], numbers[1]);
        return true;
    }

    public static Point<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var point))
            return point;
        
        throw new FormatException($"Unable to parse '{s}'.");
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Point<T> result)
    {
        result = default;
        
        var numbers = s.ToString().ParseNumbers<T>()[0];
        if (numbers.Count != 2)
            return false;

        result = new(numbers[0], numbers[1]);
        return true;
    }
}

public static class PointExtensions
{
    public static IEnumerable<Point<T>> GetDirections<T>(this IEnumerable<Point<T>> points)
        where T : INumber<T>
    {
        var dir = (T.Zero, T.Zero);
        
        yield return dir;

        foreach (var point in points.Window(2).Select(w => w[1] - w[0]))
            yield return point;
    }
}
