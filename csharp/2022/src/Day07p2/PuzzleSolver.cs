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
        var fs = ParseFileSystem();
        var unused = 70000000 - fs.First(_ => _.Name == "/").TotalSize;

        return fs
            .Where(_ => _.Name != "/")
            .Select(_ => _.TotalSize)
            .Where(_ => unused + _ > 30000000)
            .Min();
    }

    HashSet<Dir> ParseFileSystem() =>
        input
            .SplitLines()
            .Aggregate((fs: new HashSet<Dir>(), wd: (Dir?)null), (acc, cur) =>
            {
                var (fs, wd) = acc;

                if (cur.StartsWith("$ cd .."))
                    return (fs, wd?.Parent);

                if (cur.StartsWith("$ cd "))
                {
                    var dir = new Dir
                    {
                        Name = cur[5..],
                        Parent = wd
                    };

                    fs.Add(dir);
                    wd?.Children.Add(dir);

                    return (fs, dir);
                }

                if (long.TryParse(cur.Split(" ")[0], out var size))
                {
                    wd!.Size += size;
                    return (fs, wd);
                }

                return (fs, wd);
            }).fs;

    record Dir
    {
        public string Name { get; init; } = string.Empty;
        public long Size { get; set; }
        public long TotalSize => Size + Children.Sum(_ => _.TotalSize);
        public Dir? Parent { get; init; } 
        public List<Dir> Children { get; init; } = new();
    }
}