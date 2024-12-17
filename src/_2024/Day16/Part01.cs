namespace _2024.Day16;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input.ToCharGrid();

        var startPos = grid.Find('S');
        var startDir = Point.Right;
        var end = grid.Find('E');
        
        var bestScore = long.MaxValue;
        var visited = new Dictionary<(Point pos, Point dir), long>();
        var states = new Queue<(Point pos, Point dir, List<Point> path, long score)>();

        var initialPath = new List<Point>() { startPos };
        states.Enqueue((startPos, startDir, initialPath, 0));
        visited[(startPos, startDir)] = 0;
        
        var bestPath = new List<Point>();
        
        while (states.TryDequeue(out var state))
        {
            var (pos, dir, path, score) = state;
            
            if (score >= bestScore)
                continue;

            if (pos == end)
            {
                bestPath = path;
                bestScore = Math.Min(bestScore, score);
                continue;
            }

            var neighbors = pos.OrthogonalNeighbors()
                .Where(p => grid.At(p) != '#')
                .Where(p => !path.Contains(p));

            foreach (var neighbor in neighbors)
            {
                var newPath = path.Append(neighbor).ToList();
                long newScore = 0;
                long prevScore = 0;

                if (pos + dir == neighbor)
                {
                    newScore = score + 1;
                    if (!visited.TryGetValue((neighbor, dir), out prevScore) || newScore < prevScore)
                    {
                        states.Enqueue((neighbor, dir, newPath, newScore));
                        visited[(neighbor, dir)] = newScore;
                    }

                    continue;
                }

                (int turn, Point dir) left = (0, dir);
                (int turn, Point dir) right = (0, dir);
                (int turn, Point dir) newDir;
                
                while (true)
                {
                    if (neighbor == pos + left.dir)
                    {
                        newDir = left;
                        break;
                    }

                    if (neighbor == pos + right.dir)
                    {
                        newDir = right;
                        break;
                    }
                    
                    left = (left.turn + 1, Point.TurnLeft90(left.dir));
                    right = (right.turn + 1, Point.TurnRight90(right.dir));
                }

                newScore = score + 1 + (1000 * newDir.turn);
                if (!visited.TryGetValue((neighbor, dir), out prevScore) || newScore < prevScore)
                {
                    states.Enqueue((neighbor, newDir.dir, newPath, newScore));
                    visited[(neighbor, newDir.dir)] = newScore;
                }
            }
        }

        new GridVisualizer<char>(grid)
            .WithOverlay((x, y) => bestPath.Contains((x, y)), '@')
            .WithValueStyle(val => val == '@', "[red]")
            .Display();
        
        return bestScore;
    }
}