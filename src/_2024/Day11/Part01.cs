using System.Numerics;

namespace _2024.Day11;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var stones = input
            .S(' ')
            .Select(long.Parse)
            .ToList();
        
        for (int blink = 0; blink < 25; blink++)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                var stone = stones[i];

                if (stone == 0)
                {
                    stones[i] = 1;
                }
                else if (stone.DigitCount() % 2 == 0)
                {
                    var (stone1, stone2) = Split(stone);

                    stones[i] = stone2;
                    stones.Insert(i++, stone1);
                }
                else
                {
                    stones[i] = stone * 2024;
                }
            }
        }

        return stones.Count;
    }
    
    private static (T part1, T part2) Split<T>(T number)
        where T : INumber<T>
    {
        var mid = number.DigitCount() / 2;
        var divisor = (T)Convert.ChangeType(Math.Pow(10, mid), typeof(T));
        var part1 = number / divisor;
        var part2 = number % divisor;
        return (part1, part2);
    }
}
