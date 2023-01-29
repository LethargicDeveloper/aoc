using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

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
        var target = Parse(input);
        var xVelocities = GenerateXVelocities();
        var yVelocities = GenerateYVelocities();
        var velocities =
            from x in xVelocities
            from y in yVelocities
            select new Point(x, y);

        var launcher = new Launcher(target);
        var hits = new List<Point>();
        foreach (var velocity in velocities)
        {
            var result = launcher.Launch(velocity);

            if (result.Hit)
                hits.Add(velocity);
        }

        return hits.Count;
    }

    static IEnumerable<int> GenerateXVelocities() => Enumerable.Range(1, 300);
    static IEnumerable<int> GenerateYVelocities() => Enumerable.Range(-400, 800);

    static Rect Parse(string file)
    {
        var match = NumbersRegex().Match(file);
        var x1 = int.Parse(match.Groups["x1"].Value);
        var x2 = int.Parse(match.Groups["x2"].Value);
        var y1 = int.Parse(match.Groups["y1"].Value);
        var y2 = int.Parse(match.Groups["y2"].Value);

        return new Rect
        {
            Left = x1,
            Top = y2,
            Right = x2,
            Bottom = y1
        };
    }

    [GeneratedRegex("x=(?<x1>-?\\d+)\\.\\.(?<x2>-?\\d+), y=(?<y1>-?\\d+)..(?<y2>-?\\d+)")]
    private static partial Regex NumbersRegex();
}

record LauncherResult(bool Hit, int MaxHeight);

class Launcher
{
    readonly Rect target;

    public Launcher(Rect target)
    {
        this.target = target;
    }

    public LauncherResult Launch(Point velocity)
    {
        var origin = new Point(0, 0);
        bool hit = false;
        int maxHeight = 0;

        var currPos = origin;
        int maxY = 0;
        while (true)
        {
            if (target.ContainsCartesianPoint(currPos))
            {
                hit = true;
                maxHeight = maxY;
                break;
            }

            if (velocity.X == 0 && currPos.X < target.Left)
                break;

            if (velocity.Y < 0 && currPos.Y < target.Bottom)
                break;

            ApplyVelocity(ref currPos, ref velocity);
            maxY = Math.Max(maxY, currPos.Y);
        }

        return new LauncherResult(hit, maxHeight);
    }

    static void ApplyVelocity(ref Point position, ref Point velocity)
    {
        position += velocity;
        UpdateVelocity(ref velocity);
    }

    static void UpdateVelocity(ref Point velocity)
    {
        velocity = velocity with
        {
            X = velocity.X switch
            {
                > 0 => velocity.X - 1,
                < 0 => velocity.X + 1,
                0 => velocity.X,
            },
            Y = velocity.Y - 1,
        };
    }
}