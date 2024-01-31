namespace AocLib;

public static class CharExtensions
{
    public static string AsString(this IEnumerable<char> list)
        => new(list.ToArray());
}
