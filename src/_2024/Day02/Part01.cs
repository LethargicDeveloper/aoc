using System.Text.RegularExpressions;
using AocLib;
using Microsoft.CodeAnalysis;

namespace _2024.Day02;

public class Part01 : PuzzleSolver<int>
{
    protected override int InternalSolve()
    {
        var reports = input
            .SplitLines()
            .Select(x => x.Split(" ").Select(x => x.AsInt()).ToArray())
            .ToArray();

        static bool IsValid(int[] record)
        {
            int prev = 0;

            for (int i = 1; i < record.Length; i++)
            {
                var val = record[i] - record[i - 1];

                if (val == 0 || Math.Abs(val) > 3 || (i > 1 && Math.Sign(val) != Math.Sign(prev)))
                    return false;

                prev = val;
            }

            return true;
        }

        int safe = 0;
        for (int i = 0; i < reports.Length; i++)
        {
            if (IsValid(reports[i])) safe++;
        }
        
        return safe;
    }
}
