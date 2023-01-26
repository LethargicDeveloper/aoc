using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var points = GetPoints();
        var surfaceArea = points.Count * 6;

        var seen = new HashSet<Point3D>();
        foreach (var point in points)
        {
            var (x, y, z) = point;
            var adjacent = new Point3D[]
            {
                point.Right(),
                point.Left(),
                point.Down(),
                point.Up(),
                point.Forward(),
                point.Backward(),
            };

            var adjacentCount = seen.Where(adjacent.Contains).Count();
            surfaceArea -= (adjacentCount * 2);

            seen.Add(point);
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