using AocLib;

namespace _2024.Day11;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var stones = input
            .SplitR(' ')
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
                else if (stone.NumberOfDigits() % 2 == 0)
                {
                    var (stone1, stone2) = stone.Split();

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
}
