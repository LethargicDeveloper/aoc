namespace AoCSharpLib.AocTypes;

public readonly struct _AocChar(char chr)
{
    readonly char chr = chr;

    public override string ToString()
        => chr.ToString();

    public static implicit operator _AocChar(char chr)
        => new(chr);

    public static implicit operator char(_AocChar aocChar)
        => aocChar.chr;
}