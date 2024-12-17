namespace AocLib;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AnswerAttribute(object value) : Attribute
{
    public object Value { get; } = value;
}