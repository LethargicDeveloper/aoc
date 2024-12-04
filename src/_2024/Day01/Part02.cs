using System.Runtime.ExceptionServices;
using AocLib;

namespace _2024.Day01;

public class Part02 : PuzzleSolver<int>
{
    private const int LINE_COUNT = 1000;
    
    protected override int InternalSolve()
    {
        var lines = input.AsSpan();
        
        var list = new int[LINE_COUNT * 2];
        var scores = new Dictionary<int, int>();
        
        for (int i = 0, j = 0; i < input.Length; i += 14, j++)
        {
            list[j] = lines[i..(i + 5)].AsInt();
            list[LINE_COUNT + j] = lines[(i + 8)..(i + 13)].AsInt();
        }
        
        for (int i = LINE_COUNT; i < LINE_COUNT * 2; i++)
        {
            scores[list[i]] = list[i] + scores.GetValueOrDefault(list[i], 0);
        }

        int count = 0;
        for (int i = 0; i < LINE_COUNT; i++)
        {
            count += scores.GetValueOrDefault(list[i], 0);
        }
        
        return count;
    }
}
