using AocLib;

namespace _2015.Day03;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var presents = new Dictionary<Point, long>();
        
        var location = Point.Zero;
        presents[location] = 1;
        
        foreach (var dir in input)
        {
            location += dir switch
            {
                '^' => Point.Up,
                'v' => Point.Down,
                '<' => Point.Left,
                '>' => Point.Right,
                _ => throw new Exception($"Invalid direction: {dir}")
            };
            
            presents[location] = presents.GetValueOrDefault(location) + 1;
        }

        return presents.Count(p => p.Value >= 1);
    }
}
