using BenchmarkDotNet.Attributes;
using System.Text;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public string Solve()
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

    void NextCycle(StringBuilder crt, int pos, ref int cycles)
    {
        int x = (++cycles - 1) % 40;

        crt.Append(x >= pos - 1 && x <= pos + 1 ? "#" : ".");
        if (x == 39) crt.AppendLine();
    }
}