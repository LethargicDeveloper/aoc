using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace _2024.Day03;

[Answer(188741603)]
public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var answer = MulRegex()
            .Matches(input)
            .Select(match => match.ParseNumbers<long>())
            .Select(v => v[0] * v[1])
            .Sum();
        
        return answer;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();
}
