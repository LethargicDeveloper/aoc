using AocLib;

namespace AdventOfCode._2023.Day21;

public class Part02 : PuzzleSolver<long>
{
    const int MAX_STEPS = 6;

    public override long Solve()
    {
        var map = input
            .SplitLines()
            .Select(_ => _.ToCharArray())
            .ToArray();

        var locations = Locations(MAX_STEPS);

        var start = FindStartPos(map);
        var queue = new Queue<(Point, int)>();
        queue.Enqueue((start, 0));

        var state = new HashSet<Point>();

        while (queue.TryDequeue(out var loc))
        {
            var (pos, steps) = loc;
            if (steps == MAX_STEPS) continue;

            steps++;

            foreach (var n in pos.OrthogonalAdjacentPoints())
            {
                if (map[n.Y][n.X] == '.')
                    if (state.Add(n))
                        queue.Enqueue((n, steps));
            }

        }

        return 0;
    }

    void Print(char[][] map, HashSet<Point> state, Point start)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                if (state.Contains((x, y)) && (start.ManhattanDistance((x, y)) % 2 == 0))
                    Console.Write('O');
                else
                    Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }
    }

    long Locations(int steps)
    {
        var even = steps % 2 == 0;
        long total = 4 * steps--;
        for (; steps > 0; steps--)
        {
            if (even && steps % 2 == 0)
                total += (4 * steps);
            else if (!even && steps % 2 != 0)
                total += (4 * steps);
        }

        return total + (even ? 1 : 0);
    }

    long NumberOfGardens(Point start, HashSet<Point> state, long steps)
    {
        return (steps % 2 == 0)
            ? state.Where(_ => _.ManhattanDistance(start) % 2 == 0).Count()
            : state.Where(_ => _.ManhattanDistance(start) % 2 != 0).Count();
    }

    Point FindStartPos(char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
            for (int x = 0; x < map[0].Length; x++)
                if (map[y][x] == 'S')
                    return (x, y);

        throw new Exception("Start position not found.");
    }
}
