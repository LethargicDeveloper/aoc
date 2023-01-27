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
        var lines = input.SplitLines().ToList();

        var lowPoints = new Dictionary<Point, int>();
        for (int y = 0; y < lines.Count; ++y)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; ++x)
            {
                var value = line[x];

                var u_lowest = y <= 0 || value < lines[y - 1][x];
                var d_lowest = y >= lines.Count - 1 || value < lines[y + 1][x];
                var l_lowest = x <= 0 || value < line[x - 1];
                var r_lowest = x >= line.Length - 1 || value < line[x + 1];

                if (u_lowest && d_lowest && l_lowest && r_lowest)
                {
                    lowPoints.Add((x, y), (int)char.GetNumericValue(value) + 1);
                    lowPoints.Remove((x, y - 1));
                    lowPoints.Remove((x, y + 1));
                    lowPoints.Remove((x - 1, y));
                    lowPoints.Remove((x + 1, y));
                }
            }
        }

        return lowPoints.Sum(_ => _.Value);
    }
}