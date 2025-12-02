using MoreLinq.Extensions;

namespace _2025.Day02;

public class Part02 : PuzzleSolver<long>
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

    bool HasRepeatingPattern(string id)
    {
        for (var i = 0; i < id.Length / 2; i++)
        {
            if (HasRepeatingPattern(id, i + 1))
                return true;
        }
        
        return false;
    }
    
    bool HasRepeatingPattern(string id, int offset)
    {
        if (id.Length % offset != 0)
            return false;
        
        var pattern = id[..offset];
        
        for (int i = offset; i <= id.Length - offset; i += offset)
        {
            var ix = id[i..(i + offset)];
            if (ix != pattern) return false;
        }

        return true;
    }
}
