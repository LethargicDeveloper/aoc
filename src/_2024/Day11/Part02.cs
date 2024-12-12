using AocLib;

namespace _2024.Day11;

public class Part02 : PuzzleSolver<long>
{
    private const int MAX_BLINK = 75;
    private readonly Dictionary<string, long> hash = [];
    
    protected override long InternalSolve()
    {
        var stones = input
            .SplitR(' ')
            .Select(s => (Value: long.Parse(s), Depth: MAX_BLINK))
            .ToList();

        long total = 0;

        foreach (var stone in stones)
        {
            total += GetStoneCountAtDepth(stone);
        }
        
        return total;
    }

    long GetStoneCountAtDepth((long Value, int Depth) stone)
    {
        var (value, depth) = stone;
        var key = $"{value}-{depth}";

        if (hash.TryGetValue(key, out var count))
            return count;

        if (depth == 0)
            return 1;

        hash[key] = 0;
        
        if (value == 0)
        {
            hash[key] += GetStoneCountAtDepth((1, depth - 1));
            return hash[key];
        }
        
        if (value.NumberOfDigits() % 2 == 0)
        {
            var (stone1, stone2) = value.Split();
            hash[key] += GetStoneCountAtDepth((stone1, depth - 1));
            hash[key] += GetStoneCountAtDepth((stone2, depth - 1));
            return hash[key];
        }
        
        hash[key] += GetStoneCountAtDepth((value * 2024, depth - 1));
        return hash[key];
    }
}