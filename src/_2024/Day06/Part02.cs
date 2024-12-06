using AocLib;

namespace _2024.Day06;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        long loop = 0;
        
        var map = input.SplitLines();
        var width = map[0].Length;
        var height = map.Length;
        
        for (int xx = 0; xx < width; xx++)
        for (int yy = 0; yy < height; yy++)
        {
            if (map[yy][xx] == '#' || map[yy][xx] == '^')
                continue;
            
            map = input.SplitLines();
            map[yy] = map[yy].ReplaceCharAt(xx, '#'); 
            
            var startPos = map
                .Select((x, i) => (X: x.IndexOf('^'), Y: i))
                .First(x => x.X != -1);
            
            var queue = new Queue<(Point, Point)>();
            queue.Enqueue((startPos, Point.Up));

            var visited = new HashSet<(Point, Point)> { (startPos, Point.Up) };
        
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
                    if (visited.Add((nextPos, dir)))
                        queue.Enqueue((nextPos, dir));
                    else
                    {
                        loop++;
                        break;
                    }
                }
            }
        }
        
        return loop;
    }
}
