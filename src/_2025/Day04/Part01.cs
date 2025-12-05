using MoreLinq.Extensions;

namespace _2025.Day04;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input.ToGrid();
        var forklift = new Grid<int>(grid.Width, grid.Height);

        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            if (grid[x, y] == '.')
                continue;
            
            foreach (var neighbor in new Point(x, y).Neighbors().Where(grid.InBounds))
            {
                forklift[neighbor]++;
            }
        }

        long count = 0;
        
        for (int y = 0; y < grid.Height; y++)
        for (int x = 0; x < grid.Width; x++)
        {
            if (grid[x, y] == '@' && forklift[x, y] < 4)
                count++;
        }

        return count;
    }
}

