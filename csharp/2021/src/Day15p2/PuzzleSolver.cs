using BenchmarkDotNet.Attributes;
using Priority_Queue;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var graph = input
            .SplitLines()
            .Select(_ => _.Select(c => (int)char.GetNumericValue(c)).ToArray())
            .ToArray();

        var newGraph = ExpandGraph(graph);

        var pathFinder = new PathFinder(newGraph);
        return pathFinder.TotalDistance();
    }

    static int[][] ExpandGraph(int[][] graph)
    {
        var size = graph.Length;

        var newGraph = new int[size * 5][];
        for (int y = 0; y < size * 5; ++y)
            newGraph[y] = new int[size * 5];

        var sizex = -1;
        var sizey = -1;
        for (int y = 0; y < size * 5; ++y)
        {
            if (y % size == 0) ++sizey;
            for (int x = 0; x < size * 5; ++x)
            {
                var yy = y % size;
                var xx = x % size;
                if (x % size == 0) ++sizex;

                var val = graph[yy][xx] + (sizex + sizey);
                newGraph[y][x] = val > 9 ? (val % 10) + 1 : val;
            }

            sizex = -1;
        }

        return newGraph;
    }
}

class PathFinder
{
    readonly int[][] graph;
    readonly int size;

    public PathFinder(int[][] graph)
    {
        this.graph = graph;
        this.size = graph.Length;
    }

    public long TotalDistance()
    {
        var (dist, prev) = Dijkstra(graph);
        var path = ShortestPath(prev);
        return dist[path[^1]];
    }

    List<Point> ShortestPath(Dictionary<Point, Point> prev)
    {
        var stack = new Stack<Point>();
        var target = new Point(size - 1, size - 1);

        while (true)
        {
            stack.Push(target);
            if (!prev.TryGetValue(target, out target))
                break;
        }

        return stack.ToList();
    }

    (Dictionary<Point, long> dist, Dictionary<Point, Point> prev) Dijkstra(int[][] graph)
    {
        var start = new Point(0, 0);
        var end = new Point(size - 1, size - 1);

        var queue = new SimplePriorityQueue<Point>();
        var distance = new Dictionary<Point, long>();
        var previous = new Dictionary<Point, Point>();

        for (int y = 0; y < size; ++y)
            for (int x = 0; x < size; ++x)
            {
                var v = new Point(x, y);
                distance[v] = long.MaxValue;
                queue.Enqueue(v, distance[v]);
            }

        distance[start] = 0;

        while (queue.Count > 0)
        {
            var u = queue.Dequeue();
            if (u == end) break;

            foreach (var v in Neighbors(u).Where(_ => queue.Contains(_)))
            {
                var alt = distance[u] + graph[v.Y][v.X];
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
        if (point.X > 0) points.Add((point.X - 1, point.Y));
        if (point.X < size - 1) points.Add((point.X + 1, point.Y));
        if (point.Y > 0) points.Add((point.X, point.Y - 1));
        if (point.Y < size - 1) points.Add((point.X, point.Y + 1));
        return points;
    }
}
