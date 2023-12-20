using AocLib;
using MoreLinq;

namespace AdventOfCode._2023.Day13;

public class Part02 : PuzzleSolver<long>
{
    public override long Solve()
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
                if (found && (top < change.y && bottom > change.y)) break;

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

        found = (top < change.y && bottom > change.y);
        if (start == mirror.Length || !found)
        {
            if (!horizontal) return -1;

            var transpose = mirror
                .Transpose()
                .Select(_ => _.CreateString())
                .ToArray();

            return FindMirror(transpose, false, (change.y, change.x));
        }

        return (start + 1) * (horizontal ? 100 : 1);
    }

    void Print(string[] mirror, int pos, bool horizontal)
    {
        if (!horizontal)
            mirror = mirror
                .Transpose()
                .Select(_ => _.CreateString())
                .ToArray();

        for (int y = 0; y < mirror.Length; y++)
        {
            Console.Write($"{y,2} ");
            if (y == pos - 1) Console.Write("V");
            else if (y == pos) Console.Write("^");
            else Console.Write(" ");

            for (int x = 0; x < mirror[0].Length; x++)
            {
                Console.Write(mirror[y][x]);
            }

            if (y == pos - 1) Console.Write("V");
            else if (y == pos) Console.Write("^");
            else Console.Write(" ");

            Console.Write($" {y,2}");
            Console.WriteLine();
        }
    }
}

