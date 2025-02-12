using AocLib;

namespace _2023.Day12;

// 7344
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input
            .SplitLines()
            .Select(_ => _
                .Split(' ')
                .Pipe(x => (rec: x[0], arr: x[1]
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray())))
            .ToList();

        long total = 0;
        foreach (var (rec, arr) in GetPermutations(lines))
        {
            bool isValid = true;
            var groups = rec.S('.');
            if (groups.Length != arr.Length)
                continue;

            for (int i = 0; i < groups.Length; ++i)
            {
                if (groups[i].Length != arr[i])
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid) total++;
        }

        return total;
    }

    IEnumerable<(string rec, int[] arr)> GetPermutations(List<(string rec, int[] arr)> lines)
    {
        foreach (var line in lines)
        {
            var perms = new Queue<(string rec, int[] arr)>();
            perms.Enqueue(line);

            var nextperms = new Queue<(string rec, int[] arr)>();

            int i = 0;
            do
            {
                if (line.rec[i] == '.' || line.rec[i] == '#')
                {
                    i++;
                    continue;
                }

                while (perms.TryDequeue(out var perm))
                {
                    nextperms.Enqueue(perm);
                    var newperm = perm.rec.ReplaceCharAt(i, '#');
                    nextperms.Enqueue((newperm, line.arr));
                }

                i++;

                perms = nextperms;
                nextperms = new Queue<(string rec, int[] arr)>();
            } while (i < line.rec.Length);

            while (perms.TryDequeue(out var perm))
                yield return (perm.rec.Replace('?', '.'), perm.arr);
        }
    }
}
