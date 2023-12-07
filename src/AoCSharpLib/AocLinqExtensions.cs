using AoCSharpLib.AocTypes;

namespace AoCSharpLib;

public static class AocLinqExtensions
{
    public static IEnumerable<AocString> Map(this IEnumerable<AocString> values, Func<AocString, string> func)
        => values.Select(func).Select(_ => new AocString(_));

    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> values, Func<T, R> func)
        => values.Select(func);

    public static AocInt Sum(this IEnumerable<AocString> list)
    {
        long sum = 0;
        foreach (var item in list)
            sum += (long)item;

        return sum;
    }
}
