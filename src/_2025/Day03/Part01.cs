using MoreLinq.Extensions;

namespace _2025.Day03;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input.SplitLines()
            .Select(bank =>
            {
                var num1 = bank[..^1].Max();
                var num2 = bank[(bank.IndexOf(num1) + 1)..].Max();
                var joltage = int.Parse($"{num1}{num2}");

                return joltage;
            })
            .Sum();
    }
}

