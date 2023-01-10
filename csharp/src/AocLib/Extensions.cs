namespace AocLib;

public static class Extensions
{
    public static void Log(this object obj)
        => Console.WriteLine(obj);

    public static string CreateString(this IEnumerable<char> list)
        => new(list.ToArray());
}
