using AocLib;

namespace _2023.Day13;

// 37381
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var mirrors = input
            .SplitEmptyLines()
            .Select(_ => _
                .SplitLines()
                .SelectMany((l, y) => l
                    .Select((c, x) => c == '.' ? new Point(-1, -1) : new Point(x, y)))
                    .Where(_ => _.X > -1)
                    .ToList())
            .ToList();

        long totalv = 0;
        long totalh = 0;
        for (int i = 0; i < mirrors.Count; i++)
        {
            var v = CheckV(mirrors[i]);
            var h = CheckH(mirrors[i]);

            if (v == -1 && h == -1)
            {
                PrintGrid(mirrors[i]);
                break;
            }

            totalv += v > 0 ? v : 0;
            totalh += h > 0 ? h : 0;
        }

        return totalv + totalh * 100;
    }

    void PrintGrid(List<Point> grid)
    {
        var maxY = grid.Max(_ => _.Y);
        var maxX = grid.Max(_ => _.X);
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                var p = grid.FirstOrDefault(_ => _ == (x, y), (-1, -1));
                Console.Write(p == (-1, -1) ? '.' : '#');
            }

            Console.WriteLine();
        }
    }

    int CheckV(List<Point> mirror)
    {
        var min = mirror.Min(_ => _.X);
        var max = mirror.Max(_ => _.X);

        for (int x = 1; x <= max; x++)
        {
            var dx2 = x * 2 - 1;
            var dx1 = dx2 - max;
            dx2 = (int)Math.Min(dx2, max);
            dx1 = Math.Max(dx1, 0);

            Console.WriteLine($"{x} - {dx1} - {dx2}");

            var p1 = mirror
                .Where(_ => _.X < x && _.X >= dx1)
                .OrderBy(_ => _.X)
                .ThenBy(_ => _.Y);

            var p2 = mirror
                .Where(_ => _.X >= x && _.X <= dx2)
                .Select(_ => new Point(dx2 - _.X + dx1, _.Y))
                .OrderBy(_ => _.X)
                .ThenBy(_ => _.Y);

            if (p1.SequenceEqual(p2))
                return x;
        }

        return -1;
    }

    int CheckH(List<Point> mirror)
    {
        var min = mirror.Min(_ => _.Y);
        var max = mirror.Max(_ => _.Y);

        for (int y = 1; y <= max; y++)
        {
            var dy2 = y * 2 - 1;
            var dy1 = dy2 - max;
            dy2 = (int)Math.Min(dy2, max);
            dy1 = Math.Max(dy1, 0);

            var p1 = mirror
                .Where(_ => _.Y < y && _.Y >= dy1)
                .OrderBy(_ => _.X)
                .ThenBy(_ => _.Y);

            var p2 = mirror
                .Where(_ => _.Y >= y && _.Y <= dy2)
                .Select(_ => new Point(_.X, dy2 - _.Y + dy1))
                .OrderBy(_ => _.X)
                .ThenBy(_ => _.Y);

            if (p1.SequenceEqual(p2))
                return y;
        }

        return -1;
    }
}
