namespace _2024.Day20;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        const int CHEAT_TIME = 100;
        
        var grid = input.ToGrid<char>();
        var start = grid.Find('S');
        var end = grid.Find('E');
        
        var (track, _) = new Part01().FindPath(grid, start, end);
        var ix = track
            .WithIndex()
            .ToDictionary(k => k.Value, v => v.Index);

        var cheatStarts = track
            .SelectMany(pos => pos
                .OrthogonalNeighbors()
                .Where(n => grid[n] == '#')
                .Select(n => (Track: pos, Start: n)));

        var cheatsToCheck = cheatStarts
                .SelectMany(cheat => track
                    .Where(pos => pos.ManhattanDistance(cheat.Start) <= 20)
                    .Select(pos => (cheat.Track, cheat.Start, End: pos)))
            .Where(cheat => ix[cheat.Track] < ix[cheat.End]);

        var found = new HashSet<(Point, Point)>();
        foreach (var cheat in cheatsToCheck)
        {
            if (found.Contains((cheat.Track, cheat.End)))
                continue;

            var states = new Queue<(Point, int, Point[])>();
            states.Enqueue((cheat.Start, 0, [cheat.Start]));
            
            var visited = new HashSet<Point>();
            visited.AddRange(track[..(ix[cheat.Track] + 1)]);

            while (states.TryDequeue(out var state))
            {
                var (pos, length, path) = state;

                if (!visited.Add(pos))
                    continue;

                if (length > 20)
                    continue;

                if (pos == cheat.End)
                {
                    var distStartToCheatStart = ix[cheat.Track] + 1;
                    var distCheatEndToEnd = (track.Count - 1) - ix[cheat.End];
                    var timeSaved = (track.Count - 1) - (distStartToCheatStart + length + distCheatEndToEnd);
                    if (timeSaved >= CHEAT_TIME)
                    {
                        found.Add((cheat.Track, cheat.End));
                        
                        // new GridVisualizer<char>(grid)
                        //     .WithData(new
                        //     {
                        //         distStartToCheatStart,
                        //         length,
                        //         distCheatEndToEnd,
                        //         timeSaved
                        //     })
                        //     .WithOverlay(track.Contains, '@')
                        //     .WithOverlay(path.Contains, '*')
                        //     .WithOverlay(v => v == start, 'S')
                        //     .WithOverlay(v => v == end, 'E')
                        //     .WithOverlay(v => v == cheat.Start, '1')
                        //     .WithOverlay(v => v == cheat.End, '2')
                        //     .WithOverlay(v => v == cheat.Track, '!')
                        //     .WithStyle((_, v) => v == '#', "[gray]")
                        //     .WithStyle((_, v) => "SE".Contains(v), "[green]")
                        //     .WithStyle((_, v) => v == '@', "[red]")
                        //     .WithStyle((_, v) => v == '*', "[yellow]")
                        //     .WithStyle((_, v) => "!12".Contains(v), "[yellow]")
                        //     .Display(wait: true);
                        
                        break;
                    }

                    continue;
                }
                
                var neighbors = pos
                    .OrthogonalNeighbors()
                    .Where(grid.InBounds)
                    .Where(n => !visited.Contains(n));

                foreach (var neighbor in neighbors)
                {
                    states.Enqueue((neighbor, length + 1, [..path, neighbor]));
                }
            }
        }
        
        return found.Count;
    }
}
