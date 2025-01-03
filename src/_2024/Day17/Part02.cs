namespace _2024.Day17;

[Answer(164516454365621)]
public class Part02 : PuzzleSolver<double>
{
    protected override double InternalSolve()
    {
        var expected = input.SplitEmptyLines()[1].S(' ')[1];

        double total = 4;

        while (true)
        {
            var output = new Solution(total).Solve();
            if (output == expected)
                break;
            
            if (expected.EndsWith(output))
            {
                total *= 8;
                continue;
            }

            total++;
        }
        
        
        return total;
    }

    private class Solution(double registerA) : Part01(registerA)
    {
        public new string Solve() => base.InternalSolve();
    }
}