using System.Numerics;

namespace AocLib;

public static class LinqExtensions
{
    public static IEnumerable<T> Select<T>(this IEnumerable<T> list)
        => list.Select(_ => _);

    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> list)
        => list.SelectMany(_ => _);

    public static IEnumerable<IGrouping<T, T>> GroupBy<T>(this IEnumerable<T> list)
        => list.GroupBy(_ => _);

    public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> list)
        => list.Select((v, i) => (i, v));

    public static T Product<T>(this IEnumerable<T> list)
        where T : IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        => list.Aggregate(T.MultiplicativeIdentity, (acc, cur) => acc * cur);

    public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> childSelector)
    {
        foreach (var element in source)
        {
            var children = childSelector(element);
            foreach (var child in Flatten(children, childSelector))
            {
                yield return child;
            }

            yield return element;
        }
    }
}
