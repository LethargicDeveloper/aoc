using AocLib;
using Microsoft.CodeAnalysis;

namespace _2024.Day02;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var reports = input
            .SplitLines()
            .Select(x => x.Split(" ").Select(long.Parse).ToArray())
            .ToArray();

        static bool Rule1(long[] record)
        {
            var distinct = record.Distinct().ToList();
            return distinct.SequenceEqual(record.OrderBy()) ||
                   distinct.SequenceEqual(record.OrderByDescending());   
        }
        
        static bool Rule2(long[] record) => record
            .WithIndex()
            .Skip(1)
            .All(level => Math.Abs(level.Value - record[level.Index - 1]) <= 3);

        var answer = reports
            .Count(record => Rule1(record) && Rule2(record));
        
        return answer;
    }
}
