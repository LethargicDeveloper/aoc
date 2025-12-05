using System.Collections;
using System.Numerics;

namespace AocLib;

public class AocRange<T>(T start, T end) : IEnumerable<T>
    where T : INumber<T>
{
    public T Start { get; set; } = start;
    public T End { get; set; } = end;

    public IEnumerable<AocRange<T>> Merge(AocRange<T> range)
    {
        if (Overlaps(range))
        {
            yield return new AocRange<T>(
                start: MathEx.Min(Start, range.Start),
                end: MathEx.Max(End, range.End));
        }
        else
        {
            if (Start < range.Start)
            {
                yield return this;
                yield return range;
            }
            else
            {
                yield return range;
                yield return this;
            }
        }
    }

    public T Count => End - Start + T.One;

    public bool Contains(T value)
    {
        return value >= Start && value <= End;
    }
    
    public bool Overlaps(AocRange<T> range)
        => Start <= range.End && End >= range.Start;

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = Start; i <= End; i++)
            yield return i;
    }

    public override string ToString()
    {
        return $"{{ Start: {Start}, End: {End} }}";
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
