using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day10;

public partial class Part01 : PuzzleSolver<long>
{
    static long SignalStrength(long x, long cycles) => x * cycles;

    public override long Solve()
    {
        long x = 1;
        long cycles = 1;
        var strengths = new List<long>();

        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(' '));

        foreach (var cmd in cmds)
        {
            if (++cycles % 40 == 20)
                strengths.Add(SignalStrength(x, cycles));

            if (cmd[0] == "addx")
            {
                x += long.Parse(cmd[1]);
                if (++cycles % 40 == 20)
                    strengths.Add(SignalStrength(x, cycles));
            }
        }

        return strengths.Sum();
    }
}
