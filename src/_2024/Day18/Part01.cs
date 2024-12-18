namespace _2024.Day18;

[Answer(278)]
public class Part01 : PuzzleSolver<long>
{
    private const int Width = 70;
    private const int NumBytes = 1024;
    
    protected override long InternalSolve()
    {
        var bytes = input
            .ParsePoints<int>()
            .SelectMany()
            .Take(NumBytes)
            .ToList();

        var start = new Point(0, 0);
        var end = new Point(Width, Width);

        var states = new Queue<(Point Position, Point[] Path)>();
        states.Enqueue((start, [start]));
        
        var visited = new HashSet<Point>();

        while (states.TryDequeue(out var state))
        {
            var (pos, path) = state;
         
            if (pos == end)
                return path.Length - 1;

            var neighbors = pos.OrthogonalNeighbors()
                .Where(p => p.InBounds(0, Width))
                .Where(p => !path.Contains(p))
                .Where(p => !bytes.Contains(p));

            foreach (var neighbor in neighbors)
            {
                if (visited.Add(neighbor))
                    states.Enqueue((neighbor, [..path, neighbor]));
            }
        }
        
        return 0;
    }
}
