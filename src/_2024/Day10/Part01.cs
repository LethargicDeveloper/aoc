using AocLib;

namespace _2024.Day10;

public class Part01 : PuzzleSolver<long>
{
    private List<List<int>> map = [];
    
    protected override long InternalSolve()
    {
        map = input
            .SplitLines()
            .Select(x => x
                .Select(char.GetNumericValue)
                .Select(Convert.ToInt32)
                .ToList())
            .ToList();

        var startPositions = new List<Point>();
        for (int y = 0; y < map.Count; y++)
        for (int x = 0; x < map[0].Count; x++)
        {
            if (map[y][x] == 0)
                startPositions.Add(new Point(x, y));
        }

        var totalScore = startPositions
            .AsParallel()
            .Select(TrailheadScore)
            .Sum();
        
        return totalScore;
    }

    long TrailheadScore(Point point)
    {
        bool InBounds(Point p) => p.InBounds(0, 0, map[0].Count - 1, map.Count - 1);
        
        var queue = new Queue<Point>();
        queue.Enqueue(point);

        var end = new HashSet<Point>();
        while (queue.TryDequeue(out var pos))
        {
            var (x, y) = pos;
            var val = map[(int)y][(int)x];

            if (val == 9)
            {
                end.Add(pos);
                continue;
            }
            
            foreach (var next in pos.OrthogonalNeighbors())
                if (InBounds(next) && map[(int)next.Y][(int)next.X] == val + 1)
                    queue.Enqueue(next);
        }
        
        return end.Count;
    }
}
