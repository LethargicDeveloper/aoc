using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day07;

public partial class Part01 : PuzzleSolver<long>
{
    public override long Solve() => ParseFileSystem()
        .Select(_ => _.TotalSize)
        .Where(size => size <= 100000)
        .Sum();

    HashSet<Dir> ParseFileSystem() =>
       this.input
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
