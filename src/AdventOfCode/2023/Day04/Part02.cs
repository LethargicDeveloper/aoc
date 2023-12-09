using AocLib;
using MoreLinq;

namespace AdventOfCode._2023.Day04;

// 9496801
public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var cards = input
            .SplitLines()
            .Select(_ => _
                .Split(':')[1]
                .Split('|')
                .Select(r => r.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray())
            .Select((_, i) => (i, _[1].Intersect(_[0]).Count()))
            .ToList();

        var queue = new Queue<(int, int)>();
        foreach (var card in cards)
            queue.Enqueue(card);

        long ticketCount = 0;
        while (queue.TryDequeue(out var c))
        {
            var (card, count) = c;

            ticketCount++;

            Enumerable.Range(card + 1, Math.Min(count, cards.Count - card))
                .ForEach(i => queue.Enqueue(cards[i]));
        }

        return ticketCount;
    }
}
