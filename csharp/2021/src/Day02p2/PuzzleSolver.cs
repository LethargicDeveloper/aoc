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
    public long Solve()
    {
        var (_, depth, position) = input
            .SplitLines()
            .Select(ParseCommand)
            .Aggregate((Aim: 0, Depth: 0, Position: 0), (acc, cur) =>
                cur.Operation switch
                {
                    "depth" => (acc.Aim + cur.Value, acc.Depth, acc.Position),
                    "forward" => (acc.Aim, acc.Depth + (acc.Aim * cur.Value), acc.Position + cur.Value),
                    _ => acc
                });

        return depth * position;
    }

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