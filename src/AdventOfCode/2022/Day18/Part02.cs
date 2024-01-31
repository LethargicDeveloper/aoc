using AocLib;

namespace AdventOfCode._2022.Day18;

// change to Z is up; instead of Y

public partial class Part02 : PuzzleSolver<long>
{
    record MinMax(int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ);

    static bool InRange(Point3D point, MinMax range)
    {
        var (x, y, z) = point;
        return (x >= range.MinX && y >= range.MinY && z >= range.MinZ &&
                x <= range.MaxX && y <= range.MaxY && z <= range.MaxZ);
    }

    static Point3D[] GetNeighbors(Point3D point) =>
        new Point3D[]
        {
            point.MoveRight(),
            point.MoveLeft(),
            point.MoveDown(),
            point.MoveUp(),
            point.MoveForward(),
            point.MoveBackward()
        };

    static MinMax GetMinMaxRanges(HashSet<Point3D> points)
    {
        var minX = points.Min(_ => _.X) - 1;
        var maxX = points.Max(_ => _.X) + 1;
        var minY = points.Min(_ => _.Y) - 1;
        var maxY = points.Max(_ => _.Y) + 1;
        var minZ = points.Min(_ => _.Z) - 1;
        var maxZ = points.Max(_ => _.Z) + 1;
        return new MinMax(minX, maxX, minY, maxY, minZ, maxZ);
    }

    protected override long InternalSolve()
    {
        var points = GetPoints();

        var range = GetMinMaxRanges(points);
        var start = (range.MinX, range.MinY, range.MinZ);
        var seen = new HashSet<Point3D>();
        var stack = new Stack<Point3D>();

        seen.Add(start);
        stack.Push(start);

        while (stack.TryPop(out var point))
        {
            foreach (var neighbor in GetNeighbors(point))
            {
                if (!InRange(neighbor, range)) continue;
                if (seen.Contains(neighbor)) continue;
                if (points.Contains(neighbor))
                {
                    seen.Add(neighbor);
                    continue;
                };

                seen.Add(neighbor);
                stack.Push(neighbor);
            }
        }

        var surfaceArea = 4302;
        int x, y, z;
        for (x = range.MinX; x <= range.MaxX; ++x)
        {
            for (y = range.MinY; y <= range.MaxY; ++y)
            {
                for (z = range.MinZ; z <= range.MaxZ; ++z)
                {
                    var p = (x, y, z);
                    if (seen.Contains(p) || points.Contains(p))
                        continue;

                    var n = GetNeighbors(p);
                    surfaceArea -= n.Where(_ => seen.Contains(_) || points.Contains(_)).Count();
                }
            }
        }

        return surfaceArea;
    }

    HashSet<Point3D> GetPoints() => this.input
        .SplitLines()
        .Select(_ => _.Split(',') switch
        {
            var p => new Point3D(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]))
        }).ToHashSet();
}
