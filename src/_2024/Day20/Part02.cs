namespace _2024.Day20;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        const int CHEAT_TIME = 100;
        
        var grid = input.ToGrid();
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
                    .Where(pos => cheat.Track.ManhattanDistance(pos) <= 20)
                    .Select(pos => (cheat.Track, cheat.Start, End: pos)))
            .Where(cheat => ix[cheat.Track] < ix[cheat.End]);
        
        var bestTimes = cheatsToCheck
            .Select(cheat =>
            {
                var trackLength = track.Count - 1;
                var startToCheat = ix[cheat.Track];
                var cheatLength = cheat.Track.ManhattanDistance(cheat.End);
                var cheatToEnd = trackLength - ix[cheat.End];
                var timeSaved = trackLength - startToCheat - cheatToEnd - cheatLength;
                return (cheat.Track, cheat.Start, cheat.End, TimeSaved: timeSaved);
            })
            .Where(cheat => cheat.TimeSaved >= CHEAT_TIME)
            .DistinctBy(cheat => (cheat.Track, cheat.End));
        
        
        return bestTimes.Count();
    }
}
