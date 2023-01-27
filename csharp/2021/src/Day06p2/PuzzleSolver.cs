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
            .GroupBy()
            .ToDictionary(g => g.Key, g => g.LongCount());

        Enumerable.Range(0, 9)
            .Except(fishes.Keys)
            .ToList()
            .ForEach(_ => fishes.TryAdd(_, 0));

        for (int day = 0; day < 256; ++day)
        {
            long newFish = 0;
            foreach (var fish in fishes.OrderBy(_ => _.Key))
            {
                if (fish.Key == 0)
                    newFish = fish.Value;
                else
                    fishes[fish.Key - 1] += fish.Value;
                
                fishes[fish.Key] = 0;
            }

            fishes[8] += newFish;
            fishes[6] += newFish;
        }

        return fishes.Values.Sum();
    }
}