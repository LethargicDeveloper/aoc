using System.Text.RegularExpressions;
using AocLib;
using MathNet.Numerics.LinearAlgebra;

namespace _2024.Day13;

public class Part02 : PuzzleSolver<double>
{
    record ClawMachine(Point ButtonA, Point ButtonB, Point Prize);
    
    protected override double InternalSolve()
    {
        var clawMachines = input
            .SplitEmptyLines()
            .Select(l => Regex.Matches(l, "[+]?\\d+")
                    .Select(m => int.Parse(m.Value))
                    .ToList() switch
                {
                    [var ax, var ay, var bx, var by, var px, var py] =>
                        new ClawMachine((ax, ay), (bx, by), (px + 10000000000000, py + 10000000000000)),
                    _ => throw new FormatException()
                });

        var tokens = clawMachines
            .Select(SolveFewestTokens)
            .Sum();

        return tokens;
    }

    double SolveFewestTokens(ClawMachine clawMachine)
    {
        var (buttonA, buttonB, prize) = clawMachine;

        var a = Matrix<double>.Build.DenseOfArray(new double[,]
        {
            { buttonA.X, buttonB.X },
            { buttonA.Y, buttonB.Y },
        });

        var b = Vector<double>.Build.Dense([prize.X, prize.Y]);

        var s = a.Solve(b)
             .Where(v =>
             {
                 var dec = v - Math.Truncate(v);
                 return MathEx.Approximate(dec, 1) || MathEx.Approximate(dec, 0);
             })
            //.Select(v => MathF.Round(v))
            .ToList();
        
        return s.Count < 2 ? 0 : 3.0 * s[0] + s[1];
    }
}
