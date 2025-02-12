using AocLib;
using System.Text.RegularExpressions;

namespace _2023.Day03;

// 549908
public class Part01 : PuzzleSolver<long>
{
    record Part(int X, int Y, string Value);

    bool IsAdjacent(string[] grid, Part part)
    {
        var (x, y, value) = part;
        var points = Enumerable.Range(x, value.Length)
            .Select(_ => new Point(_, y));
        return points.Any(p => p.Neighbors()
            .Any(ap => IsAdjacent(grid, ap)));
    }

    bool IsAdjacent(string[] grid, Point point)
    {
        var x = Math.Clamp((int)point.X, 0, grid[0].Length - 1);
        var y = Math.Clamp((int)point.Y, 0, grid.Length - 1);
        var val = grid[y][x];
        return !char.IsDigit(val) && val != '.';
    }

    protected override long InternalSolve()
    {
        var grid = input.SplitLines();

        return grid
            .SelectMany((_, i) => Regex.Matches(_, "\\d+")
                .Select(m => new Part(m.Index, i, m.Value)))
            .Where(_ => IsAdjacent(grid, _))
            .Sum(_ => long.Parse(_.Value));
    }
}