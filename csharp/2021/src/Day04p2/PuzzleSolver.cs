using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var lines = input.SplitLines();
        var nums = lines
            .First()
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var boards = lines
            .Skip(1)
            .WithIndex()
            .GroupBy(_ => _.Index / 5)
            .Select(g => Board.Create(g.Select(_ => _.Value)))
            .ToList();

        var loser = GetLosingBoard(nums, boards);
        return loser.GetWinningValue();
    }

    static Board GetLosingBoard(List<int> nums, List<Board> boards)
    {
        var stack = new Stack<Board>();
        foreach (var num in nums)
            foreach (var board in boards.Where(_ => !_.Winner))
            {
                board.SetNumber(num);
                if (board.Winner)
                    stack.Push(board);
            }

        stack.TryPop(out Board? loser);

        return loser!;
    }
}

class Board
{
    const int SIZE = 5;

    (int val, bool set)[][] board = new (int, bool)[SIZE][];
    int winningNum;

    public bool Winner { get; private set; }

    public static Board Create(IEnumerable<string> lines) =>
        new()
        {
            board = lines
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => (val: int.Parse(n), set: false))
                .ToArray())
            .ToArray()
        };

    public void SetNumber(int num)
    {
        if (Winner) return;

        for (int x = 0; x < SIZE; ++x)
            for (int y = 0; y < SIZE; ++y)
            {
                if (board[y][x].val == num)
                {
                    board[y][x].set = true;
                    this.winningNum = num;
                    Winner = CheckWinner(x, y);
                    return;
                }
            }
    }

    public long GetWinningValue()
    {
        if (!Winner) return -1;
        return board
            .SelectMany()
            .Where(_ => !_.set)
            .Sum(_ => _.val) * this.winningNum;
    }

    bool CheckWinner(int x, int y)
    {
        int count = 0;
        for (int xx = 0; xx < 5; ++xx)
            if (board[y][xx].set) ++count;
        if (count == 5) return true;

        count = 0;
        for (int yy = 0; yy < 5; ++yy)
            if (board[yy][x].set) ++count;
        return count == 5;
    }
}

