using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day03;

public partial class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        bool multiply = true;
        long answer = 0;
        
        foreach (Match match in MulRegex().Matches(input))
        {
            switch (match.Groups["op"].Value)
            {
                case "mul":
                    answer += multiply
                        ? long.Parse(match.Groups["val1"].Value) * long.Parse(match.Groups["val2"].Value)
                        : 0;
                    break;
                case "do":
                    multiply = true;
                    break;
                case "don't":
                    multiply = false;
                    break;
            }
        }

        return answer;
    }

    [GeneratedRegex(@"(?<op>mul)\((?<val1>\d{1,3}),(?<val2>\d{1,3})\)|(?<op>do)\(\)|(?<op>don't)\(\)")]
    private static partial Regex MulRegex();
}
