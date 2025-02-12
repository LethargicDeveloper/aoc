using AocLib;

namespace _2023.Day10;

// 285
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input
            .SplitLines()
            .Select(_ => _.ToList())
            .ToList();

        var w = grid[0].Count;
        var h = grid.Count;

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

        if (up.Y >= 0 && "|7F".Contains(grid[(int)up.Y][(int)up.X]))
            nextPos.Add((up, Point.Up));

        if (down.Y < h && "|LJ".Contains(grid[(int)down.Y][(int)down.X]))
            nextPos.Add((down, Point.Down));

        if (left.X >= 0 && "-FL".Contains(grid[(int)left.Y][(int)left.X]))
            nextPos.Add((left, Point.Left));

        if (right.X < w && "-J7".Contains(grid[(int)right.Y][(int)right.X]))
            nextPos.Add((right, Point.Right));

        var state1 = nextPos[0];
        var state2 = nextPos[1];
        var path = new HashSet<Point>
        {
            start,
            state1.pos,
            state2.pos
        };

        while (true)
        {
            state1 = GetNextState(grid, state1);
            state2 = GetNextState(grid, state2);
            path.Add(state1.pos);
            path.Add(state2.pos);
            if (state1.pos == state2.pos) break;
        }

        grid[(int)start.Y][(int)start.X] = 'J';

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (!path.Contains((x, y)))
                    grid[y][x] = '.';
            }
        }

        for (int y = 0; y < h; ++y)
            grid[y].Add('.');

        w++;

        var padding = new string('.', w).Select(_ => _).ToList();
        grid.Add(padding);

        h++;

        var inside = 0;
        for (int y = 0; y < h - 1; ++y)
        {
            var walls = 0;
            var curWall = '\0';

            for (int x = w - 1; x >= 0; --x)
            {
                var c = grid[y][x];
                if (c == '.')
                {
                    if (walls % 2 != 0)
                        inside++;
                }
                else if (c == '|')
                {
                    walls++;
                }
                else if ("FJL7".Contains(c))
                {
                    if (curWall == '\0')
                    {
                        walls++;
                        curWall = c;
                    }
                    else
                    {
                        if (c == 'F' && curWall == '7' || c == 'L' && curWall == 'J')
                        {
                            walls++;
                        }

                        curWall = '\0';
                    }
                }
            }
        }

        return inside;
    }

    (Point pos, Point dir) GetNextState(List<List<char>> grid, (Point pos, Point dir) state)
    {
        return grid[(int)state.pos.Y][(int)state.pos.X] switch
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
