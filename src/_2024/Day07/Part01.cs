using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day07;

public class Part01 : PuzzleSolver<long>
{
    Func<long, long, long>[] actions =
    [
        (long num1, long num2) => num1 + num2,
        (long num1, long num2) => num1 * num2
    ];
    
    protected override long InternalSolve()
    {
        var equations = input
            .SplitLines()
            .Select(x => Regex.Matches(x, @"\d+")
                .Select(m => long.Parse(m.Value)).ToList())
            .Select(x => (Answer: x[0], Numbers: x[1..]))
            .ToList();

        var solutions = 0L;
        
        foreach (var equation in equations)
        {
            var answer = equation.Answer;
            var numbers = equation.Numbers;
            
            var totals = new Queue<(long, int)>();
            foreach (var action in this.actions)
                totals.Enqueue((action(numbers[0], numbers[1]), 2));

            while (totals.TryDequeue(out var cur))
            {
                var (total, index) = cur;
                
                if (total == answer)
                {
                    solutions += answer;
                    break;
                }
                
                if (total > answer || index >= numbers.Count)
                    continue;
                
                foreach (var action in this.actions)
                    totals.Enqueue((action(total, numbers[index]), index + 1));
            }
        }

        return solutions;
    }
}
