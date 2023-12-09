using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;

namespace AocLib;

public static class LinqExtensions
{
    public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> list)
        => list.Select((v, i) => (i, v));

    public static T Product<T>(this IEnumerable<T> list)
        where T : IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        => list.Aggregate(T.MultiplicativeIdentity, (acc, cur) => acc * cur);

    public static IEnumerable<T> Loop<T>(this IEnumerable<T> list)
    {
        while (true)
            foreach (var item in list)
                yield return item;
    }

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
