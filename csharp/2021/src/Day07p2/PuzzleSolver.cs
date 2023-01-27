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
        var positions = input
            .Split(',')
            .Select(int.Parse);

        return Enumerable
            .Range(positions.Min(), positions.Max())
            .Select(r => positions.Select(p => CostToPosition(p, r)).Sum())
            .Min();
    }

    static int CostToPosition(int start, int end)
    {
        var n = Math.Abs(start - end);
        return (n * (n + 1)) / 2;
    }
}