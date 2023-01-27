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
        var fishes = input
            .SplitLines()
            .SelectMany(_ => _.Split(','))
            .Select(int.Parse)
            .ToList();

        var newFishes = new List<int>();
        for (int day = 0; day < 80; ++day)
        {
            for (int i = 0; i < fishes.Count; ++i)
            {
                if (--fishes[i] < 0)
                {
                    fishes[i] = 6;
                    newFishes.Add(8);
                }
            }

            fishes.AddRange(newFishes);
            newFishes.Clear();
        }

        return fishes.Count;
    }
}