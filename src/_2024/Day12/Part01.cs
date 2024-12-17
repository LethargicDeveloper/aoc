using AocLib;

namespace _2024.Day12;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input
            .SplitLines()
            .Select(s => s.ToCharArray())
            .ToArray();

        var totalVisited = new HashSet<Point>();
        
        long CalculatePrice(Point pos)
        {
            var plot = grid[pos.Y][pos.X];
            var queue = new Queue<Point>();
            queue.Enqueue(pos);

            var visited = new HashSet<Point>();
            var perimeter = 0;
            
            while (queue.TryDequeue(out var p))
            {
                if (!visited.Add(p))
                    continue;

                var neighbors = p.OrthogonalNeighbors()
                    .Where(n => n.InBounds(0, 0, grid[0].Length - 1, grid.Length - 1))
                    .Where(n => grid[n.Y][n.X] == plot)
                    .ToList();

                perimeter += 4 - neighbors.Count;
                
                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }

            var area = visited.Count;
            totalVisited.AddRange(visited);

            return area * perimeter;
        }

        var price = 0L;
        
        for (int y = 0; y < grid.Length; y++)
        for (int x = 0; x < grid[0].Length; x++)
        {
            if (!totalVisited.Contains((x, y)))
            {
                price += CalculatePrice((x, y));
            }
        }

        return price;
    }
}
