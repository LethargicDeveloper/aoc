using AocLib;
using Priority_Queue;

namespace AdventOfCode._2022.Day12;

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var map = this.input
            .SplitLines()
            .Select(_ => _.ToCharArray())
            .ToArray();

        return new PathFinder(map).MinDistance();
    }

    class PathFinder
    {
        readonly char[][] map;
        readonly Point? startPos = null;

        public PathFinder(char[][] map, Point? startPos = null)
        {
            this.startPos = startPos;
            this.map = map;
        }
        static bool InRange(char height1, char height2)
            => ("yz".Contains(height1) && height2 == 'E') || (height2 != 'E' && (height2 <= height1 + 1));

        public long MinDistance()
        {
            var (dist, prev) = Dijkstra();
            var path = ShortestPath(prev);
            return dist[path[^1]];
        }

        (Point, Point) GetStartAndEndPos()
        {
            var startPos = this.startPos ?? (0, 0);
            var endPos = (0, 0);
            for (int y = 0; y < map.Length; ++y)
                for (int x = 0; x < map[0].Length; ++x)
                {
                    var c = map[y][x];

                    if (c == 'S')
                    {
                        map[y][x] = 'a';
                        if (this.startPos == null)
                        {
                            startPos = (x, y);
                        }
                    }

                    if (c == 'E')
                    {
                        endPos = (x, y);
                    }
                }

            return (startPos, endPos);
        }

        List<Point> ShortestPath(Dictionary<Point, Point> prev)
        {
            var stack = new Stack<Point>();
            var (_, target) = GetStartAndEndPos();

            while (true)
            {
                stack.Push(target);
                if (!prev.TryGetValue(target, out target))
                    break;
            }

            return stack.ToList();
        }

        (Dictionary<Point, long> dist, Dictionary<Point, Point> prev) Dijkstra()
        {
            var (start, end) = GetStartAndEndPos();

            var queue = new SimplePriorityQueue<Point>();
            var distance = new Dictionary<Point, long>();
            var previous = new Dictionary<Point, Point>();

            for (int y = 0; y < map.Length; ++y)
                for (int x = 0; x < map[0].Length; ++x)
                {
                    var v = (x, y);
                    distance[v] = v == start ? 0 : long.MaxValue;
                    queue.Enqueue(v, distance[v]);
                }

            while (queue.TryDequeue(out var u))
            {
                if (u == end) break;

                foreach (var v in Neighbors(u).Where(queue.Contains))
                {
                    if (!InRange(map[u.Y][u.X], map[v.Y][v.X]))
                        continue;

                    var alt = distance[u] + 1;
                    if (alt < distance[v])
                    {
                        distance[v] = alt;
                        previous[v] = u;

                        queue.UpdatePriority(v, alt);
                    }
                }
            }

            return (distance, previous);
        }

        List<Point> Neighbors(Point point)
        {
            var points = new List<Point>();
            if (point.X > 0)
                points.Add((point.X - 1, point.Y));

            if (point.X < map[0].Length - 1)
                points.Add((point.X + 1, point.Y));

            if (point.Y > 0)
                points.Add((point.X, point.Y - 1));

            if (point.Y < map.Length - 1)
                points.Add((point.X, point.Y + 1));

            return points;
        }
    }
}
