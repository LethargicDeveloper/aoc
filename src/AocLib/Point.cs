namespace AocLib;

public readonly record struct Point
{
    public Point(long x, long y)
    {
        this.X = x;
        this.Y = y;
    }

    public long X { get; init; }
    public long Y { get; init; }

    public static Point operator +(Point p1, Point p2) =>
        new(p1.X + p2.X, p1.Y + p2.Y);
        
    public static Point operator -(Point p1, Point p2) =>
        new(p1.X - p2.X, p1.Y - p2.Y);

    public static Point operator *(Point p1, Point p2) =>
        new(p1.X * p2.X, p1.Y * p2.Y);
    
    public static Point operator *(Point p1, long p2) =>
        new(p1.X * p2, p1.Y * p2);

    public static Point operator /(Point p1, Point p2) =>
        new(p1.X / p2.X, p1.Y / p2.Y);

    public static implicit operator (long, long)(Point p) => (p.X, p.Y);

    public static implicit operator Point((long, long) p) => new(p.Item1, p.Item2);

    public static long ManhattanDistance(Point p1, Point p2) =>
        Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

    public long ManhattanDistance(Point p)
        => ManhattanDistance(this, p);

    public bool IsOrthoganllyAdjacentTo(Point point)
        => OrthogonalAdjacentPoints().Contains(point);

    public bool IsDiagonallyAdjacentTo(Point point)
        => DiagonalAdjacentPoints().Contains(point);

    public bool IsAdjacentTo(Point point)
        => IsOrthoganllyAdjacentTo(point) || IsDiagonallyAdjacentTo(point);

    public IEnumerable<Point> AdjacentPoints()
        => DiagonalAdjacentPoints().Concat(OrthogonalAdjacentPoints());

    public IEnumerable<Point> DiagonalAdjacentPoints()
    {
        return
        [
            (X - 1, Y - 1),
            (X + 1, Y - 1),
            (X - 1, Y + 1),
            (X + 1, Y + 1)
        ];
    }

    public IEnumerable<Point> OrthogonalAdjacentPoints()
    {
        return
        [
            (X - 1, Y),
            (X + 1, Y),
            (X, Y - 1),
            (X, Y + 1)
        ];
    }

    public bool InBounds(long min, long max)
        => InBounds(min, min, max, max);

    public bool InBounds(long xMin, long yMin, long xMax, long yMax)
        => X >= xMin && Y >= yMin && X <= xMax && Y <= yMax;

    public bool InBounds(Func<Point, bool> predicate)
        => predicate(this);

    public void Deconstruct(out long x, out long y)
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
    
    public static Point Up => (0, -1);
    public static Point Down => (0, 1);
    public static Point Left => (-1, 0);
    public static Point Right => (1, 0);
    public static Point Zero => (0, 0);

    public static Point TurnLeft(Point facing) => facing switch
    {
        (0, -1) => (-1, 0),
        (-1, 0) => (0, 1),
        (0, 1) => (1, 0),
        (1, 0) => (0, -1),
        _ => (0, 0)
    };
    
    public static Point TurnRight(Point facing) => facing switch
    {
        (0, -1) => (1, 0),
        (1, 0) => (0, 1),
        (0, 1) => (-1, 0),
        (-1, 0) => (0, -1),
        _ => (0, 0)
    };
    
    // public Point TurnToward(Point facing, Point dest)
    // {
    //     if (facing == (0, 0))
    //         return facing;
    //     
    //     var dx = X - dest.X;
    //     var dy = Y - dest.Y;
    //
    //     int ndx = 0, ndy = 0;
    //     if (Math.Abs(dx) > Math.Abs(dy))
    //         ndx = dx > 0 ? 1 : -1;
    //     else
    //         ndy = dy > 0 ? -1 : 1;
    //
    //     var dirs = new[] { Down,  Right, Up, Left };
    //
    //     int ix = Array.IndexOf(dirs, facing);
    //     int newIx = Array.IndexOf(dirs, (ndx, ndy));
    //
    //     int clockwise = (newIx - ix + 4) % 4;
    //     int counterClockwise = (ix - newIx + 4) % 4;
    //
    //     //return clockwise <= counterClockwise ? dirs[newIx] : dirs[ix];
    //     return clockwise <= counterClockwise ? dirs[newIx] : dirs[(ix - counterClockwise + 4) % 4];
    // }
    
    [Obsolete]
    public static Point RotateRight90(Point dir) => dir switch
    {
        (0, -1) => (1, 0),
        (1, 0) => (0, 1),
        (0, 1) => (-1, 0),
        (-1, 0) => (0, -1),
        _ => (0, 0)
    };
}
