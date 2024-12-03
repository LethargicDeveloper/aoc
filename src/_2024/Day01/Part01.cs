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
            list[j] = FastParse(lines[i..(i + 5)]);
            list[LINE_COUNT + j] = FastParse(lines[(i + 8)..(i + 13)]);
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
    
    private static int FastParse(ReadOnlySpan<char> input)
    {
        return (input[0] - '0') * 10000 +
               (input[1] - '0') * 1000 +
               (input[2] - '0') * 100 +
               (input[3] - '0') * 10 +
               (input[4] - '0');
    }
}
