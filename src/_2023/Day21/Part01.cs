using AocLib;

namespace _2023.Day21;

// 3795
public class Part01 : PuzzleSolver<long>
{
    const int MAX_STEPS = 64;

    protected override long InternalSolve()
    {
        var map = input
            .SplitLines()
            .Select(_ => _.ToArray())
            .ToArray();

        var start = FindStartPos(map);
        var queue = new Queue<(Point, int)>();
        queue.Enqueue((start, 0));

        var state = new HashSet<Point>();

        while (queue.TryDequeue(out var loc))
        {
            var (pos, steps) = loc;
            if (steps == MAX_STEPS) continue;

            steps++;

            foreach (var n in pos.OrthogonalNeighbors())
            {
                if (map[n.Y][n.X] == '.')
                    if (state.Add(n))
                        queue.Enqueue((n, steps));
            }

        }

        Print(map, state, start);

        return state.Where(_ => _.ManhattanDistance(start) % 2 == 0).Count() + 1;
    }

    void Print(char[][] map, HashSet<Point> state, Point start)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                if (state.Contains((x, y)) && start.ManhattanDistance((x, y)) % 2 == 0)
                    Console.Write('O');
                else
                    Console.Write(map[y][x]);
            }

            Console.WriteLine();
        }
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
