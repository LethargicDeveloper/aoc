namespace AocLib;

public readonly record struct Point3D
{
    public Point3D(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }

    public static implicit operator (int, int, int)(Point3D p) 
        => (p.X, p.Y, p.Z);

    public static implicit operator Point3D((int X, int Y, int Z) p)
      => new(p.X, p.Y, p.Z);

    public void Deconstruct(out int x, out int y)
    {
        x = this.X;
        y = this.Y;
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = this.X;
        y = this.Y;
        z = this.Z;
    }

    public Point3D Up() => (X, Y - 1, Z);
    public Point3D Down() => (X, Y + 1, Z);
    public Point3D Left() => (X - 1, Y, Z);
    public Point3D Right() => (X + 1, Y, Z);
    public Point3D Backward() => (X, Y, Z - 1);
    public Point3D Forward() => (X, Y, Z + 1);
}
