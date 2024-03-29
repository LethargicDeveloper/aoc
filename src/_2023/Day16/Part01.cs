using AocLib;

namespace _2023.Day16;

// 7210
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input
            .SplitLines()
            .Select(r => r
                .Select(v => (v, e: false))
                .ToArray())
            .ToArray();

        var lights = new Queue<(Point pos, Point dir)>();
        lights.Enqueue(((0, 0), Point.Right));

        var state = new HashSet<(Point, Point)>();

        while (lights.TryDequeue(out var light))
        {
            var (pos, dir) = light;

            if (light.pos.X < 0 || light.pos.X > grid[0].Length - 1 || light.pos.Y < 0 || light.pos.Y > grid.Length - 1)
                continue;

            if (!state.Add(light))
                continue;

            grid[pos.Y][pos.X].e = true;

            switch (grid[pos.Y][pos.X].v)
            {
                case '/':
                    light = dir switch
                    {
                        var d when d == Point.Right => (light.pos.MoveUp(), Point.Up),
                        var d when d == Point.Left => (light.pos.MoveDown(), Point.Down),
                        var d when d == Point.Down => (light.pos.MoveLeft(), Point.Left),
                        var d when d == Point.Up => (light.pos.MoveRight(), Point.Right),
                        _ => throw new Exception("Invalid direction.")
                    };
                    break;

                case '\\':
                    light = dir switch
                    {
                        var d when d == Point.Right => (light.pos.MoveDown(), Point.Down),
                        var d when d == Point.Left => (light.pos.MoveUp(), Point.Up),
                        var d when d == Point.Down => (light.pos.MoveRight(), Point.Right),
                        var d when d == Point.Up => (light.pos.MoveLeft(), Point.Left),
                        _ => throw new Exception("Invalid direction.")
                    };
                    break;

                case '|':
                    switch (dir)
                    {
                        case var d when d == Point.Right || d == Point.Left:
                            var newLight = (light.pos.MoveDown(), Point.Down);
                            if (!lights.Contains(newLight))
                                lights.Enqueue(newLight);
                            light = (light.pos.MoveUp(), Point.Up);
                            break;

                        default:
                            light = (light.pos + light.dir, light.dir);
                            break;
                    }
                    break;

                case '-':
                    switch (dir)
                    {
                        case var d when d == Point.Down || d == Point.Up:
                            var newLight = (light.pos.MoveLeft(), Point.Left);
                            if (!lights.Contains(newLight))
                                lights.Enqueue(newLight);
                            light = (light.pos.MoveRight(), Point.Right);
                            break;

                        default:
                            light = (light.pos + light.dir, light.dir);
                            break;
                    }
                    break;

                default:
                    light = (light.pos + light.dir, light.dir);
                    break;
            }

            lights.Enqueue(light);
        }

        return grid
            .SelectMany()
            .Where(_ => _.e)
            .Count();
    }

    void Print((char v, bool e)[][] grid, IEnumerable<(Point pos, Point dir)> lights)
    {
        Console.Clear();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                var light = lights.Where(_ => _.pos == (x, y));
                if (light.Any())
                {
                    if (light.Count() > 1)
                        Console.Write(light.Count());
                    else
                    {
                        var f = light.First();
                        if (f.dir == Point.Up) Console.Write('^');
                        else if (f.dir == Point.Down) Console.Write('V');
                        else if (f.dir == Point.Left) Console.Write('<');
                        else if (f.dir == Point.Right) Console.Write('>');
                    }
                }
                else
                {
                    Console.Write(grid[y][x].e ? '#' : grid[y][x].v);
                }
            }

            Console.WriteLine();
        }

        Console.ReadKey(false);
    }
}
