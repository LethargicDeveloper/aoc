using AocLib;

namespace AdventOfCode._2022.Day14;

public partial class Part02 : PuzzleSolver<long>
{
    static int CalcYMax(char[][] grid) => grid.Select((_, i) => (i, v: Array.IndexOf(_, '#'))).Last(_ => _.v != -1).i + 1;

    protected override long InternalSolve()
    {
        var grid = ParseInput();

        var spawnPos = (Y: 0, X: 500);
        var gridBottom = CalcYMax(grid) + 1;
        for (int x = 0; x < grid[0].Length; ++x)
            grid[gridBottom][x] = '#';

        var sandPos = spawnPos;
        int sandCount = 1;
        while (true)
        {
            if ("#O".Contains(grid[sandPos.Y + 1][sandPos.X]))
            {
                if ("#O".Contains(grid[sandPos.Y + 1][sandPos.X - 1]))
                {
                    if ("#O".Contains(grid[sandPos.Y + 1][sandPos.X + 1]))
                    {
                        grid[sandPos.Y][sandPos.X] = 'O';
                        if (sandPos == spawnPos) break;
                        sandPos = spawnPos;
                        sandCount++;
                        continue;
                    }
                    else
                    {
                        sandPos = (sandPos.Y + 1, sandPos.X + 1);
                    }
                }
                else
                {
                    sandPos = (sandPos.Y + 1, sandPos.X - 1);
                }
            }
            else
            {
                sandPos = (sandPos.Y + 1, sandPos.X);
            }
        }

        return sandCount;
    }

    char[][] ParseInput()
    {
        var grid = new char[1000][];
        for (int y = 0; y < grid.Length; ++y)
        {
            grid[y] = new char[1000];
            for (int x = 0; x < grid[0].Length; ++x)
                grid[y][x] = '.';
        }
        grid[0][500] = '+';

        var points = this.input
            .SplitLines()
            .Select(line => line
                .Split(" -> ")
                .Select(_ => _.Split(",") switch { var a => (Y: int.Parse(a[1]), X: int.Parse(a[0])) })
                .ToArray())
            .ToArray();

        foreach (var point in points)
        {
            for (int i = 0; i < point.Length - 1; ++i)
            {
                var yStart = Math.Min(point[i].Y, point[i + 1].Y);
                var yRange = Enumerable.Range(yStart, Math.Abs(point[i + 1].Y - point[i].Y) + 1).ToArray();

                var xStart = Math.Min(point[i].X, point[i + 1].X);
                var xRange = Enumerable.Range(xStart, Math.Abs(point[i + 1].X - point[i].X) + 1).ToArray();

                for (int y = yRange[0]; y <= yRange[^1]; ++y)
                {
                    for (int x = xRange[0]; x <= xRange[^1]; ++x)
                    {
                        grid[y][x] = '#';
                    }
                }
            }
        }

        return grid;
    }
}
