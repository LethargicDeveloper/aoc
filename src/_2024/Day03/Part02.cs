using System.Text.RegularExpressions;

namespace _2024.Day03;

[Answer(67269798)]
public partial class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        bool multiply = true;
        var answer = MulRegex().Matches(input)
            .Select(m =>
            {
                var op = m.Get("op");
                if (op == "mul")
                    return multiply ? m.ParseNumbers<long>().Product() : 0;
                
                multiply = op == "do";
                return 0;
            })
            .Sum();

        return answer;
    }

    [GeneratedRegex(@"(?<op>mul)\((?<val1>\d{1,3}),(?<val2>\d{1,3})\)|(?<op>do)\(\)|(?<op>don't)\(\)")]
    private static partial Regex MulRegex();
}
