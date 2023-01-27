using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _.Select(c => c == '9' ? c : 'O').ToList())
            .ToList();

        var basins = new List<int>();
        for (int y = 0; y < lines.Count; ++y)
            for (int x = 0; x < lines[0].Count; ++x)
                if (lines[y][x] == 'O')
                    basins.Add(FillCount(ref lines, (x, y)));

        return basins
            .OrderDescending()
            .Take(3)
            .Product();
    }

    static int FillCount(ref List<List<char>> grid, Point position)
    {
        var points = new Stack<Point>();
        var charToChange = grid[position.Y][position.X];
        points.Push(position);

        int fillCount = 0;
        while (points.Count > 0)
        {
            var point = points.Pop();
            if (grid[point.Y][point.X] == charToChange)
            {
                grid[point.Y][point.X] = 'X';
                if (point.X - 1 >= 0)
                    points.Push(point.Left());
                if (point.X + 1 < grid[0].Count)
                    points.Push(point.Right());
                if (point.Y - 1 >= 0)
                    points.Push(point.Up());
                if (point.Y + 1 < grid.Count)
                    points.Push(point.Down());
                fillCount++;
            }
        }

        return fillCount;
    }
}