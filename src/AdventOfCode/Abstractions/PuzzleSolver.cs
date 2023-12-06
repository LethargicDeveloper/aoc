using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Abstractions;

[MemoryDiagnoser]
public class PuzzleSolver<T> : IPuzzleSolver<T>
{
    protected string input => internalInput;
    readonly string path;
    string internalInput;

    public PuzzleSolver()
    {
        var parts = GetType().FullName?.Split(".")!;
        var year = parts[1][1..];
        var day = parts[2];
        var part = parts[3].Replace("Part", string.Empty);
        path = $@".\{year}\{day}";
        var filename = $@"{path}\{part}.txt";

        if (!File.Exists(path))
        {
            filename = $@"{path}\01.txt";
        }

        internalInput = File.ReadAllText(filename);
    }

    [Benchmark]
    public virtual T? Solve() => default;

    public T? Solve(string filename)
    {
        internalInput = File.ReadAllText($@"{path}\{filename}.txt");

        return Solve();
    }
}
