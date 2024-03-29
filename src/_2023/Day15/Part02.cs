using AocLib;

using System.Text.RegularExpressions;

namespace _2023.Day15;

// 247763
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        static long Hash(string str)
            => str.Aggregate(0, (cur, acc) => (acc + cur) * 17 % 256);

        var items = input
            .Split(',')
            .Select(_ => Regex.Match(_, "(\\w+)[=-](\\d+)?"))
            .Select(_ => (
                label: _.Groups[1].Value,
                hash: Hash(_.Groups[1].Value),
                value: long.TryParse(_.Groups[2].Value, out var v) ? v : (long?)null))
            .ToList();

        var boxes = new Dictionary<long, List<(string label, long value)>>();
        foreach (var (label, hash, value) in items)
        {
            if (!boxes.ContainsKey(hash))
                boxes[hash] = [];

            if (value == null)
            {
                var box = boxes[hash];
                var index = box.FindIndex(_ => _.label == label);
                if (index != -1) box.RemoveAt(index);
            }
            else
            {
                var box = boxes[hash];
                var index = box.FindIndex(_ => _.label == label);
                if (index == -1)
                    box.Add((label, value.Value));
                else
                    box[index] = (label, value.Value);
            }
        }

        long total = 0;
        foreach (var key in boxes.Keys)
        {
            var box = boxes[key];
            for (int i = 0; i < box.Count; i++)
            {
                total += (key + 1) * (i + 1) * box[i].value;
            }
        }

        return total;
    }
}
