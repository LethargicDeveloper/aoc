using MoreLinq;
using QuikGraph;

namespace _2024.Day23;

public class Part02 : PuzzleSolver<string>
{
    protected override string InternalSolve()
    {
        var graph = input
            .SplitLines()
            .Select(s => s.Split('-'))
            .Select(s => new Edge<string>(s[0], s[1]))
            .ToUndirectedGraph<string, Edge<string>>();

        List<string> GetNeighbors(string v) =>
            graph.AdjacentEdges(v).Select(e => e.Target == v ? e.Source : e.Target).ToList();

        var path = graph
            .Vertices
            .Select(v =>
            {
                var n = GetNeighbors(v);
                var validN = n
                    .Where(x =>
                    {
                        var xn = GetNeighbors(x);
                        var valid = !n.Except(xn).Any();
                        return valid;
                    })
                    .OrderBy();
                return string.Join(",", validN);
            })
            .ToList();
            //.MaxBy(v => v.Length);

        return "";

        // var states = new Stack<(string, List<string>)>();
        // foreach (var vertex in graph.Vertices)
        //     states.Push((vertex, [vertex]));
        //
        // var bestPath = new List<string>();
        // while (states.TryPop(out var state))
        // {
        //     var (vertex, path) = state;
        //
        //     var neighbors = GetNeighbors(vertex)
        //         .Where(n => !path.Contains(n))
        //         .Where(n => !path.Except(GetNeighbors(n)).Any())
        //         .ToList();
        //
        //     path = [..path, ..neighbors];
        //
        //     // if (path.Count + neighbors.Count + 1 < bestPath.Count)
        //     //     continue;
        //
        //     if (path.Count > bestPath.Count)
        //     {
        //         bestPath = path;
        //     }
        //
        //     continue;
        //     
        //     foreach (var neighbor in neighbors)
        //     {
        //         states.Push((neighbor, [..path, neighbor]));
        //     }
        //}

        //return string.Join(",", bestPath.OrderBy());
    }
}
