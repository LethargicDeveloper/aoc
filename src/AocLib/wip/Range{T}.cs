using System.Numerics;

namespace AocLib;

public class Range<T>(long start, long end)
    where T : INumber<T>
{
    public long Start { get; set; } = start;
    public long End { get; set; } = end;

    public override string ToString()
    {
        return $"{{ Start: {Start}, End: {End} }}";
    }
}
