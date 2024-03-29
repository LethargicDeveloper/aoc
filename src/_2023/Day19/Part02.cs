using AocLib;

namespace _2023.Day19;

// 125657431183201
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var lines = input.SplitEmptyLines();
        var funcs = lines[0]
            .SplitLines()
            .Select(_ => _.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries) switch
            {
                var x => (name: x[0], conditions: x[1].Split(','))
            })
            .ToDictionary(k => k.name, v => v.conditions);

        var state = new Dictionary<char, Range>
        {
            { 'x', 1..4000 },
            { 'm', 1..4000 },
            { 'a', 1..4000 },
            { 's', 1..4000 }
        };

        var queue = new Queue<(Dictionary<char, Range> vals, string func)>();
        queue.Enqueue((state, "in"));

        var accepted = new List<Dictionary<char, Range>>();

        while (queue.TryDequeue(out var perm))
        {
            if (perm.func == "A")
            {
                accepted.Add(perm.vals);
                continue;
            }

            if (perm.func == "R")
                continue;

            var conditions = funcs[perm.func];
            var newState = perm.vals.ToDictionary(_ => _.Key, _ => _.Value);

            foreach (var condition in conditions)
            {
                var parts = condition.Split(":");
                var @return = parts.Length == 2
                    ? parts[1] : parts[0];
                var cond = parts.Length == 2
                    ? parts[0] : null;

                if (cond != null)
                {
                    var @var = cond[0];
                    var comp = cond[1];
                    var val = int.Parse(cond[2..]);

                    var prev = newState[@var];
                    if (comp == '>')
                    {
                        newState[@var] = (val + 1)..prev.End.Value;
                        queue.Enqueue((newState, @return));

                        newState = newState.ToDictionary(k => k.Key, k => k.Value);
                        newState[@var] = prev.Start.Value..val;
                    }
                    else
                    {
                        newState[@var] = prev.Start.Value..(val - 1);
                        queue.Enqueue((newState, @return));

                        newState = newState.ToDictionary(k => k.Key, k => k.Value);
                        newState[@var] = val..prev.End.Value;
                    }
                }
                else
                {
                    queue.Enqueue((newState, @return));
                }
            }
        }

        Console.Clear();
        Console.WriteLine($"\tX\t\tM\t\tA\t\tS");
        foreach (var acc in accepted)
        {
            Console.WriteLine($"{acc['x'],10}\t{acc['m'],10}\t{acc['a'],10}\t{acc['s'],10}");
        }
        Console.WriteLine();

        return accepted
            .Select(_ => _.Values.Select(v => v.End.Value - (long)v.Start.Value + 1))
            .Select(_ => _.Product())
            .Sum();
    }
}