namespace AocLib;

public static class EnumerableExtensions
{
    public static List<List<T>> ToCollections<T>(this IEnumerable<IEnumerable<T>> lists) =>
        lists.Select(l => l.ToList()).ToList();
}