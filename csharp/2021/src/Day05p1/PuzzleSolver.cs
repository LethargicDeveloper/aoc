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
        var lines = input
             .SplitLines()
             .Select(ParseLine)
             .Where(_ => _.IsVertical || _.Slope == 0);

        var overlap = new Dictionary<Point, int>();
        foreach (var line in lines)
            foreach (var point in GetPointsOnLine(line))
            {
                if (!overlap.ContainsKey(point))
                    overlap.Add(point, 0);

                overlap[point]++;
            }

        return overlap.Where(_ => _.Value > 1).Count();
    }

    static Line ParseLine(string line)
    {
        var points = line.Split(" -> ");
        return new Line
        {
            Point1 = ParsePoint(points[0]),
            Point2 = ParsePoint(points[1])
        };
    }

    static Point ParsePoint(string point)
    {
        var p = point.Split(',');
        return new Point
        {
            X = int.Parse(p[0]),
            Y = int.Parse(p[1])
        };
    }

    static IEnumerable<Point> GetPointsOnLine(Line line)
    {
        var startX = line.Point1.X;
        var startY = line.Point1.Y;
        var endX = line.Point2.X;
        var endY = line.Point2.Y;
        var incX = (line.IsVertical ? 0 : 1) * (startX < endX ? 1 : -1);
        var incY = (line.IsVertical ? 1 : (int)Math.Abs(line.Slope)) * (startY < endY ? 1 : -1);

        static bool check(int inc, int start, int end) =>
            start <= end ? inc <= end : inc >= end;

        var points = new List<Point>();
        for (int y = startY, x = startX; check(y, startY, endY) && check(x, startX, endX); y += incY, x += incX)
            points.Add(new Point(x, y));

        return points;
    }
}