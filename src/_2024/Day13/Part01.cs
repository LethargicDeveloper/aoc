using System.Text.RegularExpressions;
using AocLib;

namespace _2024.Day13;

public class Part01 : PuzzleSolver<long>
{
    record ClawMachine(Point ButtonA, Point ButtonB, Point Prize);
    
    protected override long InternalSolve()
    {
        var clawMachines = input
            .SplitEmptyLines()
            .Select(l => Regex.Matches(l, "[+]?\\d+")
                    .Select(m => int.Parse(m.Value))
                    .ToList() switch
                {
                    [var ax, var ay, var bx, var by, var px, var py] =>
                        new ClawMachine((ax, ay), (bx, by), (px, py)),
                    _ => throw new FormatException()
                });

        var tokens = clawMachines
            .Select(SolveFewestTokens)
            .Sum();

        return tokens;
    }

    long SolveFewestTokens(ClawMachine clawMachine)
    {
        var (buttonA, buttonB, prize) = clawMachine;

        var tokens = long.MaxValue;
        
        for (int pressA = 0; pressA <= (prize.X / buttonA.X) ; pressA++)
        for (int pressB = 0; pressB <= (prize.Y / buttonB.Y) ; pressB++)
        {
            var x = (pressA * buttonA.X) + (pressB * buttonB.X);
            var y = (pressA * buttonA.Y) + (pressB * buttonB.Y);

            if ((x, y) == prize)
            {
                tokens = Math.Min(tokens, (3 * pressA) + pressB);
            }
        }
        
        return tokens == long.MaxValue ? 0 : tokens;
    }
}
