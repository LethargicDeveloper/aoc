using AocLib;

namespace AdventOfCode._2023.Day01;

// 54159
public class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        return input
            .SplitLines()
            .Select(_ => _.Where(char.IsDigit).CreateString())
            .Select(_ => $"{_[0]}{_[^1]}")
            .Select(int.Parse)
            .Sum();
    }
}
