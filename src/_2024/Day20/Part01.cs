using System.Text;

namespace _2024.Day20;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input.ToGrid();
        var start = grid.Find('S');
        var end = grid.Find('E');
        
        var (path, cheats) = FindPath(grid, start, end);
        var times = GetTimesSaved(grid, path, cheats);

        var bestTimes = times.Where(t => t.TimeSaved >= 100);
        
        //Print(bestTimes.GroupBy(t => t.TimeSaved).ToList());

        return bestTimes.Count();
    }

    List<(Point[] Cheat, int TimeSaved)> GetTimesSaved(Grid<char> grid, List<Point> path, List<Point[]> cheats)
    {
        var timesSaved = new List<(Point[], int)>();
        
        var distToEnd = path.IndexOf(path[^1]);
        
        foreach (var cheat in cheats)
        {
            var distStartToP1 = path.IndexOf(cheat[2]) + 1;
            var distP2ToEnd = distToEnd - path.IndexOf(cheat[1]) + 1;
            var cheatDist = distStartToP1 + distP2ToEnd;
            var timeSaved = distToEnd - cheatDist;
            timesSaved.Add((cheat, timeSaved));
            
            // Console.Clear();
            //
            // new GridVisualizer<char>(grid)
            //     .WithOverlay(path.Contains, '@')
            //     .WithOverlay(v => v == path[0], 'S')
            //     .WithOverlay(v => v == path[^1], 'E')
            //     .WithOverlay(v => v == cheat[0], '1')
            //     .WithOverlay(v => v == cheat[1], '2')
            //     .WithValueStyle(v => v == '#', "[grey30]")
            //     .WithValueStyle(v => v == '@', "[red]")
            //     .WithValueStyle(v => "SE".Contains(v), "[green]")
            //     .WithValueStyle(v => v == '1', "[yellow4]")
            //     .WithValueStyle(v => v == '2', "[wheat4]")
            //     .Display();
            //
            // Console.WriteLine($"Total Distance: {distToEnd}");
            // Console.WriteLine($"Distance to Cheat: {distStartToP1}");
            // Console.WriteLine($"Distance from Cheat to End: {distP2ToEnd}");
            // Console.WriteLine($"Total cheat distance: {cheatDist}");
            // Console.WriteLine($"Time Saved: {timeSaved}");
            //
            //Console.ReadLine();
        }
        
        return timesSaved;
    }

    public (List<Point>, List<Point[]>) FindPath(Grid<char> grid, Point start, Point end)
    {
        var index = 0;
        var visited = new HashSet<Point>();

        var cheats = new List<Point[]>();
        var path = new List<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(start);
    
        while (queue.TryDequeue(out var pos))
        {
            if (!visited.Add(pos))
                continue;
            
            cheats.AddRange(pos.OrthogonalNeighbors()
                .Where(n => grid.InBounds(n + (n - pos)))
                .Select(n => new[] { n, n + (n - pos), pos })
                .Where(n => grid[n[0]] == '#' && ".E".Contains(grid[n[1]]))
                .Where(n => !visited.Contains(n[1])));
            
            path.Add(pos);

            if (pos == end)
            {
                return (path, cheats);
            }

            var nextPos = pos.OrthogonalNeighbors()
                .Where(n => !visited.Contains(n))
                .First(n => ".E".Contains(grid[n]));
            
            queue.Enqueue(nextPos);
        }
    
        return ([], []);
    }

    private void Print(List<IGrouping<int, (Point[] Cheat, int TimeSaved)>> cheats)
    {
        var sb = new StringBuilder();
        
        foreach (var cheat in cheats)
        {
            sb.Append("There");
            sb.Append(cheat.Count() > 1 ? $" are {cheat.Count()} cheats " : " is one cheat ");
            sb.Append($"that saved {cheat.Key} picoseconds.");
            sb.AppendLine();
        }
    
        Console.WriteLine(sb.ToString());
    }
}
