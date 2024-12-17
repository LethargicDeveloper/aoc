using System.Collections;
using System.Runtime.InteropServices;
using AocLib;

namespace _2024.Day15;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var data = input.SplitEmptyLines();
        
        var grid = data[0]
            .SplitLines()
            .Select(s => s
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@."))
            .ToCharGrid();

        var cmds = data[1]
            .SplitLines()
            .SelectMany()
            .ToArray();

        var pos = grid.Find('@');

        var index = 0;
        foreach (var cmd in cmds)
        {
            grid.Set(pos, cmd);

            var dir = cmd switch
            {
                '^' => Point.Up,
                'v' => Point.Down,
                '<' => Point.Left,
                '>' => Point.Right,
                _ => throw new Exception($"Invalid direction {cmd}")
            };

            pos = dir == Point.Left || dir == Point.Right
                ? MoveHorizontal(pos, dir, grid, cmd)
                : MoveVertical(pos, dir, grid, cmd);
        }

        var gps = 0;
        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            if (grid[x, y] == '[')
                gps += (100 * y) + x;
        }
        
        return gps;
    }

    Point MoveHorizontal(Point pos, Point dir, Grid<char> grid, char cmd)
    {
        var checkPos = pos;
        var boxes = 0;

        while (true)
        {
            checkPos += dir;
            
            if (grid.At(checkPos) == '#')
                return pos;

            if (grid.At(checkPos) == '.')
                break;
            
            if ("[]".Contains(grid.At(checkPos)))
            {
                boxes++;
                checkPos += dir;
            }
        }
        
        for (int i = boxes; i > 0; i--)
        {
            grid.Set(checkPos, dir == Point.Left ? '[' : ']');
            checkPos -= dir;
            grid.Set(checkPos, dir == Point.Left ? ']' : '[');
            checkPos -= dir;
        }
        
        grid.Set(pos, '.');
        var nextPos = pos + dir;
        grid.Set(nextPos, cmd);

        return nextPos;
    }

    Point MoveVertical(Point pos, Point dir, Grid<char> grid, char cmd)
    {
        (Point, Point) ToBoxPoints(Point point)
        {
            if (grid.At(point) == '.' || grid.At(point) == '#')
                return (point, point);
            
            var point2 = grid.At(point) == '['
                ? point.MoveRight()
                : point.MoveLeft();
            
            var l = point.X < point2.X ? point : point2;
            var r = point.X > point2.X ? point : point2;

            return (l, r);
        }
        
        var boxes = new Stack<HashSet<(Point, Point)>>();
        boxes.Push([ToBoxPoints(pos + dir)]);

        while (true)
        {
            var currBoxes = boxes.Peek();

            if (currBoxes.Any(b => grid.At(b.Item1) == '#' || grid.At(b.Item2) == '#'))
                return pos;

            if (currBoxes.All(b => grid.At(b.Item1) == '.' && grid.At(b.Item2) == '.'))
                break;

            var newBoxes = new HashSet<(Point, Point)>();
            foreach (var box in currBoxes)
            {
                if (box.Item1 == box.Item2)
                    continue;
                
                var l = ToBoxPoints(box.Item1 + dir);
                var r = ToBoxPoints(box.Item2 + dir);

                newBoxes.Add(l);
                newBoxes.Add(r);
            }
            
            boxes.Push(newBoxes);
        }

        boxes.TryPop(out _);
        while (boxes.TryPop(out var level))
        {
            foreach (var box in level)
            {
                if (box.Item1 == box.Item2)
                    continue;
                
                grid.Set(box.Item1, '.');
                grid.Set(box.Item2, '.');
                
                var l = box.Item1 + dir;
                var r = box.Item2 + dir;
                
                grid.Set(l, '[');
                grid.Set(r, ']');
            }
        }

        grid.Set(pos, '.');
        var nextPos = pos + dir;
        grid.Set(nextPos, cmd);
        
        return nextPos; 
    }
}
