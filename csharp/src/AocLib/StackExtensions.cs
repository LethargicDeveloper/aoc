﻿namespace AocLib;

public static class StackExtensions
{
    public static Stack<T> PushRange<T>(this Stack<T> stack, IEnumerable<T> list)
    {
        foreach (var item in list)
        {
            stack.Push(item);
        }

        return stack;
    }

    public static IEnumerable<T> PopRange<T>(this Stack<T> stack, int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            yield return stack.Pop();
        }
    }
}
