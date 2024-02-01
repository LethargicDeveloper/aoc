using AocLib;

namespace _2022.Day18;

// change to Z is up; instead of Y

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var points = GetPoints();
        var surfaceArea = points.Count * 6;

        var seen = new HashSet<Point3D>();
        foreach (var point in points)
        {
            var (x, y, z) = point;
            var adjacent = new Point3D[]
            {
                point.MoveRight(),
                point.MoveLeft(),
                point.MoveDown(),
                point.MoveUp(),
                point.MoveForward(),
                point.MoveBackward(),
            };

            var adjacentCount = seen.Where(adjacent.Contains).Count();
            surfaceArea -= adjacentCount * 2;

            seen.Add(point);
        }

        return surfaceArea;
    }

    HashSet<Point3D> GetPoints() => input
        .SplitLines()
        .Select(_ => _.Split(',') switch
        {
            var p => new Point3D(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]))
        }).ToHashSet();
}
