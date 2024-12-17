using System.Text;
using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day14;

public class Part02 : PuzzleSolver<long>
{
    private const long WIDTH = 101;
    private const long HEIGHT = 103;
    
    protected override long InternalSolve()
    {
        var data = input
            .SplitLines()
            .Select(l => Regex.Matches(l, @"[-]?\d+")
                    .Select(m => int.Parse(m.Value))
                    .ToList() switch
                {
                    [var px, var py, var vx, var vy] =>
                        (Pos: new Point<long>(px, py), Velocity: new Point<long>(vx, vy)),
                    _ => throw new InvalidOperationException()
                })
            .ToList();

        var pos = data.Select(d => d.Pos).ToList();
        var vel = data.Select(d => d.Velocity).ToList();
        
        for (long time = 0 ;; time++)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                var newPos = pos[i] + vel[i];
                pos[i] = (MathEx.Mod(newPos.X, WIDTH), MathEx.Mod(newPos.Y, HEIGHT));
            }

            foreach (var point in pos)
            {
                var tree = new HashSet<Point<long>>();
                tree.Add(point);

                for (int y = 1; y < 5; y++)
                {
                    tree.Add((point.X - y, point.Y + y));
                    tree.Add((point.X + y, point.Y + y));
                }
                
                if (tree.All(t => pos.Contains(t)))
                {
                    Console.Clear();
                    Print(pos);

                    break;
                }
            }
        }
            
        return 8006;
    }

    void Print(List<Point<long>> points)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                sb.Append(points.Contains((x, y)) ? '#' : '.');
            }
            
            sb.AppendLine();
        }
        
        Console.WriteLine(sb.ToString());
    }
}