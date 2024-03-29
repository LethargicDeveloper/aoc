using AocLib;

namespace _2023.Day01;

// 54159
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(_ => _.Where(char.IsDigit).AsString())
            .Select(_ => $"{_[0]}{_[^1]}")
            .Select(int.Parse)
            .Sum();
    }
}