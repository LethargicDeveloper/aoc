using AocLib;

namespace _2024.Day16;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input
            .SplitLines()
            .Select(s => s.ToCharArray())
            .ToArray();

        var startPos = grid.FindPosition('S');
        var startDir = Point.Right;
        var end = grid.FindPosition('E');
        
        var bestScore = long.MaxValue;
        var visited = new Dictionary<(Point pos, Point dir), long>();
        var states = new Stack<(Point pos, Point dir, List<Point> path, long score)>();

        var initialPath = new List<Point>() { startPos };
        states.Push((startPos, startDir, initialPath, 0));
        visited[(startPos, startDir)] = 0;
        
        var bestPath = new List<Point>();
        
        while (states.TryPop(out var state))
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

            var neighbors = pos.OrthogonalAdjacentPoints()
                .Where(p => grid.At(p) != '#')
                .Where(p => !path.Contains(p));

            long newScore = 0;
            long prevScore = 0;
            
            foreach (var neighbor in neighbors)
            {
                var newPath = path.Append(neighbor).ToList();
                
                if (pos + dir == neighbor)
                {
                    newScore = score + 1;
                    if (!visited.TryGetValue((neighbor, dir), out prevScore) || newScore < prevScore)
                    {
                        states.Push((neighbor, dir, newPath, newScore));
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
                    
                    left = (left.turn + 1, Point.TurnLeft(left.dir));
                    right = (right.turn + 1, Point.TurnRight(right.dir));
                }

                newScore = score + 1 + (1000 * newDir.turn);
                if (!visited.TryGetValue((neighbor, dir), out prevScore) || newScore < prevScore)
                {
                    states.Push((neighbor, newDir.dir, newPath, newScore));
                    visited[(neighbor, newDir.dir)] = newScore;
                }
            }
        }
        
        grid.Print(overlay: bestPath);
        
        return bestScore;
    }
}