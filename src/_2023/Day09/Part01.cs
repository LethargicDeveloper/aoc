using AocLib;

namespace _2023.Day09;

// 1743490457
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var histories = input
            .SplitLines()
            .Select(_ => _.S(' ')
                .Select(long.Parse)
                .ToList());

        long total = 0;
        foreach (var history in histories)
        {
            var list = new List<List<long>> { history };
            total += GetNextHistory(list);
        }

        return total;
    }

    long GetNextHistory(List<List<long>> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].All(_ => _ == 0))
                break;

            list.Add([]);
            for (int j = 1; j < list[i].Count; ++j)
            {
                list[i + 1].Add(list[i][j] - list[i][j - 1]);
            }
        }

        return list.Sum(_ => _[^1]);
    }
}
