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
        var grid = input
             .SplitLines()
             .Select(_ => _.Select(c => (Value: char.GetNumericValue(c), Flashed: false)).ToArray())
             .ToArray();

        int width = grid[0].Length;
        int height = grid.Length;

        int flashCount = 0;
        var flashes = new Stack<Point>();
        for (int step = 0; step < 100; ++step)
        {
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                    IncreaseEnergyLevel(x, y);

            while (flashes.Count > 0)
            {
                var (x, y) = flashes.Pop();
                if (grid[y][x].Flashed)
                    continue;

                grid[y][x].Flashed = true;

                IncreaseEnergyLevel(x - 1, y - 1);
                IncreaseEnergyLevel(x - 1, y);
                IncreaseEnergyLevel(x - 1, y + 1);
                IncreaseEnergyLevel(x, y + 1);
                IncreaseEnergyLevel(x + 1, y + 1);
                IncreaseEnergyLevel(x + 1, y);
                IncreaseEnergyLevel(x + 1, y - 1);
                IncreaseEnergyLevel(x, y - 1);
            }

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                    if (grid[y][x].Value > 9)
                    {
                        grid[y][x].Value = 0;
                        grid[y][x].Flashed = false;
                        flashCount++;
                    }
        }

        void IncreaseEnergyLevel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return;

            grid[y][x].Value++;
            if (grid[y][x].Value > 9 && !grid[y][x].Flashed)
                flashes.Push((x, y));
        }

        return flashCount;
    }
}