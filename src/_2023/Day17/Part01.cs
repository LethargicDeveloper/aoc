using AocLib;
using QuikGraph;

namespace _2023.Day17;

// 1128
public class Part01 : PuzzleSolver<long>
{
    record State(List<Point> Path, int Distance, Point CurDirection, int DirectionCount);

    protected override long InternalSolve()
    {
        var lines = input.SplitLines();
        bool InBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X <= lines[0].Length - 1 && p.Y <= lines.Length - 1;

        var start = new Point(0, 0);
        var end = new Point(lines[0].Length - 1, lines.Length - 1);

        var vertexes = new List<Point>();
        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[0].Length; x++)
                vertexes.Add((x, y));

        var graph = vertexes.ToAdjacencyGraph(edge =>
            edge.OrthogonalNeighbors()
                .Where(InBounds)
                .Select(p => new STaggedEdge<Point, int>(edge, p, (int)char.GetNumericValue(lines[(int)p.Y][(int)p.X])))
                .ToList());

        var queue = new Queue<State>();
        queue.Enqueue(new State([start], 0, Point.Zero, 0));

        var hash = new Dictionary<(Point p, Point dir, int cnt), int>();
        var finalStates = new List<State>();

        while (queue.TryDequeue(out var state))
        {
            var (path, distance, direction, dirCount) = state;

            if (path[^1] == end)
            {
                finalStates.Add(state);
            }

            foreach (var neighbor in path[^1].OrthogonalNeighbors())
            {
                var nextDirection = neighbor - path[^1];

                if (!InBounds(neighbor))
                    continue;

                if (path.Contains(neighbor))
                    continue;

                if (nextDirection == direction && dirCount == 3)
                    continue;

                var newState = new State(
                    Path: [.. path, neighbor],
                    Distance: distance + (graph.TryGetEdge(path[^1], neighbor, out var edge)
                        ? edge.Tag
                        : throw new Exception("Edge not found!")),
                    CurDirection: nextDirection,
                    DirectionCount: nextDirection == direction ? dirCount + 1 : 1);

                var key = (newState.Path[^1], newState.CurDirection, newState.DirectionCount);
                if (!hash.TryGetValue(key, out int value))
                    hash[key] = int.MaxValue;

                if (newState.Distance >= hash[key])
                    continue;

                hash[key] = newState.Distance;

                queue.Enqueue(newState);
            }
        }

        var result = hash.Where(_ => _.Key.p == end).Select(_ => _.Value).Min();
        var finalState = finalStates.Where(_ => _.Distance == result).First();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (finalState.Path.Contains((x, y)))
                {
                    var color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(lines[y][x]);
                    Console.BackgroundColor = color;
                }
                else
                {
                    Console.Write(lines[y][x]);
                }
            }

            Console.WriteLine();
        }


        return result;
    }
}
