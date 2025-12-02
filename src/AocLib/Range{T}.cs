using System.Collections;
using System.Numerics;

namespace AocLib;

public class AocRange<T>(T start, T end) : IEnumerable<T>
    where T : INumber<T>
{
    public T Start { get; set; } = start;
    public T End { get; set; } = end;

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
