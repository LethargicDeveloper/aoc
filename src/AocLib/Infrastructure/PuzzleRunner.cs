namespace AocLib;

public class PuzzleRunner<T>
    where T : IPuzzleSolver
{
    public static void Solve()
    {
#if !DEBUG
        BenchmarkDotNet.Running.BenchmarkRunner.Run<T>();
#else
        var solver = (IPuzzleSolver)Activator.CreateInstance(typeof(T))!;
        solver.Solve();
#endif
    }

    public static void Solve(string filename)
    {
#if !DEBUG
        BenchmarkDotNet.Running.BenchmarkRunner.Run<T>();
#else
        var solver = (IPuzzleSolver)Activator.CreateInstance(typeof(T))!;
        solver.Solve(filename);
#endif
    }
}