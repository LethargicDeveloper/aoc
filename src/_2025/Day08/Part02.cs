using QuikGraph;
using QuikGraph.Algorithms;

namespace _2025.Day08;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var boxes = input.SplitLines().Select(Point<long>.Parse).ToList();

        var pairs = (
                from b1 in boxes
                from b2 in boxes
                where b1 != b2
                orderby b1.Distance(b2)
                select (Box1: b1, Box2: b2)
            )
            .DistinctBy(p => string.CompareOrdinal($"{p.Box1}", $"{p.Box2}") < 1 ? (p.Box1, p.Box2) : (p.Box2, p.Box1))
            .ToList();

        var graph = new UndirectedGraph<Point<long>, Edge<Point<long>>>();

        foreach (var pair in pairs)
        {
            graph.AddVerticesAndEdge(new Edge<Point<long>>(pair.Box1, pair.Box2));
            
            var dict = new Dictionary<Point<long>, int>();
            graph.ConnectedComponents(dict);

            if (dict.All(d => d.Value == 0) && dict.Keys.Count == boxes.Count)
                return pair.Box1.X * pair.Box2.X;
        }

        throw new Exception("You done messed up.");
    }
}

