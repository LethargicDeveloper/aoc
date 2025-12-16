namespace AocLib;

public static class EnumerableExtensions
{
    public static void Swap<T>(this IList<T> collection, int index1, int index2)
    {
        var tmp = collection.ElementAt(index1);
        collection[index1] = collection.ElementAt(index2);
        collection[index2] = tmp;
    }

    public static List<List<T>> ToLists<T>(this IEnumerable<IEnumerable<T>> lists) =>
        lists.Select(l => l.ToList()).ToList();
    
    public static T[][] ToArrays<T>(this IEnumerable<IEnumerable<T>> lists) =>
        lists.Select(l => l.ToArray()).ToArray();

    public static T[,] To2dArray<T>(this IEnumerable<IEnumerable<T>> lists)
    {
        var arr = lists.ToArrays();
        
        var height = arr.Length;
        var width = arr.Max(a => a.Length);

        var newArr = new T[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newArr[y, x] = arr[y].ElementAtOrDefault(x)!;
            }
        }

        return newArr;
    }
    
    public static long ComputeHash<T>(this IEnumerable<T> arr)
    {
        var hashCode = new HashCode();

        foreach (var item in arr)
            hashCode.Add(item);
        
        return hashCode.ToHashCode();
    }
}