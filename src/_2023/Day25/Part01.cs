using AocLib;
using Microsoft.Diagnostics.Runtime;
using MoreLinq.Extensions;
using QuikGraph;

namespace _2023.Day25;

// 569904
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var graph = input
            .SplitLines()
            .SelectMany(_ => _.Split(':', StringSplitOptions.RemoveEmptyEntries) switch
            {
                var x => 
                    from c1 in x[0..1]
                    from c2 in x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    select new Edge<string>(c1, c2)
                
            })
            .ToUndirectedGraph<string, Edge<string>>();

        var a = GlobalMinCut(graph);

        return (graph.VertexCount - a.v.Count) * a.v.Count;
    }

    void Print(UndirectedGraph<string, Edge<string>> graph)
    {
        foreach (var vertex in graph.Vertices)
        {
            var edges = graph.AdjacentEdges(vertex);
            Console.WriteLine($"{vertex}: {string.Join(" ", edges.Select(_ => _.Target == vertex ? _.Source : _.Target))}");
        }
    }

    public static (int k, List<int> v) GlobalMinCut(UndirectedGraph<string, Edge<string>> graph)
    {
        var vertexes = graph.Vertices.ToList();
        var mat = new int[vertexes.Count][];
        for (int i = 0; i < vertexes.Count; i++)
            mat[i] = new int[vertexes.Count];

        foreach (var a in vertexes)
        {
            foreach (var b in vertexes)
            {
                if (graph.AdjacentEdges(a).Select(_ => _.Target == a ? _.Source : _.Target).Contains(b))
                {
                    mat[vertexes.FindIndex(_ => _ == a)][vertexes.FindIndex(_ => _ == b)] = 1;
                    mat[vertexes.FindIndex(_ => _ == b)][vertexes.FindIndex(_ => _ == a)] = 1;
                }
            }
        }

        var best = (k: int.MaxValue, v: new List<int>());
        int n = mat.Length;
        var co = new List<List<int>>();

        for (int i = 0; i < n; i++)
            co.Add([i]);

        for (int ph = 1; ph < n; ph++)
        {
            var w = mat[0].ToList();
            int s = 0, t = 0;

            for (int it = 0; it < n - ph; it++)
            {
                w[t] = int.MinValue;
                s = t;
                t = w.IndexOf(w.Max());
                for (int i = 0; i < n; i++)
                    w[i] += mat[t][i];
            }

            var newK = w[t] - mat[t][t];
            if (newK < best.k)
            {
                best = (newK, co[t]);
            }
            else if (newK == best.k)
            {
                var zip = best.v.Cast<int?>()
                    .ZipLongest(co[t].Cast<int?>(), (v1, v2) => (v1 ?? int.MinValue, v2 ?? int.MinValue));

                var cmp = zip.Select(v => v.Item2.CompareTo(v.Item1)).FirstOrDefault(_ => _ != 0);

                if (cmp == -1)
                    best = (newK, co[t]);
            }

            co[s].AddRange(co[t]);

            for (int i = 0; i < n; i++)
                mat[s][i] += mat[t][i];

            for (int i = 0; i < n; i++)
                mat[i][s] = mat[s][i];

            mat[0][t] = int.MinValue;
        }

        return best;
    }
}