namespace AocLib.Utility;

internal readonly record struct Maybe<T>(T Value, bool HasValue)
    where T : struct
{
    public static Maybe<T> Some(T value) => new(value, true);
    public static Maybe<T> None() => new(default!, false);
}
