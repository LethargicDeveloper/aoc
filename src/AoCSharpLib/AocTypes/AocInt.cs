namespace AoCSharpLib.AocTypes;

public readonly struct AocInt
{
    readonly long value;

    public AocInt(long value)
    {
        this.value = value;
    }

    public static implicit operator AocInt(int value)
        => new(value);

    public static implicit operator AocInt(long value)
        => new(value);

    public static implicit operator int(AocInt aocInt)
        => (int)aocInt.value;

    public static implicit operator long(AocInt aocInt)
        => aocInt.value;
}
