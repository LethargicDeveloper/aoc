using System.Reflection.Metadata.Ecma335;

namespace _2025.Day07;


public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Where(l => l.Contains('^'))
            .ToList();

        var start = lines[0].IndexOf('^');

        var nums = new List<List<(long Value, bool Fallthrough)>>();
        for (int n = 0; n < lines.Count; n++)
        {
            nums.Add([]);
            for (int k = 0; k <= n; k++)
            {
                if (n < 2 || k == 0 || k == n)
                {
                    nums[n].Add((2, false));
                    continue;
                }
                
                var pos = (start - n) + (k * 2);
                var symbol = lines[n][pos];
                
                var (lpv, rpv) = nums[n - 1][(k - 1)..(k + 1)];
                var gpv = nums[n - 2][k - 1];

                var lv = lpv.Fallthrough ? 0 : lpv.Value / (symbol == '^' ? 1 : 2);
                var rv = rpv.Fallthrough ? 0 : rpv.Value / (symbol == '^' ? 1 : 2);
                var gv = gpv.Fallthrough ? gpv.Value * (symbol == '^' ? 2 : 1) : 0;
                
                var num = lv + rv + gv;
                nums[n].Add((num, symbol != '^'));
            }
        }

        return nums[^1].Sum(n => n.Value) + nums[^2].Where(n => n.Fallthrough).Sum(n => n.Value);
    }
}

