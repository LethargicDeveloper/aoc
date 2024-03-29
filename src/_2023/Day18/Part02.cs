using AocLib;

namespace _2023.Day18;

// 44644464596918
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        (char, long) ParseColor(string color)
        {
            var dist = Convert.ToInt64(color[2..^2], 16);
            return (color[^2], dist);
        }

        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(' ')[2])
            .Select(ParseColor);

        (long x, long y) pos = (1, 1);
        var points = new List<(long x, long y)> { pos };

        long perimeter = 0;

        foreach (var (cmd, dist) in cmds)
        {
            (long x, long y) move = cmd switch
            {
                '0' => (dist, 0),
                '1' => (0, dist),
                '2' => (-dist, 0),
                '3' => (0, -dist),
                _ => throw new Exception("Invalid direction.")
            };

            perimeter += dist;
            pos = (pos.x + move.x, pos.y + move.y);
            points.Add(pos);
        }

        var xs = new List<long>();
        for (int i = 0; i < points.Count - 1; i++)
            xs.Add(points[i].x * points[i + 1].y);

        var ys = new List<long>();
        for (int i = 0; i < points.Count - 1; i++)
            ys.Add(points[i].y * points[i + 1].x);

        var ps = xs.Zip(ys).Select(_ => _.First - _.Second).Sum();

        var area = Math.Abs(ps / 2);

        return area + perimeter / 2 + 1;
    }
}
