namespace _2025.Day07;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input.ToGrid();
        
        var queue = new Queue<List<Point>>();
        queue.Enqueue([grid.Find('S')]);

        var count = 0;
        while (queue.TryDequeue(out var pos))
        {
            if (pos.Count == 0)
                continue;
            
            pos = pos
                .Select(p => p.MoveDown())
                .Where(grid.InBounds)
                .SelectMany(p =>
                {
                    if (grid[p] != '^')
                        return new[] { p };
                    
                    count++;
                    return [p.MoveLeft(), p.MoveRight()];

                })
                .Distinct()
                .ToList();
            
            queue.Enqueue(pos);
        }

        return count;
    }
}

