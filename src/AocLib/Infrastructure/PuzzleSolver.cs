using BenchmarkDotNet.Attributes;

namespace AocLib;

public interface IPuzzleSolver
{
    void Solve();
    void Solve(string filename);
}

[MemoryDiagnoser]
public class PuzzleSolver<T> : IPuzzleSolver
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "I like it the way it is.")]
    protected string input => internalInput;

    readonly string path;

    string internalInput;

    public PuzzleSolver()
    {
        var type = GetType();
        var name = (type.BaseType) == typeof(PuzzleSolver<T>)
            ? type.FullName : type.BaseType!.FullName;

        Console.WriteLine($"TYPE: {name}");
        var parts = name?.Split(".")!;
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

    protected virtual T? InternalSolve() => default;

    [Benchmark]
    public void Solve()
    {
        var solution = InternalSolve();

#if DEBUG
        Console.WriteLine(solution);
#endif
    }

    public void Solve(string filename)
    {
        internalInput = File.ReadAllText($@"{path}\{filename}.txt");
        Solve();
    }
}