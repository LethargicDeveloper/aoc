using QuikGraph;

namespace _2024.Day23;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var graph = input
            .SplitLines()
            .Select(s => s.Split('-'))
            .Select(s => new Edge<string>(s[0], s[1]))
            .ToBidirectionalGraph<string, Edge<string>>();
        
        
        
        return 0;
    }
}
