
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;

namespace _2025.Day10;

public partial class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Select(line => LineRegex().Match(line).GetMatches())
            .Select(parts =>
            {
                var (_, buttons, joltage) = parts;

                var target = joltage![1..^1].Split(',').Select(int.Parse).ToArray();
                
                return (
                    Buttons: buttons!.Split(' ').Select(b => b[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray(),
                    Target: target
                );
            })
            .ToList();

        long total = 0;
        
        foreach (var (buttons, target) in lines)
        {
            var solver = Solver.CreateSolver("SCIP");
            
            var variables = buttons
                .Select((_, i) => solver.MakeIntVar(0.0, double.PositiveInfinity, $"x{i}"))
                .ToArray();
            
            for (int i = 0; i < target.Length; i++)
            {
                var sum = new LinearExpr();
                for (int j = 0; j < buttons.Length; j++)
                {
                    if (buttons[j].Contains(i))
                        sum += variables[j];
                }
                
                solver.Add(sum == target[i]);
            }

            var objective = new LinearExpr();
            for (int i = 0; i < buttons.Length; i++)
                objective += variables[i];
            
            solver.Minimize(objective);
            
            var status = solver.Solve();

            if (status != Solver.ResultStatus.OPTIMAL)
                throw new Exception("I fear you have failed.");
            
            total += (long)variables.Sum(v => v.SolutionValue());
        }

        return total;
    }

    [GeneratedRegex(@"^(\[.*\])\s(.*?)\s(\{.*?\})$")]
    private static partial Regex LineRegex();
}