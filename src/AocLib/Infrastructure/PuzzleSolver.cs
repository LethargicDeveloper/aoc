using System.Reflection;
using BenchmarkDotNet.Attributes;
using Spectre.Console;

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

        var parts = name?.Split(".")!;
        var day = parts[1];
        var part = parts[2].Replace("Part", string.Empty);
        path = $@"{day}";
        var filename = $@"{path}/{part}.txt";

        if (!File.Exists(path))
        {
            filename = $@"{path}/01.txt";
        }

        internalInput = File.ReadAllText(filename);
    }

    protected virtual T? InternalSolve() => default;

    [Benchmark]
    public void Solve()
    {
        var solution = InternalSolve();

#if DEBUG
        var answer = GetType().GetCustomAttribute<AnswerAttribute>()?.Value;
        var correct = solution?.Equals(Convert.ChangeType(answer, typeof(T)));
        var style = new Style(foreground: correct switch
        {
            true => Color.Green,
            false => Color.Red,
            _ => Color.White
        });
        
        var expected = correct.HasValue && !correct.Value
            ? $" - Expected: {answer}"
            : string.Empty;
        
        AnsiConsole.Write(new Text($"{solution}{expected}", style));
#endif
    }

    public void Solve(string filename)
    {
        internalInput = File.ReadAllText($@"{path}/{filename}.txt");
        Solve();
    }
}
