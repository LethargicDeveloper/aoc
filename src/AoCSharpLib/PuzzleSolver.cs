using AoCSharpLib.AocTypes;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode;

[MemoryDiagnoser]
public abstract class PuzzleSolver<T>
{
    protected AocString input => internalInput;
    
    readonly string path;
    string internalInput;

    public PuzzleSolver()
    {
        var parts = GetType().FullName!.Split('.');
        var year = parts[1][1..];
        var day = parts[2];
        var part = parts[3].Replace("Part", string.Empty);

        path = Path.Combine(year, day);

        var filename = Path.Combine(path, $"{part}.txt");
        if (!File.Exists(path))
        {
            filename = Path.Combine(path, "01.txt");
        }

        internalInput = File.ReadAllText(filename);
    }

    [Benchmark]
    public abstract T Solve();

    public T Solve(string filename)
    {
        internalInput = File.ReadAllText(Path.Combine(path, $"{filename}.txt"));
        return Solve();
    }
}
