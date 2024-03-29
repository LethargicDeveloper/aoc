using AocLib;

namespace _2023.Day10;

// 7030
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input
            .SplitLines()
            .Select(_ => _.ToArray())
            .ToArray();

        var w = grid[0].Length;
        var h = grid.Length;

        Point start = (0, 0);
        for (int x = 0; x < w; ++x)
            for (int y = 0; y < h; ++y)
                if (grid[y][x] == 'S')
                {
                    start = (x, y);
                    x = w; y = h;
                    continue;
                }

        List<(Point pos, Point dir)> nextPos = [];
        var up = start.MoveUp();
        var down = start.MoveDown();
        var left = start.MoveLeft();
        var right = start.MoveRight();

        if (up.Y >= 0 && "|7F".Contains(grid[up.Y][up.X]))
            nextPos.Add((up, Point.Up));

        if (down.Y < grid.Length && "|LJ".Contains(grid[down.Y][down.X]))
            nextPos.Add((down, Point.Down));

        if (left.X >= 0 && "-FL".Contains(grid[left.Y][left.X]))
            nextPos.Add((left, Point.Left));

        if (right.X < grid[0].Length && "-J7".Contains(grid[right.Y][right.X]))
            nextPos.Add((right, Point.Right));

        var state1 = nextPos[0];
        var state2 = nextPos[1];
        var dist = 1;

        while (true)
        {
            state1 = GetNextState(grid, state1);
            state2 = GetNextState(grid, state2);
            dist++;
            if (state1.pos == state2.pos) break;
        }

        return dist;
    }

    (Point pos, Point dir) GetNextState(char[][] grid, (Point pos, Point dir) state)
    {
        return grid[state.pos.Y][state.pos.X] switch
        {
            '|' => state.dir == Point.Up
                ? (state.pos.MoveUp(), state.dir)
                : (state.pos.MoveDown(), state.dir),
            '-' => state.dir == Point.Right
                ? (state.pos.MoveRight(), state.dir)
                : (state.pos.MoveLeft(), state.dir),
            'L' => state.dir == Point.Down
                ? (state.pos.MoveRight(), Point.Right)
                : (state.pos.MoveUp(), Point.Up),
            'J' => state.dir == Point.Down
                ? (state.pos.MoveLeft(), Point.Left)
                : (state.pos.MoveUp(), Point.Up),
            '7' => state.dir == Point.Up
                ? (state.pos.MoveLeft(), Point.Left)
                : (state.pos.MoveDown(), Point.Down),
            'F' => state.dir == Point.Up
                ? (state.pos.MoveRight(), Point.Right)
                : (state.pos.MoveDown(), Point.Down),
            _ => throw new Exception("nope")
        };
    }
}
