using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day14;

public class Part01 : PuzzleSolver<long>
{
    private const long TIME = 100;
    private const long WIDTH = 101;
    private const long HEIGHT = 103;
    
    protected override long InternalSolve()
    {
        var safetyFactor = input
            .SplitLines()
            .Select(l => Regex.Matches(l, @"[-]?\d+")
                    .Select(m => int.Parse(m.Value))
                    .ToList() switch
                {
                    [var px, var py, var vx, var vy] =>
                        (Pos: new Point<long>(px, py), Velocity: new Point<long>(vx, vy)),
                    _ => throw new InvalidOperationException()
                })
            .Select(v => v.Pos + (v.Velocity * TIME))
            .Select(v => new Point<long>(MathEx.Mod(v.X, WIDTH), MathEx.Mod(v.Y, HEIGHT)))
            .Where(p => p.X != WIDTH / 2 && p.Y != HEIGHT / 2)
            .GroupBy(p =>
            { 
                if (p.InBounds(0, 0, WIDTH / 2 - 1, HEIGHT / 2 - 1))
                    return 0;
                if (p.InBounds(WIDTH / 2 + 1, 0, WIDTH, HEIGHT / 2 - 1))
                    return 1;
                if (p.InBounds(0, HEIGHT / 2 + 1, WIDTH / 2 - 1, HEIGHT - 1))
                    return 2;
                return 3;
            })
            .Select(g => g.Count())
            .Product();

        return safetyFactor;
    }
}
