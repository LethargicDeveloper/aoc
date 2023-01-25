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
        var points = this.input
            .SplitLines()
            .Select(Parse)
            .Select(_ => (_.Sensor, Dist: _.Sensor.ManhattanDistance(_.Beacon)))
            .ToList();

        var beacon = FindBeacon(points, max: 4000000);
        var tuning = TuningFrequency(beacon);

        return tuning;
    }

    (Point Sensor, Point Beacon) Parse(string input) =>
        NumberRegex().Matches(input).ToArray() switch
        {
            var a => (
                (X: int.Parse(a[0].Value), Y: int.Parse(a[1].Value)),
                (X: int.Parse(a[2].Value), Y: int.Parse(a[3].Value))
            )
        };

    static Point FindBeacon(List<(Point Sensor, int Dist)> points, int max)
    {
        for (int y = 0; y < max; ++y)
        {
            var area = new List<(int sx, int ex)>();
            foreach (var point in points)
            {
                var (sensor, dist) = point;
                var offset = Math.Abs(y - sensor.Y);
                var startX = Math.Clamp((sensor.X - dist) + offset, 0, max);
                var endX = Math.Clamp((sensor.X + dist) - offset, 0, max);
                area.Add((startX, endX));
            }

            area.Sort();
            int cur = 0;
            for (int x = 0; x < area.Count; ++x)
            {
                var (sx, ex) = area[x];
                if (sx <= cur && ex > cur)
                {
                    cur = ex + 1;
                }
                else if (sx > cur && sx < max)
                {
                    return (sx - 1, y);
                }
            }
        }

        return (0, 0);
    }

    public static long TuningFrequency(Point p) => (p.X * 4000000L) + p.Y;

    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumberRegex();
}