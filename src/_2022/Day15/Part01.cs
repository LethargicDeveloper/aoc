﻿using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day15;

public partial class Part01 : PuzzleSolver<long>
{
    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumberRegex();

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
            .Select(Parse);

        var area = DetectedArea(points, row: 2000000).OrderBy(_ => _.X);

        return area.Count();
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
