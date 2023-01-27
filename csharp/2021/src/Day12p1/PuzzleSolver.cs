using BenchmarkDotNet.Attributes;
using System.Text;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var node in input
            .SplitLines()
            .Select(_ => _.Split('-')))
        {
            if (graph.ContainsKey(node[0]))
            {
                graph[node[0]].Add(node[1]);
            }
            else if (node[1] != "start")
            {
                graph.Add(node[0], new HashSet<string>() { node[1] });
            }

            if (graph.ContainsKey(node[1]))
            {
                graph[node[1]].Add(node[0]);
            }
            else if (node[0] != "start")
            {
                graph.Add(node[1], new HashSet<string>() { node[0] });
            }
        }

        var sb = new StringBuilder();

        var stack = new Stack<string>();
        var visited = new Stack<string>();
        long counter = 0;
        foreach (var startNode in graph["start"])
        {
            visited.Push("start");

            stack.Push(startNode);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (node == "done")
                {
                    while (stack.TryPeek(out string? r) && r == "done")
                    {
                        stack.Pop();
                        visited.Pop();
                    }

                    visited.Pop();
                    continue;
                }

                visited.Push(node);

                if (node == "end")
                {
                    sb.AppendLine(string.Join(",", visited.Reverse()));
                    counter++;
                    visited.Pop();
                    continue;
                }

                var children = graph[node]
                    .Where(_ => !visited.Where(v => char.IsLower(v[0])).Contains(_))
                    .ToList();

                if (children.Count > 0)
                {
                    stack.Push("done");
                    foreach (var child in children)
                        stack.Push(child);
                }
                else
                {
                    visited.Pop();
                }
            }

            visited.Clear();
        }

        return counter;
    }
}