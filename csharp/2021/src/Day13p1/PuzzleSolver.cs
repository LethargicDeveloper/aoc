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
        var points = new HashSet<Point>();
        var folds = new List<Fold>();

        foreach (var line in input.SplitLines())
        {
            if (line.StartsWith("fold along"))
            {
                var p = line.Split('=');
                folds.Add(new Fold
                (
                    p[0][^1].ToString().ToLower(),
                    int.Parse(p[1])
                ));
            }
            else
            {
                var p = line.Split(',');
                points.Add((int.Parse(p[0]), int.Parse(p[1])));
            }
        }

        var height = points.Select(_ => _.Y).Max();
        var width = points.Select(_ => _.X).Max();

        var fold = folds.First();
        var pointsToFold = points
            .Where(_ => fold.Axis == "x" ? _.X > fold.Position : _.Y > fold.Position)
            .Select(_ => fold.Axis == "x"
                ? new Point(fold.Position - (_.X - (width - fold.Position)), _.Y)
                : new Point(_.X, (fold.Position - (_.Y - (height - fold.Position)))));

        points = points
            .Where(_ => fold.Axis == "x" ? _.X < fold.Position : _.Y < fold.Position)
            .Union(pointsToFold)
            .ToHashSet();

        return points.Count;
    }

    record Fold(string Axis, int Position);
}