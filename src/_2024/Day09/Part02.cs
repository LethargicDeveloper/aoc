using System.Data.Common;
using System.Text;
using AocLib;
using MoreLinq;

namespace _2024.Day09;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var diskMap = input
            .Select(char.GetNumericValue)
            .Select(Convert.ToInt32)
            .Select((val, i) => string.Join("", (i % 2 == 0 ? $"{(char)((i / 2) + 100)}" : ".").Repeat(val)))
            .SelectMany()
            .ToList();

        var moved = new HashSet<char>();
        
        for (int i = diskMap.Count - 1; i > 0; i--)
        {
            var id = diskMap[i];
            if (id == '.' || moved.Contains(id)) continue;
            
            var ix2 = i;
            var ix1 = i;
            while (ix1 > 0 && diskMap[ix2] == diskMap[ix1 - 1])
                ix1--;
            
            var file = diskMap[ix1..(ix2 + 1)];

            var freeix = 0;
            for (int f1 = 0; f1 < ix1; f1++)
            {
                if (diskMap[f1] != '.') continue;

                var f2 = f1 + 1;
                while (f2 < diskMap.Count && (f2 - f1) < file.Count && diskMap[f2] == '.') f2++;
                f2--;

                if (f2 - f1 >= file.Count - 1)
                {
                    freeix = f1;
                    break;
                }
            }

            if (freeix > 0)
            {
                for (int j = freeix; (j - freeix) < file.Count; j++)
                    diskMap[j] = id;

                for (int j = ix1; j <= ix2; j++)
                    diskMap[j] = '.';
            }

            moved.Add(id);
            i -= file.Count - 1;
        }

        var checksum = diskMap
            .WithIndex()
            .Select(x => x.Value == '.' ? 0 : (long)x.Index * (x.Value - 100))
            .Sum();
        
        return checksum;
    }
}