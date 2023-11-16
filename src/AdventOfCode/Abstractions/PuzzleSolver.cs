using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Abstractions;

[MemoryDiagnoser]
public class PuzzleSolver<T> : IPuzzleSolver<T>
{
    protected readonly string input;

    public PuzzleSolver()
    {
        var parts = GetType().FullName?.Split(".")!;
        var year = parts[1][1..];
        var day = parts[2];
        var part = parts[3].Replace("Part", string.Empty);
        var path = $@".\{year}\{day}\{part}.txt";

        if (!File.Exists(path))
        {
            path = $@".\{year}\{day}\01.txt";
        }

        input = File.ReadAllText(path);
    }

    [Benchmark]
    public virtual T? Solve() => default;
}
