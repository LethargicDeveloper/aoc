using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    static readonly Dictionary<char, char> ParenPairOpen = new()
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
        { '<', '>' }
    };

    static readonly Dictionary<char, char> ParenPairClosed = new()
    {
        { ')', '(' },
        { ']', '[' },
        { '}', '{' },
        { '>', '<' }
    };

    static readonly Dictionary<char, int> Points = new()
    {
        { ')', 1 },
        { ']', 2 },
        { '}', 3 },
        { '>', 4 }
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

        var points = new List<long>();
        foreach (var line in lines)
        {
            var stack = new Stack<char>();
            foreach (var paren in line)
            {
                if (ParenPairOpen.ContainsKey(paren))
                {
                    stack.Push(paren);
                }
                else
                {
                    var openParen = stack.Pop();
                    if (ParenPairClosed[paren] != openParen)
                    {
                        stack.Clear();
                        break;
                    }
                }
            }

            long point = 0;
            bool addPoints = false;
            while (stack.Count > 0)
            {
                addPoints = true;
                var p = stack.Pop();
                var closedParen = ParenPairOpen[p];
                point = (point * 5) + Points[closedParen];
            }

            if (addPoints)
                points.Add(point);
        }

        return points.OrderBy(_ => _).ToList()[(points.Count - 1) / 2];
    }
}