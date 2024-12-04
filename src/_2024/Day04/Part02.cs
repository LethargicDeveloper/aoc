using System.Text;
using AocLib;

namespace _2024.Day04;

public class Part02 : PuzzleSolver<long>
{
    private const string word = "MAS";
    
    private string[] lines;
    
    protected override long InternalSolve()
    {
        this.lines = input.SplitLines();

        var count = 0L;
        
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
      
                if (CheckDirection(word, x, y, (1, 1)) &&
                    CheckDirection(word, x + 2, y, (-1, 1)))
                    count++;
            }
        }

        return count;
    }
    
    bool CheckDirection(ReadOnlySpan<char> word, int x, int y, (int xDir, int yDir) dir)
    {
        var sb = new StringBuilder();
        
        bool InBounds(int xx, int yy) =>
            xx >= 0 && xx <= lines[0].Length - 1 && yy >= 0 && yy <= lines.Length - 1;
        
        while (sb.Length < word.Length && InBounds(x, y))
        {
            sb.Append(lines[y][x]);
            x += dir.xDir;
            y += dir.yDir;
        }
        
        return (word.ToString() == sb.ToString() || word.ToString() == sb.ToString().Reverse().AsString());
    }
}
