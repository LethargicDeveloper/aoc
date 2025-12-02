using MoreLinq.Extensions;

namespace _2025.Day02;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .S(',')
            .Select(x => x.Split('-'))
            .SelectMany(ValidIds)
            .Sum();
    }

    IEnumerable<long> ValidIds(string[] ids)
    {
        var start = long.Parse(ids[0]);
        var end = long.Parse(ids[1]);
        
        return new AocRange<long>(start, end)
            .Select(x => x.ToString())
            .Where(id => id[0] == '0' || HasRepeatingPattern(id))
            .Select(long.Parse);
    }

    bool HasRepeatingPattern(string id) => id[..(id.Length/2)] == id[(id.Length/2)..];
}

