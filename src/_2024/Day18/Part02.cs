namespace _2024.Day18;

[Answer("(43, 12)")]
public class Part02 : PuzzleSolver<string>
{
    private const int Width = 70;
    private const int NumBytes = 1024;

    protected override string InternalSolve()
    {
        var bytes = input
            .ParsePoints<int>()
            .SelectMany()
            .ToList();
        
        for (int i = 0; i < bytes.Count - NumBytes; i++)
        {
            var fallingBytes = bytes[..(NumBytes + i)];
            if (!HasPath(fallingBytes))
                return $"{fallingBytes[^1]}";
        }

        return "(0, 0)";
    }
    
    private bool HasPath(List<Point> corruptedMemory)
    {
        var start = new Point(0, 0);
        var end = new Point(Width, Width);

        var states = new Stack<(Point Position, Point[] Path)>();
        states.Push((start, [start]));

        var visited = new HashSet<Point>();

        while (states.TryPop(out var state))
        {
            var (pos, path) = state;

            if (pos == end)
                return true;

            var neighbors = pos.OrthogonalNeighbors()
                .Where(p => p.InBounds(0, Width))
                .Where(p => !path.Contains(p))
                .Where(p => !corruptedMemory.Contains(p));

            foreach (var neighbor in neighbors)
            {
                if (visited.Add(neighbor))
                    states.Push((neighbor, [..path, neighbor]));
            }
        }
            
        return false;
    }
}