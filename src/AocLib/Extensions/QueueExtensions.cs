namespace AocLib;

public static class QueueExtensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> list)
    {
        var queue = new Queue<T>();
        queue.EnqueueRange(list);

        return queue;
    }

    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> range)
    {
        foreach (var r in range)
            queue.Enqueue(r);
    }
}
