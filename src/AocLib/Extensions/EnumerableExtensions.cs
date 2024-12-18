namespace AocLib;

public static class EnumerableExtensions
{
    public static List<List<T>> ToLists<T>(this IEnumerable<IEnumerable<T>> lists) =>
        lists.Select(l => l.ToList()).ToList();
    
    public static T[][] ToArrays<T>(this IEnumerable<IEnumerable<T>> lists) =>
        lists.Select(l => l.ToArray()).ToArray();
}