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

        return mirrors.Sum(_ => FindMirror(_));
    }

    int FindMirror(string[] mirror, bool horizontal = true)
    {
        int start = 0;
        int top = 0;
        int bottom = 1;
        bool found = true;

        while (true)
        {
            if (top < 0 || bottom > mirror.Length - 1)
            {
                if (found) break;

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

        if (start == mirror.Length)
        {
            if (!horizontal) return -1;

            var transpose = mirror
                .Transpose()
                .Select(_ => _.CreateString())
                .ToArray();

            return FindMirror(transpose, false);
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

        Console.Clear();

        if (horizontal)
        {
            for (int y = 0; y < mirror.Length; y++)
            {
                Console.Write($"{y,2} ");
                if (y == pos) Console.Write("V");
                else if (y == pos + 1) Console.Write("^");
                else Console.Write(" ");

                for (int x = 0; x < mirror[0].Length; x++)
                {
                    Console.Write(mirror[y][x]);
                }

                if (y == pos) Console.Write("V");
                else if (y == pos + 1) Console.Write("^");
                else Console.Write(" ");

                Console.Write($"{y,2}");
                Console.WriteLine();
            }
        }
        else
        {
            for (int x = 0; x < mirror[0].Length; x++)
                Console.Write($"{x % 10}");

            Console.WriteLine();
            for (int x = 0; x < mirror[0].Length; x++)
            {
                if (x == pos) Console.Write(">");
                else if (x == pos + 1) Console.Write("<");
                else Console.Write(" ");
            }
            Console.WriteLine();

            for (int y = 0; y < mirror.Length; y++)
            {
                for (int x = 0; x < mirror[0].Length; x++)
                {
                    Console.Write(mirror[y][x]);
                }

                Console.WriteLine();
            }

            for (int x = 0; x < mirror[0].Length; x++)
            {
                if (x == pos) Console.Write(">");
                else if (x == pos + 1) Console.Write("<");
                else Console.Write(" ");
            }
            
            Console.WriteLine();
            for (int x = 0; x < mirror[0].Length; x++)
                Console.Write($"{x % 10}");
        }

        Console.ReadKey(false);
    }
}

