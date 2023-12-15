﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;

namespace AocLib;

public static class LinqExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> list)
        => list.SelectMany(_ => _);

    public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> list)
        => list.Select((v, i) => (i, v));

    public static T Product<T>(this IEnumerable<T> list)
        where T : IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
        => list.Aggregate(T.MultiplicativeIdentity, (acc, cur) => acc * cur);

    public static T Subtract<T>(this IEnumerable<T> list)
        where T : INumber<T>
        => list.Aggregate((acc, cur) => acc - cur);

    public static IEnumerable<T> Loop<T>(this IEnumerable<T> list)
    {
        while (true)
            foreach (var item in list)
                yield return item;
    }

    public static T[][] Pivot<T>(this T[][] arr)
    {
        var min = arr.Select(_ => _.Length).Min();

        return arr
            .SelectMany(x => x.Take(min).Select((y, i) => new { val = y, idx = i }))
            .GroupBy(x => x.idx, x => x.val)
            .Select(x => x.ToArray())
            .ToArray();
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
