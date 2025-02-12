using AocLib;
using QuikGraph;

namespace _2023.Day23;

// 6802
public class Part02 : PuzzleSolver<long>
{
    string slopes = "^><v";

    protected override long InternalSolve()
    {
        var map = input.SplitLines();

        var startPos = new Point(map[0].IndexOf('.'), 0);
        var endPos = new Point(map[^1].IndexOf('.'), map.Length - 1);
        var intersections = new List<Point>() { startPos, endPos };

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                var x1 = Math.Clamp(x - 1, 0, map[0].Length - 1);
                var x2 = Math.Clamp(x + 1, 0, map[0].Length - 1);
                var y1 = Math.Clamp(y - 1, 0, map.Length - 1);
                var y2 = Math.Clamp(y + 1, 0, map.Length - 1);

                if (slopes.Contains(map[y][x1]) && slopes.Contains(map[y][x2]) ||
                    slopes.Contains(map[y1][x]) && slopes.Contains(map[y2][x]))
                {
                    intersections.Add((x, y));
                }
            }
        }

        var edges = intersections.SelectMany(_ => FindEdges(map, _, intersections.Where(i => i != _).ToList()));

        var graph = edges.ToUndirectedGraph<Point, STaggedEdge<Point, int>>();

        return LongestPath(startPos, endPos, graph);
    }

    List<STaggedEdge<Point, int>> FindEdges(string[] map, Point start, List<Point> intersections)
    {
        var queue = new Queue<(Point pos, List<Point> path, int dist)>();
        queue.Enqueue((start, [start], 0));

        var paths = new List<STaggedEdge<Point, int>>();

        while (queue.TryDequeue(out var state))
        {
            var (pos, path, dist) = state;

            if (intersections.Contains(pos))
            {
                paths.Add(new STaggedEdge<Point, int>(start, pos, dist));
                continue;
            }

            var neighbors = pos
                .OrthogonalNeighbors()
                .Where(_ => _.InBounds(0, 0, map[0].Length - 1, map.Length - 1))
                .Where(_ => !path.Contains(_))
                .Where(_ => map[(int)_.Y][(int)_.X] != '#');

            foreach (var n in neighbors)
            {
                queue.Enqueue((n, [.. path, n], dist + 1));
            }
        }

        return paths;
    }

    long LongestPath(Point start, Point end, UndirectedGraph<Point, STaggedEdge<Point, int>> graph)
    {
        var stack = new Stack<(Point pos, List<Point> path, long dist)>();
        stack.Push((start, [start], 0));

        var finalPaths = new List<(List<Point>, long)>();

        while (stack.TryPop(out var state))
        {
            var (pos, path, dist) = state;

            if (pos == end)
            {
                finalPaths.Add(([.. path, pos], dist));
                continue;
            }

            var neighbors = graph
                .AdjacentEdges(pos)
                .Where(e => e.Source == pos && !path.Contains(e.Target));

            foreach (var e in neighbors)
            {
                stack.Push((e.Target, [.. path, e.Target], dist + e.Tag));
            }
        }

        return finalPaths.Max(p => p.Item2);
    }
}
