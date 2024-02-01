namespace AocLib;

public static class CharExtensions
{
    public static string AsString(this IEnumerable<char> list)
        => new(list.ToArray());

    public static IEnumerable<char> Repeat(this char input, int count)
    {
        for (int i = 0; i < count; i++)
            yield return input;
    }
}
