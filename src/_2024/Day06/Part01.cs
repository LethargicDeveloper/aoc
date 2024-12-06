using AocLib;

namespace _2024.Day06;

// > 5550
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var map = input.SplitLines();

        var startPos = map
            .Select((x, i) => (X: x.IndexOf('^'), Y: i))
            .First(x => x.X != -1);
        
        var queue = new Queue<(Point, Point)>();
        queue.Enqueue((startPos, Point.Up));

        var visited = new HashSet<Point> { startPos };

        while (queue.TryDequeue(out var guard))
        {
            var (curPos, dir) = guard;

            var nextPos = curPos + dir;
            if (!nextPos.InBounds(0, 0, map[0].Length - 1, map.Length - 1))
                break;
            
            if (map[nextPos.Y][nextPos.X] == '#')
                queue.Enqueue((curPos, Point.RotateRight90(dir)));
            else
            {
                visited.Add(nextPos);
                queue.Enqueue((nextPos, dir));
            }
        }
        
        return visited.Count;
    }
}
