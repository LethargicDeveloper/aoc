using System.Text;
using AocLib;

namespace _2024.Day09;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var diskMap = input
            .Select(char.GetNumericValue)
            .Select(Convert.ToInt32)
            .Select((val, i) => string.Join("", (i % 2 == 0 ? $"{(char)((i / 2) + 100)}" : ".").Repeat(val)))
            .SelectMany()
            .ToList();
        
        int j = 0;
        for (int i = diskMap.Count - 1; i > j; i--)
        {
            var id = diskMap[i];
            if (id == '.') continue;

            while (j < i && diskMap[j] != '.') j++;
            if (j == i) break;
            
            diskMap[j] = diskMap[i];
            diskMap[i] = '.';
        }

        var checksum = diskMap
            .TakeWhile(x => x != '.')
            .WithIndex()
            .Select(x => (long)x.Index * (x.Value - 100))
            .Sum();
        
        return checksum;
    }
}
