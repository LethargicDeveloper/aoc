using AocLib;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023.Day19;

// 397061
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input.SplitEmptyLines();
        var program = lines[0]
            .SplitLines()
            .Select(_ => _.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries) switch
            {
                var x => (name: x[0], conditions: x[1].Split(','))
            });
        var args = lines[1]
            .SplitLines()
            .Select(_ => Regex.Matches(_, "\\d+") switch
            {
                var x =>
                (
                    x: int.Parse(x[0].Value),
                    m: int.Parse(x[1].Value),
                    a: int.Parse(x[2].Value),
                    s: int.Parse(x[3].Value)
                )
            });

        var builder = new CodeBuilder();
        foreach (var (name, conditions) in program)
        {
            var sb = new StringBuilder();
            foreach (var condition in conditions)
            {
                var parts = condition.Split(":");
                var @return = parts.Length == 2
                    ? parts[1] : parts[0];
                var cond = parts.Length == 2
                    ? parts[0] : null;

                if (cond != null)
                {
                    var @var = cond[0].ToString();
                    sb.Append($"if ({cond}) ");
                }

                if ("AR".Contains(@return))
                    sb.AppendLine($"return \"{@return}\" == \"A\";");
                else
                    sb.AppendLine($"return _{@return}(x, m, a, s);");
            }

            builder.AddFunction($"_{name}", sb.ToString(), "bool", ["long x", "long m", "long a", "long s"]);
        }

        long total = 0;
        var prog = builder.Build();
        foreach (var arg in args)
        {
            bool accept = prog._in(arg.x, arg.m, arg.a, arg.s);
            if (accept)
                total += arg.x + arg.m + arg.a + arg.s;
        }

        return total;
    }
}