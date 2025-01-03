using AocLib;
using System.Text.RegularExpressions;

namespace _2023.Day03;

// 81166799
public class Part02 : PuzzleSolver<long>
{
    record Part(int X, int Y, string Value);

    long GearRatio(Point star, Part[] parts)
    {
        var adj = parts
            .Where(part => Enumerable.Range(part.X, part.Value.Length)
                .Select(_ => new Point(_, part.Y))
                .Any(_ => _.IsNeighborOf(star)))
            .ToArray();

        if (adj.Length != 2) return 0;
        return long.Parse(adj[0].Value) * long.Parse(adj[1].Value);
    }

    protected override long InternalSolve()
    {
        var grid = input.SplitLines();

        var parts = grid
            .SelectMany((_, i) => Regex.Matches(_, "\\d+")
                .Select(m => new Part(m.Index, i, m.Value)))
            .ToArray();

        var stars = grid
            .SelectMany((_, i) => Regex.Matches(_, "\\*")
                .Select(m => new Point(m.Index, i)));

        return stars
            .Select(_ => GearRatio(_, parts))
            .Sum();
    }
}