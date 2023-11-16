namespace AdventOfCode.Abstractions;

public interface IPuzzleSolver { }

public interface IPuzzleSolver<T> : IPuzzleSolver
{
    T? Solve();
}
