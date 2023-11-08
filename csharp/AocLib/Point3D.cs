using System.Runtime.CompilerServices;

namespace AocLib;

public readonly record struct Point3D
{
    public static readonly Point3D Zero = new();

    public Point3D(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }
    
    public static Point3D operator +(Point3D p1, Point3D p2) =>
        new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);

    public static Point3D operator -(Point3D p1, Point3D p2) =>
        new(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

    public static implicit operator (int, int, int)(Point3D p) =>
        (p.X, p.Y, p.Z);

    public static implicit operator Point3D((int X, int Y, int Z) p) =>
        new(p.X, p.Y, p.Z);

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

    public Point3D Delta(Point3D point) =>
        (this - point) switch
        {
            var (x, y, z) => (x * -1, y * -1, z * -1)
        };

    public Point3D RotateX(Point3D center, int angle) =>
        RotateX(this, center, angle);

    public static Point3D RotateX(Point3D point, Point3D center, int angle)
    {
        var rad = angle * Math.PI / 180;
        var sin = Math.Sin(rad);
        var cos = Math.Cos(rad);

        var x = point.X - center.X;
        var y = point.Y - center.Y;
        var z = point.Z - center.Z;

        var xnew = x;
        var ynew = y * cos - z * sin;
        var znew = y * sin + z * cos;

        return new Point3D(
            (xnew + center.X),
            (int)(ynew + center.Y),
            (int)(znew + center.Z));
    }

    public Point3D RotateY(Point3D center, int angle) =>
        RotateY(this, center, angle);

    public static Point3D RotateY(Point3D point, Point3D center, int angle)
    {
        var rad = angle * Math.PI / 180;
        var sin = Math.Sin(rad);
        var cos = Math.Cos(rad);

        var x = point.X - center.X;
        var y = point.Y - center.Y;
        var z = point.Z - center.Z;

        var xnew = z * sin + x * cos;
        var ynew = y;
        var znew = z * cos - x * sin;

        return new Point3D(
            (int)(xnew + center.X),
            (ynew + center.Y),
            (int)(znew + center.Z));
    }

    public Point3D RotateZ(Point3D center, int angle) =>
        RotateZ(this, center, angle);

    public static Point3D RotateZ(Point3D point, Point3D center, int angle)
    {
        var rad = angle * Math.PI / 180;
        var sin = Math.Sin(rad);
        var cos = Math.Cos(rad);

        var x = point.X - center.X;
        var y = point.Y - center.Y;
        var z = point.Z - center.Z;

        var xnew = y * sin + x * cos;
        var ynew = y * cos - x * sin;
        var znew = z;

        return new Point3D(
            (int)(xnew + center.X),
            (int)(ynew + center.Y),
            (znew + center.Z));
    }

    public Point3D Up() => (X, Y - 1, Z);
    public Point3D Down() => (X, Y + 1, Z);
    public Point3D Left() => (X - 1, Y, Z);
    public Point3D Right() => (X + 1, Y, Z);
    public Point3D Backward() => (X, Y, Z - 1);
    public Point3D Forward() => (X, Y, Z + 1);
}
