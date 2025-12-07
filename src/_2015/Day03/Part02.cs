using AocLib;
using MoreLinq;

namespace _2015.Day03;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var presents = new Dictionary<Point, long>();
        
        var santa = Point.Zero;
        var robot = Point.Zero;
        
        presents[Point.Zero] = 1;
        
        for (int i = 0; i < input.Length; i++)
        {
            var dir = input[i];
            var next = dir switch
            {
                '^' => Point.Up,
                'v' => Point.Down,
                '<' => Point.Left,
                '>' => Point.Right,
                _ => throw new Exception($"Invalid direction: {dir}")
            };
            
            var pos = (i % 2 == 0 ? santa : robot) + next;
            presents[pos] = presents.GetValueOrDefault(pos) + 1;
            
            if (i % 2 == 0)
                santa = pos;
            else
                robot = pos;
        }

        return presents.Count(p => p.Value >= 1);
    }
}
