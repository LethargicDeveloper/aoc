using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day08;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var boxes = input.SplitLines().Select(Point<long>.Parse).ToList();

        var graph = (
                from b1 in boxes
                from b2 in boxes
                where b1 != b2
                orderby b1.Distance(b2)
                select (Box1: b1, Box2: b2)
            )
            .DistinctBy(p => string.CompareOrdinal($"{p.Box1}", $"{p.Box2}") < 1 ? (p.Box1, p.Box2) : (p.Box2, p.Box1))
            .Take(1000)
            .Select(p => new Edge<Point<long>>(p.Box1, p.Box2))
            .ToUndirectedGraph<Point<long>, Edge<Point<long>>>();
        
        var dict = new Dictionary<Point<long>, int>();
        graph.ConnectedComponents(dict);

        return dict
            .GroupBy(kvp => kvp.Value, kvp => kvp.Key)
            .Select(g => g.Count())
            .OrderDescending()
            .Take(3)
            .Product();
    }
}
