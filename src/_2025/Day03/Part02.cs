using MoreLinq.Extensions;

namespace _2025.Day03;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input.SplitLines()
            .Select(bank =>
            {
                var digits = new List<char>();
                
                for (int i = 0; i < bank.Length; i++)
                {
                    var remaining = 12 - digits.Count;
                    if (remaining == 0) break;
                    
                    var digit = bank[i..^(remaining - 1)].Max();
                    var index = bank.IndexOf(digit, i);
                    i = index;
                    
                    digits.Add(digit);
                }

                var joltage = long.Parse(digits.AsString());
                
                return joltage;
            })
            .Sum();
    }
}
