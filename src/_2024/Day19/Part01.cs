namespace _2024.Day19;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var towels = input.SplitEmptyLines();
        
        var have = towels[0].Split(", ").ToHashSet();
        var want = towels[1].SplitLines();
        
        var canMake = new List<List<string>>();

        foreach (var towel in want)
        {
            var queue = new Queue<(string Remaining, List<string> Path)>();
            foreach (var part in GetTowelParts(towel).Where(t => have.Contains(t.TowelPart)))
                queue.Enqueue((part.Remaining, [part.TowelPart]));

            while (queue.TryDequeue(out var item))
            {
                var (remaining, path) = item;
                if (string.Join("", path) == towel)
                {
                    canMake.Add(path);
                    break;
                }

                foreach (var part in GetTowelParts(remaining).Where(t => have.Contains(t.TowelPart)))
                {
                    List<string> newPath = [..path, part.TowelPart];
                    if (have.Add(string.Join("", newPath)))
                        queue.Enqueue((part.Remaining, newPath));
                }
            }
        }
        
        return canMake.Count;
    }

    IEnumerable<(string TowelPart, string Remaining)> GetTowelParts(string towel)
    {
        for (int i = 1; i <= towel.Length; i++)
            yield return (towel[..i], towel[i..]);
    }
}
