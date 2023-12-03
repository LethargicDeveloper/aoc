using AdventOfCode.Abstractions;
using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Day03;

// 549908
public class Part01 : PuzzleSolver<long>
{
    record Part(int X, int Y, string Value);

    bool IsAdjacent(Part part, string[] grid)
    {
        var (x, y, value) = part;
        var points = Enumerable.Range(x, value.Length)
            .Select(_ => new Point(_, y));
        return points.Any(p => p.AdjacentPoints()
            .Any(ap => IsAdjacent(ap, grid)));
    }

    bool IsAdjacent(Point point, string[] grid)
    {
        var x = Math.Clamp(point.X, 0, grid[0].Length - 1);
        var y = Math.Clamp(point.Y, 0, grid.Length - 1);
        var val = grid[y][x];
        return !char.IsDigit(val) && val != '.';
    }

    public override long Solve()
    {
        var grid = input.SplitLines();

        return grid
            .SelectMany((_, i) => Regex.Matches(_, "\\d+")
                .Select(m => new Part(m.Index, i, m.Value)))
            .Where(_ => IsAdjacent(_, grid))
            .Sum(_ => long.Parse(_.Value));
    }
}