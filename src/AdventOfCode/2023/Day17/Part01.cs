using AocLib;
using Priority_Queue;

namespace AdventOfCode._2023.Day17;

public class Part01 : PuzzleSolver<long>
{
    record State(Point Pos, Point Dir, int Value, Point[] Path);

    public override long Solve()
    {
        var grid = input
            .SplitLines()
            .Select(_ => _.Select(c => c - '0').ToArray())
            .ToArray();

        return new PathFinder(grid).MinDistance();
    }

    class PathFinder(int[][] map)
    {
        record State(Point Pos, Point Dir, List<Point> Path);

        public long MinDistance()
        {
            var (dist, prev) = Dijkstra();
            var path = ShortestPath(prev, new Point(map[0].Length - 1, map.Length - 1));

            Print(path);

            return dist[path[^1]];
        }

        void Print(List<Point> path)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[0].Length; x++)
                {
                    if (path.Contains((x, y)))
                        Console.Write('.');
                    else
                        Console.Write(map[y][x]);
                }

                Console.WriteLine();
            }
        }

        (Dictionary<Point, long> dist, Dictionary<Point, Point> prev) Dijkstra()
        {
            var start = new Point(0, 0);
            var end = new Point(map[0].Length - 1, map.Length - 1);

            var queue = new SimplePriorityQueue<Point>();
            var distance = new Dictionary<Point, long>();
            var previous = new Dictionary<Point, Point>();

            for (int y = 0; y < map.Length; ++y)
                for (int x = 0; x < map[0].Length; ++x)
                {
                    var v = (x, y);
                    distance[v] = v == start ? 2 : long.MaxValue;
                    queue.Enqueue(v, distance[v]);
                }

            while (queue.TryDequeue(out var u))
            {
                //if (u == end) break;

                foreach (var v in Neighbors(u).Where(queue.Contains))
                {
                    if (!CanMove(previous, v))
                        continue;

                    var alt = distance[u] + map[v.Y][v.X];
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

        List<Point> ShortestPath(Dictionary<Point, Point> prev, Point target)
        {
            var stack = new Stack<Point>();

            while (true)
            {
                stack.Push(target);
                if (!prev.TryGetValue(target, out target))
                    break;
            }

            return stack.ToList();
        }

        bool CanMove(Dictionary<Point, Point> previous, Point v)
        {
            var path = CurrentPath(previous, v);
            if (path.Count < 3) return true;

            var lastMoves = path[^3..];
            var sameDir = lastMoves.Zip(lastMoves.Skip(1), (cur, next) => next - cur).Distinct().Count() == 1;
            return !sameDir;
        }


        List<Point> CurrentPath(Dictionary<Point, Point> previous, Point v)
        {
            return ShortestPath(previous, v)
                .AsEnumerable()
                .ToList();
        }
    }

}
