using AocLib;

namespace _2024.Day15;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var data = input.SplitEmptyLines();
        
        var grid = data[0].ToCharGrid();
        var cmds = data[1]
            .SplitLines()
            .SelectMany()
            .ToArray();

        var pos = grid.Find('@');

        foreach (var cmd in cmds)
        {
            var boxes = 0;
            var moveCount = 0;
            var checkPos = pos;
            var nextPos = pos;
            var dir = cmd switch
            {
                '^' => Point.Up,
                'v' => Point.Down,
                '<' => Point.Left,
                '>' => Point.Right,
                _ => throw new Exception($"Invalid direction {cmd}")
            };
            
            while (grid.At(checkPos) != '#')
            {
                checkPos += dir;
                moveCount++;

                if (grid.At(checkPos) == 'O')
                    boxes++;
                
                if (grid.At(checkPos) == '.')
                    break;
            }

            for (; moveCount >= 0; checkPos += dir * -1, moveCount--)
            {
                if (grid.At(checkPos) == '#')
                    continue;

                switch (boxes)
                {
                    case > 0:
                        grid.Set(checkPos, 'O');
                        boxes--;
                        continue;
                    case 0:
                        grid.Set(checkPos, '@');
                        nextPos = checkPos;
                        boxes--;
                        continue;
                    default:
                        grid.Set(checkPos, '.');
                        break;
                }
            }
            
            pos = nextPos;
        }

        var gps = 0;
        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            if (grid[x, y] == 'O')
                gps += (100 * y) + x;
        }
        
        return gps;
    }
}
