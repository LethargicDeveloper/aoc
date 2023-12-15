using AocLib;
using MoreLinq;
using System.Text.RegularExpressions;

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
                var x => (rec: x[0], arr: x[1].Split(',').Select(int.Parse).ToArray())
            })
            .ToList();

        var totals = new List<long>();
        foreach (var count in GetPermutationCount(lines))
        {
            totals.Add(count);
        }

        Console.WriteLine();

        //var lines2 = lines
        //    .Select(_ => (_.rec, rotRec: RotateRecord(_.rec), _.arr, rotArr: _.arr.Rot(1)))
        //    .Select(_ => (r: _, eq: _.rotRec.TrimEnd('.') == $"{_.rec}?".TrimStart('.')))
        //    .Select(_ => (rec: _.eq ? $"{_.r.rec}?" : _.r.rotRec, arr: _.eq ? _.r.arr : _.r.rotArr));

        var lines2 = input
            .SplitLines()
            .Select(_ => _.Split(' ') switch
            {
                var x => (rec: $"{x[0]}", arr: x[1].Split(',').Select(int.Parse).ToArray())
            })
            .ToList();

        return lines2
            .WithIndex()
            .Select(_ => (_.Index, Value: GetPermutationCount(_.Value, 2)))
            //.Select(_ => (_.Index, Value: _.Value * totals[_.Index]))
            .Select(_ => (long)Math.Pow((_.Value / totals[_.Index]), 3) * _.Value)
            .Sum();
    }

    string RotateRecord(string rec)
    {
        var endIndex = Regex.Match(rec, "[?#]+\\.*$").Index;
        var end = rec[endIndex..];

        return $"{end}?{rec[..endIndex]}";
    }

    IEnumerable<long> GetPermutationCount(List<(string rec, int[] arr)> lines)
    {
        foreach (var line in lines)
        {
            yield return GetPermutationCount(line);
        }
    }

    long GetPermutationCount((string rec, int[] arr) line, int repeat = 1)
    {
        line.rec = $"{line.rec}?".Repeat(repeat).CreateString()[..^1];

        var perms = new Queue<(string rec, int[] arr)>();
        perms.Enqueue(line);

        for (int i = 0; i < line.rec.Length; ++i)
        {
            if (line.rec[i] == '.' || line.rec[i] == '#')
                continue;

            var count = perms.Count;
            for (int j = 0; j < count; ++j)
            {
                var perm = perms.Dequeue();
                perms.Enqueue(perm);

                var newperm = perm.rec.ReplaceCharAt(i, '#');
                perms.Enqueue((newperm, line.arr));
            }
        }

        long value = 0;
        while (perms.TryDequeue(out var perm))
        {
            var rec = perm.rec.Replace('?', '.');
            if (!IsValid((rec, perm.arr)))
                continue;

            value++;
        }

        Console.WriteLine($"{line.rec} - {value}");
        return value;

    }

    bool IsValid((string rec, int[] arr) line)
    { 
        bool isValid = true;
        var groups = line.rec.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (groups.Length != line.arr.Length)
            return false;

        for (int i = 0; i < groups.Length; ++i)
        {
            if (groups[i].Length != line.arr[i])
            {
                isValid = false;
                break;
            }
        }

        return isValid;
    }
}
