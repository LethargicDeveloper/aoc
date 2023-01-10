﻿namespace AocLib;

public static class LinqExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> list)
        => list.SelectMany(_ => _);

    public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> list)
        => list.Select((v, i) => (i, v));
}
