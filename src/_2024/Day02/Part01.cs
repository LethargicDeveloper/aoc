namespace _2024.Day02;

[Answer(549)]
public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var reports = input.ParseNumbers<long>();

        var answer = reports
            .Count(record => Rule1(record) && Rule2(record));
        
        return answer;
    }
    
    private static bool Rule2(List<long> record) => record
        .WithIndex()
        .Skip(1)
        .All(level => Math.Abs(level.Value - record[level.Index - 1]) <= 3);

    private static bool Rule1(List<long> record)
    {
        var distinct = record.ToHashSet();
        return distinct.SequenceEqual(record.OrderBy()) ||
               distinct.SequenceEqual(record.OrderByDescending());   
    }
}