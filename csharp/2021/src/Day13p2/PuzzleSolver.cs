using BenchmarkDotNet.Attributes;
using System.Text;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public string Solve()
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


        foreach (var fold in folds)
        {
            var height = fold.Position * 2;
            var width = fold.Position * 2;

            var pointsToFold = points
                .Where(_ => fold.Axis == "x" ? _.X > fold.Position : _.Y > fold.Position)
                .Select(_ => fold.Axis == "x"
                    ? new Point(fold.Position - (_.X - (width - fold.Position)), _.Y)
                    : new Point(_.X, (fold.Position - (_.Y - (height - fold.Position)))));

            points = points
                .Where(_ => fold.Axis == "x" ? _.X < fold.Position : _.Y < fold.Position)
                .Union(pointsToFold)
                .ToHashSet();
        }

        var h = points.Select(_ => _.Y).Max();
        var w = points.Select(_ => _.X).Max();

        var sb = new StringBuilder();
        for (int y = 0; y <= h; ++y)
        {
            for (int x = 0; x <= w; ++x)
                sb.Append(points.Contains((x, y)) ? "#" : ".");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    record Fold(string Axis, int Position);
}