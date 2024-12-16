using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day15;

public partial class Part02 : PuzzleSolver<long>
{
    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumberRegex();

    static Point FindBeacon(List<(Point Sensor, long Dist)> points, int max)
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
                area.Add(((int)startX, (int)endX));
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

    static long TuningFrequency(Point p) => (p.X * 4000000L) + p.Y;

    static HashSet<Point> DetectedArea(IEnumerable<(Point Sensor, Point Beacon)> points, int row)
    {
        var area = new HashSet<Point>();
        foreach (var point in points)
        {
            var (sensor, beacon) = point;
            var dist = sensor.ManhattanDistance(beacon);
            var offset = Math.Abs(row - sensor.Y);
            var startX = (sensor.X - dist) + offset;
            var endX = (sensor.X + dist) - offset;
            for (int x = (int)startX; x < endX; ++x)
            {
                area.Add((x, row));
            }
        }

        return area;
    }

    protected override long InternalSolve()
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
}
