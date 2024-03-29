using AocLib;

namespace _2023.Day18;

// 39039
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = new char[1000][];
        for (int y = 0; y < grid.Length; y++)
        {
            grid[y] = new char[1000];
            Array.Fill(grid[y], '.');
        }

        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(' ') switch
            {
                var x => (cmd: x[0], len: int.Parse(x[1]))
            });

        var pos = new Point(500, 500);

        foreach (var (cmd, len) in cmds)
        {
            for (int i = 0; i < len; ++i)
            {
                grid[pos.Y][pos.X] = '#';

                if (cmd == "R") pos = pos.MoveRight();
                else if (cmd == "L") pos = pos.MoveLeft();
                else if (cmd == "U") pos = pos.MoveUp();
                else if (cmd == "D") pos = pos.MoveDown();
            }

            grid[pos.Y][pos.X] = '#';
        }

        var start = GetStartPoint(grid);

        Fill(grid, start);

        return grid
            .SelectMany()
            .Where(_ => _ == '#' || _ == '*')
            .Count();
    }

    private void Fill(char[][] grid, Point start)
    {
        start = start.MoveRight().MoveDown();

        var queue = new Queue<Point>();
        queue.Enqueue(start);

        while (queue.TryDequeue(out var point))
        {
            if (grid[point.Y][point.X] == '#')
                continue;

            if (grid[point.Y][point.X] == '*')
                continue;

            grid[point.Y][point.X] = '*';

            queue.Enqueue(point.MoveUp());
            queue.Enqueue(point.MoveDown());
            queue.Enqueue(point.MoveLeft());
            queue.Enqueue(point.MoveRight());
        }
    }

    private Point GetStartPoint(char[][] grid)
    {
        for (int y = 0; y < grid.Length; y++)
            for (int x = 0; x < grid[0].Length; x++)
                if (grid[y][x] == '#')
                    return new Point(x, y);

        throw new Exception("No starting poing found.");
    }
}
