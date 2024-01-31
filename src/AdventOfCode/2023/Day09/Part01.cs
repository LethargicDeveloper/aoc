using AocLib;

namespace AdventOfCode._2023.Day09;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var histories = input.SplitLines();

        long total = 0;
        foreach (var history in histories)
        {
            var initial = history
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            var list = new List<List<long>> { initial };
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
