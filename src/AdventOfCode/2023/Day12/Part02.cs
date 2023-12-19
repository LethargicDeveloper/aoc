using AocLib;
using MoreLinq;

namespace AdventOfCode._2023.Day12;

// > 67275368117
// > 66713187062
// > 59547177710
public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _.Split(' ') switch
            {
                var x =>
                (
                    rec: x[0],
                    con: x[1].Split(',').Select(int.Parse).ToList()
                )
            })
            .ToList();

        foreach (var line in lines)
        {
            var permutations = GetPermutations(line).ToList();
        }

        return 0;
    }

    IEnumerable<string> GetPermutations((string rec, List<int> con) line)
    {
        var queue = new Queue<(string rec, bool first)>();
        queue.Enqueue((line.rec, true));

        for (int i = 0; i < line.rec.Length; i++)
        {
            if (line.rec[i] != '?')
                continue;

            while (queue.TryDequeue(out var rec))
            {
                var p1 = (rec.rec.ReplaceCharAt(i, '#'), false);
                var p2 = (rec.rec.ReplaceCharAt(i, '.'), rec.first);

                queue.Enqueue(p1);
                queue.Enqueue(p2);

                if (IsValid(p1.Item1, line.con)) yield return p1.Item1;
                if (IsValid(p2.Item1, line.con)) yield return p2.Item1;

                if (rec.first) break;
            }
        }
    }

    bool IsValid(string rec, List<int> con)
    {
        var group = rec
            .Replace('?', '.')
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        if (group.Count != con.Count)
            return false;

        return group.Zip(con).All(_ => _.First.Length == _.Second);
    }
}


