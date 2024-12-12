using AocLib;
using MoreLinq;
using QuikGraph.Algorithms.ShortestPath;

namespace _2024.Day12;

public class Part02 : PuzzleSolver<long>
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
            var walls = new HashSet<(Point, Point)>();
            var perimeter = 0;
            
            while (queue.TryDequeue(out var p))
            {
                if (!visited.Add(p))
                    continue;

                var neighbors = p.OrthogonalAdjacentPoints()
                    .Where(n => n.InBounds(0, 0, grid[0].Length - 1, grid.Length - 1))
                    .Where(n => grid[n.Y][n.X] == plot)
                    .ToHashSet();

                var leftWall = !neighbors.Contains(p.MoveLeft());
                var rightWall = !neighbors.Contains(p.MoveRight());
                var upWall = !neighbors.Contains(p.MoveUp());
                var downWall = !neighbors.Contains(p.MoveDown());
                
                if (leftWall) walls.Add((p, Point.Left));
                if (rightWall) walls.Add((p, Point.Right));
                if (upWall) walls.Add((p, Point.Up));
                if (downWall) walls.Add((p, Point.Down));

                var countLeft = leftWall && !walls.Contains((p.MoveUp(), Point.Left)) && !walls.Contains((p.MoveDown(), Point.Left));
                var countRight = rightWall && !walls.Contains((p.MoveUp(), Point.Right)) && !walls.Contains((p.MoveDown(), Point.Right));
                var countUp = upWall && !walls.Contains((p.MoveLeft(), Point.Up)) && !walls.Contains((p.MoveRight(), Point.Up));
                var countDown = downWall && !walls.Contains((p.MoveLeft(), Point.Down)) && !walls.Contains((p.MoveRight(), Point.Down));
                
                if (countLeft) perimeter++;
                if (countRight) perimeter++;
                if (countUp) perimeter++;
                if (countDown) perimeter++;
                
                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }

            totalVisited.AddRange(visited);

            return visited.Count * perimeter;
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
