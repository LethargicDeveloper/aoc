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

        var counts = positions
            .GroupBy()
            .OrderByDescending(_ => _.Count());

        var smallestDistance = counts
            .Select(count =>
            {
                var lt = positions.Where(_ => _ <= count.Key).Count();
                var gt = positions.Where(_ => _ > count.Key).Count();
                return (count.Key, diff: Math.Abs(lt - gt));
            })
            .MinBy(_ => _.diff).Key;

        return positions
            .Select(_ => Math.Abs(_ - smallestDistance))
            .Sum();
    }
}