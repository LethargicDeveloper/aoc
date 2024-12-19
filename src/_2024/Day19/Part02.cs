namespace _2024.Day19;

public class Part02 : PuzzleSolver<long>
{
    private HashSet<string> have = [];

    protected override long InternalSolve()
    {
        var towels = input.SplitEmptyLines();

        have = towels[0].Split(", ").ToHashSet();
        var want = towels[1].SplitLines();

        return want.Sum(GetTowelCombination);
    }

    long GetTowelCombination(string towel)
    {
        long total = 0;
        
        for (int i = 1; i <= towel.Length; i++)
        {
            var part = towel[..i];
            var rest = towel[i..];
                
            total += GetTowelCombination(part, rest);
        }
    
        return total;
    }

    private Dictionary<string, long> hash = []; 
    long GetTowelCombination(string part, string rest)
    {
        var key = $"{part}-{rest}";
        
        if (!have.Contains(part))
            return 0;
        
        if (hash.TryGetValue(key, out var count))
            return count;

        if (string.IsNullOrEmpty(rest))
            return 1;

        hash[key] = GetTowelCombination(rest);
        return hash[key];
    }
} 
