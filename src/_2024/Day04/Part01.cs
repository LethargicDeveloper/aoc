using System.Text;
using AocLib;

namespace _2024.Day04;

public class Part01 : PuzzleSolver<long>
{
    private const string word = "XMAS";
    
    private readonly (int x, int y)[] dirs = [
        (0, -1),  // up
        (0, 1),   // down
        (-1, 0),  // left
        (1, 0),   // right
        (-1, -1), // up-left
        (1, -1),  // up-right
        (-1, 1),  // down-left
        (1, 1),   // down-right
    ];
    
    private string[] lines;
    
    protected override long InternalSolve()
    {
        this.lines = input.SplitLines();

        var count = 0L;
        
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == word[0])
                {
                    count += dirs.Count(dir => CheckDirection(word, x, y, dir.x, dir.y));
                }
            }
        }

        return count;
    }
    
    bool CheckDirection(ReadOnlySpan<char> word, int x, int y, int xDir, int yDir)
    {
        var sb = new StringBuilder();
        
        bool InBounds(int xx, int yy) =>
            xx >= 0 && xx <= lines[0].Length - 1 && yy >= 0 && yy <= lines.Length - 1;
        
        while (sb.Length < word.Length && InBounds(x, y))
        {
            sb.Append(lines[y][x]);
            x += xDir;
            y += yDir;
        }
        
        return word.ToString() == sb.ToString();
    }
}
