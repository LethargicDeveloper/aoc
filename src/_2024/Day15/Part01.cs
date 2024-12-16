using AocLib;

namespace _2024.Day15;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var data = input.SplitEmptyLines();
        
        var grid = data[0]
            .SplitLines()
            .Select(s => s.ToCharArray())
            .ToArray();

        var cmds = data[1]
            .SplitLines()
            .SelectMany()
            .ToArray();

        var pos = grid.FindPosition('@');

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
            
            while (grid[checkPos.Y][checkPos.X] != '#')
            {
                checkPos += dir;
                moveCount++;

                if (grid[checkPos.Y][checkPos.X] == 'O')
                    boxes++;
                
                if (grid[checkPos.Y][checkPos.X] == '.')
                    break;
            }

            for (; moveCount >= 0; checkPos += dir * -1, moveCount--)
            {
                if (grid[checkPos.Y][checkPos.X] == '#')
                    continue;

                switch (boxes)
                {
                    case > 0:
                        grid[checkPos.Y][checkPos.X] = 'O';
                        boxes--;
                        continue;
                    case 0:
                        grid[checkPos.Y][checkPos.X] = '@';
                        nextPos = checkPos;
                        boxes--;
                        continue;
                    default:
                        grid[checkPos.Y][checkPos.X] = '.';
                        break;
                }
            }
            
            pos = nextPos;
        }

        var gps = 0;
        for (int y = 0; y < grid.Length; y++)
        for (int x = 0; x < grid[0].Length; x++)
        {
            if (grid[y][x] == 'O')
                gps += (100 * y) + x;
        }
        
        return gps;
    }
}
