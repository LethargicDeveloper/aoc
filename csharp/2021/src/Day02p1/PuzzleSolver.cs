using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve() => input
        .SplitLines()
        .Select(ParseCommand)
        .GroupBy(_ => _.Operation)
        .Select(_ => _.Sum(g => g.Value))
        .Product();

    static Command ParseCommand(string str)
    {
        var cmd = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var val = int.Parse(cmd[1]);
        return new Command
        (
            cmd[0] switch
            {
                "up" or "down" => "depth",
                _ => cmd[0]
            },
            cmd[0] == "up" ? -val : val
        );
    }

    record Command(string Operation, int Value);
}