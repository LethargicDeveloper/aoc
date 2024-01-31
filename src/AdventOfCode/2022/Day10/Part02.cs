using AocLib;
using System.Text;

namespace AdventOfCode._2022.Day10;

public partial class Part02 : PuzzleSolver<string>
{
    static void NextCycle(StringBuilder crt, int pos, ref int cycles)
    {
        int x = (++cycles - 1) % 40;

        crt.Append(x >= pos - 1 && x <= pos + 1 ? "#" : ".");
        if (x == 39) crt.AppendLine();
    }

    protected override string InternalSolve()
    {
        var cmds = input
            .SplitLines()
            .Select(_ => _.Split(' '));

        var crt = new StringBuilder();
        var pos = 1;
        var cycles = 0;

        foreach (var cmd in cmds)
        {
            NextCycle(crt, pos, ref cycles);

            if (cmd[0] == "addx")
            {
                NextCycle(crt, pos, ref cycles);
                pos += int.Parse(cmd[1]);
            }
        }

        return crt.ToString();
    }
}
