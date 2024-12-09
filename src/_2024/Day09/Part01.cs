using System.Text;
using AocLib;

namespace _2024.Day09;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var partitionSizes = input
            .Select(char.GetNumericValue)
            .Select(Convert.ToInt32)
            .ToList();

        var sb = new StringBuilder();
        for (int i = 0; i < partitionSizes.Count; i++)
        {
            var id = (i / 2) + 100;
            
            sb.Append(string.Join(
                string.Empty, 
                (i % 2 == 0 ? $"{(char)id}" : ".").Repeat(partitionSizes[i])));
        }
        
        var diskMap = sb.ToString().ToCharArray();
        
        int j = 0;
        for (int i = diskMap.Length - 1; i > j; i--)
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
