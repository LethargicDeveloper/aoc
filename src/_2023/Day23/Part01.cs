using AocLib;

namespace _2023.Day23;

// 2186
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var map = input.SplitLines();

        var start = new Point(map[0].IndexOf('.'), 0);
        var end = new Point(map[^1].IndexOf('.'), map.Length - 1);

        var stack = new Stack<(Point pos, List<Point> path)>();
        stack.Push((start, [start]));

        var finalPaths = new List<List<Point>>();

        while (stack.TryPop(out var state))
        {
            var (pos, path) = state;

            if (pos == end)
            {
                finalPaths.Add([.. path, pos]);
                continue;
            }

            var neighbors = pos
                .OrthogonalNeighbors()
                .Where(_ => _.InBounds(0, 0, map[0].Length - 1, map.Length - 1))
                .Where(_ => !path.Contains(_))
                .Where(_ => map[(int)_.Y][(int)_.X] != '#');

            foreach (var n in neighbors)
            {
                var symbol = map[(int)pos.Y][(int)pos.X];
                if (symbol == 'v' && n - pos == Point.Down ||
                    symbol == '^' && n - pos == Point.Up ||
                    symbol == '<' && n - pos == Point.Left ||
                    symbol == '>' && n - pos == Point.Right ||
                    symbol == '.')
                {
                    stack.Push((n, [.. path, n]));
                }
            }
        }

        return finalPaths.Max(_ => _.Count()) - 2;
    }
}
