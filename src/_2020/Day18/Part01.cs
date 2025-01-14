using System.Collections;
using System.Text;

namespace _2020.Day18;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var problems = input
            .Replace(" ", "")
            .SplitLines();

        long total = 0;

        for (int i = 0; i < problems.Length; i++)
        {
            var problem = problems[i];
            
            var stack = new Stack<(string Token, long? Value)>();
            
            while (problem.Length > 0)
            {
                (var token, problem) = GetToken(problem);
                if (long.TryParse(token, out long num))
                {
                    stack.Push((token, num));
                }

                if ("+*".Contains(token))
                {
                    stack.Push((token, null));
                }

                if (token == "(")
                {
                    stack.Push((token, null));
                }

                if (token == ")")
                {
                    var tokens = stack.PopUntil(v => v.Token == "(").ToList();
                    var result = Eval(tokens);
                    stack.Push((result.ToString(), result));
                }
            }

            var subtotal = Eval(stack.ToList());
            total += subtotal;
        }

        return total;
    }

    long Eval(List<(string Token, long? Value)> tokens)
    {
        long? total = null;

        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
            if (token.Token == "(")
                continue;

            if (total == null)
                total = token.Value;
            else
            {
                var op = token.Token;
                var num = tokens[--i].Value;
                total = op switch
                {
                    "+" => total + num,
                    "*" => total * num,
                    _ => throw new Exception($"Unexpected token {tokens[i].Token}")
                };
            }
        }
        
        return total ?? 0;
    }

    (string, string) GetToken(string problem)
    {
        var token = problem[0] switch
        {
            var x when char.IsDigit(x) => problem.TakeWhile(char.IsDigit).AsString(),
            _ => $"{problem[0]}",
        };

        return (token, problem[(token.Length)..]);
    }
}