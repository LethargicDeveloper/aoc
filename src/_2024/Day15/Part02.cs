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
            .Select(s => s.ToCharArray())
            .ToArray();

        var cmds = data[1]
            .SplitLines()
            .SelectMany()
            .ToArray();

        var pos = grid.FindPosition('@');

        var index = 0;
        foreach (var cmd in cmds)
        {
            grid[pos.Y][pos.X] = cmd;

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
        for (int y = 0; y < grid.Length; y++)
        for (int x = 0; x < grid[0].Length; x++)
        {
            if (grid[y][x] == '[')
                gps += (100 * y) + x;
        }
        
        return gps;
    }

    Point MoveHorizontal(Point pos, Point dir, char[][] grid, char cmd)
    {
        char At(Point point) => grid[point.Y][point.X];
        
        var checkPos = pos;
        var boxes = 0;

        while (true)
        {
            checkPos += dir;
            
            if (At(checkPos) == '#')
                return pos;

            if (At(checkPos) == '.')
                break;
            
            if ("[]".Contains(grid[checkPos.Y][checkPos.X]))
            {
                boxes++;
                checkPos += dir;
            }
        }
        
        for (int i = boxes; i > 0; i--)
        {
            grid[checkPos.Y][checkPos.X] = dir == Point.Left ? '[' : ']';
            checkPos -= dir;
            grid[checkPos.Y][checkPos.X] = dir == Point.Left ? ']' : '[';
            checkPos -= dir;
        }
        
        grid[pos.Y][pos.X] = '.';
        var nextPos = pos + dir;
        grid[nextPos.Y][nextPos.X] = cmd;

        return nextPos;
    }

    Point MoveVertical(Point pos, Point dir, char[][] grid, char cmd)
    {
        char At(Point point) => grid[point.Y][point.X];

        (Point, Point) ToBoxPoints(Point point)
        {
            if (At(point) == '.' || At(point) == '#')
                return (point, point);
            
            var point2 = At(point) == '['
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

            if (currBoxes.Any(b => At(b.Item1) == '#' || At(b.Item2) == '#'))
                return pos;

            if (currBoxes.All(b => At(b.Item1) == '.' && At(b.Item2) == '.'))
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
                
                grid[box.Item1.Y][box.Item1.X] = '.';
                grid[box.Item2.Y][box.Item2.X] = '.';
                
                var l = box.Item1 + dir;
                var r = box.Item2 + dir;
                
                grid[l.Y][l.X] = '[';
                grid[r.Y][r.X] = ']';
            }
        }
        
        grid[pos.Y][pos.X] = '.';
        var nextPos = pos + dir;
        grid[nextPos.Y][nextPos.X] = cmd;
        
        return nextPos; 
    }
}
