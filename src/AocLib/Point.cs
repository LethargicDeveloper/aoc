namespace AocLib;

public readonly record struct Point
{
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public static Point operator +(Point p1, Point p2) =>
        new(p1.X + p2.X, p1.Y + p2.Y);
        
    public static Point operator -(Point p1, Point p2) =>
        new(p1.X - p2.X, p1.Y - p2.Y);

    public static Point operator *(Point p1, Point p2) =>
        new(p1.X * p2.X, p1.Y * p2.Y);

    public static Point operator /(Point p1, Point p2) =>
        new(p1.X / p2.X, p1.Y / p2.Y);

    public static implicit operator (int, int)(Point p) => (p.X, p.Y);

    public static implicit operator Point((int, int) p) => new(p.Item1, p.Item2);

    public static int ManhattanDistance(Point p1, Point p2) =>
        Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

    public int ManhattanDistance(Point p)
        => ManhattanDistance(this, p);

    public void Deconstruct(out int x, out int y)
    {
        x = this.X;
        y = this.Y;
    }

    public Point MoveToward(Point p)
        => (X + Math.Sign(p.X - X), Y + Math.Sign(p.Y - Y));

    public Point MoveUp() => (X, Y - 1);
    public Point MoveDown() => (X, Y + 1);
    public Point MoveLeft() => (X - 1, Y);
    public Point MoveRight() => (X + 1, Y);
}
