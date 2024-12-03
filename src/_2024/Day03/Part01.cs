using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day03;

public partial class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var answer = MulRegex()
            .Matches(input)
            .Select(match => match.Groups["op"].Value switch
            {
                "mul" => long.Parse(match.Groups["val1"].Value) * long.Parse(match.Groups["val2"].Value),
                _ => throw new Exception("Invalid Operation")
            })
            .Sum();
        
        return answer;
    }

    [GeneratedRegex(@"(?<op>mul)\((?<val1>\d{1,3}),(?<val2>\d{1,3})\)")]
    private static partial Regex MulRegex();
}
