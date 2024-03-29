using AocLib;
using MoreLinq;

namespace _2023.Day13;

// 28210
public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var mirrors = input
            .SplitEmptyLines()
            .Select(_ => _.SplitLines())
            .ToList();

        long total = 0;
        foreach (var mirror in mirrors)
        {
            for (int y = 0; y < mirror.Length; y++)
            {
                for (int x = 0; x < mirror[0].Length; x++)
                {
                    mirror[y] = mirror[y].ReplaceCharAt(x, mirror[y][x] == '.' ? '#' : '.');
                    var value = FindMirror(mirror, true, (x, y));
                    mirror[y] = mirror[y].ReplaceCharAt(x, mirror[y][x] == '.' ? '#' : '.');

                    if (value > -1)
                    {
                        total += value;
                        goto NEXT_MIRROR;
                    }
                }
            }

        NEXT_MIRROR:
            continue;
        }

        return total;
    }

    int FindMirror(string[] mirror, bool horizontal, (int x, int y) change)
    {
        int start = 0;
        int top = 0;
        int bottom = 1;
        bool found = true;

        while (true)
        {
            if (top < 0 || bottom > mirror.Length - 1)
            {
                if (found && top < change.y && bottom > change.y) break;

                top = ++start;
                bottom = top + 1;
            }

            found = true;

            if (top >= mirror.Length - 1)
                break;

            if (mirror[top] == mirror[bottom])
            {
                top--;
                bottom++;

                continue;
            }

            found = false;
            top = ++start;
            bottom = top + 1;
        }

        found = top < change.y && bottom > change.y;
        if (start == mirror.Length || !found)
        {
            if (!horizontal) return -1;

            var transpose = mirror
                .Transpose()
                .Select(_ => _.AsString())
                .ToArray();

            return FindMirror(transpose, false, (change.y, change.x));
        }

        return (start + 1) * (horizontal ? 100 : 1);
    }
}

