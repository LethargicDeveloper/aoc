using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    static readonly Dictionary<char, char> ParenPairClosed = new()
    {
        { ')', '(' },
        { ']', '[' },
        { '}', '{' },
        { '>', '<' }
    };

    static readonly Dictionary<char, int> Points = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _.ToCharArray());

        var pointCount = new Dictionary<char, int>
        {
            { ')', 0 },
            { ']', 0 },
            { '}', 0 },
            { '>', 0 }
        };

        var parenCount = new Dictionary<char, int>
        {
            { '(', 0 },
            { '[', 0 },
            { '{', 0 },
            { '<', 0 }
        };

        var stack = new Stack<char>();
        foreach (var line in lines)
            foreach (var paren in line)
            {
                if (parenCount.ContainsKey(paren))
                {
                    stack.Push(paren);
                }
                else
                {
                    var openParen = stack.Pop();
                    if (ParenPairClosed[paren] != openParen)
                    {
                        pointCount[paren]++;
                        break;
                    }
                }
            }

        return pointCount
            .Select(_ => _.Value * Points[_.Key])
            .Sum();
    }
}