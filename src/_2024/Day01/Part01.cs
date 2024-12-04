using AocLib;

using System.Globalization;



namespace _2024.Day01;

public class Part01 : PuzzleSolver<int>
{
    private const int LINE_COUNT = 1000;

    protected override int InternalSolve()
    {
        var lines = input.AsSpan();
        
        var list = new int[LINE_COUNT * 2];
        
        for (int i = 0, j = 0; i < input.Length; i += 14, j++)
        {
            list[j] = lines[i..(i + 5)].AsInt();
            list[LINE_COUNT + j] = lines[(i + 8)..(i + 13)].AsInt();
        }
        
        Array.Sort(list, 0, LINE_COUNT);
        Array.Sort(list, LINE_COUNT, LINE_COUNT);
        
        var total = 0;
        
        for (int i = 0; i < LINE_COUNT; i++)
        {
            total += Math.Abs(list[i] - list[LINE_COUNT + i]);
        }

        return total;
    }
}
