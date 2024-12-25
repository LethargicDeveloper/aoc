namespace AocLib;

public static class DictionaryExtensions
{
    public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> added)
        where TKey : notnull
    {
        source.Add(added.Key, added.Value);
    }
}