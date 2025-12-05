using MoreLinq;

namespace _2024.Day21;

[Answer(163920)]
public class Part02 : PuzzleSolver<long>
{
    Grid<char> numpad = """
                        789
                        456
                        123
                         0A
                        """.ToGrid();

    Grid<char> dirpad = """
                         ^A
                        <v>
                        """.ToGrid();
    
    protected override long InternalSolve()
    {
        var codes = input.SplitLines();

        long sum = 0;
        
        foreach (var code in codes)
        {
            var length = SolveCodePathLength(code);
            sum += length * code.ParseNumbers<int>()[0][0];
        }
        
        return sum;
    }

    long SolveCodePathLength(string code)
    {
        long length = 0;

        var start = numpad.Find('A');

        foreach (var num in code)
        {
            var end = numpad.Find(num);
            
            var numPaths = FindNumPadPath(start, end);

            length += numPaths.Select(p => FindDirPadLength(p)).Min();
            
            start = end;
        }

        return length;
    }

    private Dictionary<string, long> memo = [];
    long FindDirPadLength(string path, int robot = 0)
    {
        var key = $"{path}-{robot}";

        if (memo.TryGetValue(key, out var value))
        {
            return value;
        }
        
        if (robot == 25)
        {
            memo[key] = path.Length;
            return memo[key];
        }

        var newPaths = "A"
            .Concat(path)
            .Window(2)
            .Select(w => FindDirPadPath(dirpad.Find(w[0]), dirpad.Find(w[1])));

        var length = newPaths
            .Select(p => p.Select(x => FindDirPadLength(x, robot + 1)).Min())
            .Sum();
        
        memo[key] = length;
        return memo[key];
    }
    
    List<string> FindNumPadPath(Point start, Point end)
    {
        List<List<Point>> paths = [];
        
        var states = new Queue<(Point, List<Point>)>();
        states.Enqueue((start, [start]));
        
        int length = int.MaxValue;
        
        while (states.TryDequeue(out var state))
        {
            var (pos, path) = state;

            if (path.Count > length)
                continue;
            
            if (pos == end)
            {
                if (path.Count <= length)
                {
                    paths.Add(path);
                    length = path.Count;
                }
                
                continue;
            }

            var neighbors = pos
                .OrthogonalNeighbors()
                .Where(numpad.InBounds)
                .Where(n => !path.Contains(n))
                .Where(n => numpad[n] != ' ');

            foreach (var neighbor in neighbors)
            {
                states.Enqueue((neighbor, [..path, neighbor]));
            }
        }

        return paths.Select(path => path
            .GetDirections()
            .Skip(1)
            .Select(dir => dir switch
            {
                _ when dir == Point.Up => '^',
                _ when dir == Point.Down => 'v',
                _ when dir == Point.Left => '<',
                _ when dir == Point.Right => '>',
                _ => throw new Exception("invalid direction"),
            }).Append('A').AsString())
            .ToList();
    }

    List<string> FindDirPadPath(Point start, Point end)
    {
        List<List<Point>> paths = [];
        
        var states = new Queue<(Point, List<Point>)>();
        states.Enqueue((start, [start]));
        
        int length = int.MaxValue;
        
        while (states.TryDequeue(out var state))
        {
            var (pos, path) = state;

            if (path.Count > length)
                continue;
            
            if (pos == end)
            {
                if (path.Count <= length)
                {
                    paths.Add(path);
                    length = path.Count;
                }
                
                continue;
            }

            var neighbors = pos
                .OrthogonalNeighbors()
                .Where(dirpad.InBounds)
                .Where(n => !path.Contains(n))
                .Where(n => dirpad[n] != ' ');

            foreach (var neighbor in neighbors)
            {
                states.Enqueue((neighbor, [..path, neighbor]));
            }
        }

        return paths.Select(path => path
                .GetDirections()
                .Skip(1)
                .Select(dir => dir switch
                {
                    _ when dir == Point.Up => '^',
                    _ when dir == Point.Down => 'v',
                    _ when dir == Point.Left => '<',
                    _ when dir == Point.Right => '>',
                    _ => throw new Exception("invalid direction"),
                }).Append('A').AsString())
            .ToList();
    }
}