using System.Text.RegularExpressions;
using AocLib;
using MoreLinq;

namespace _2015.Day06;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = new Grid<int>(1000, 1000);

        var cmds = input
            .SplitLines()
            .Select(x => Regex.Match(x, @"(.*?) (\d+),(\d+) through (\d+),(\d+)"))
            .Select(x => x.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToList())
            .Select(x => (x[0], x.Skip(1).Select(int.Parse).ToList()));
        
        foreach (var (cmd, points) in cmds)
        {
            var (x1, y1, x2, y2) = points;
            
            for (int y = y1; y <= y2; y++)
            for (int x = x1; x <= x2; x++)
            {
                grid[x, y] = cmd switch
                {
                    "turn on" => grid[x, y] + 1,
                    "turn off" => Math.Max(0, grid[x, y] - 1),
                    "toggle" => grid[x, y] + 2,
                    _ => throw new Exception()
                };
            }
        }

        return grid.Sum(x => x.Value);
    }
}
