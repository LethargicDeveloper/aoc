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

        mirrors =
        [
            """
            .#.##.#.##..###
            ...##...#######
            #.####.#.#.###.
            #..##..##..#...
            ###..###....###
            .##..##..#.#...
            .#....#..######
            #..##..########
            ########.#..#..

            ###..##.#.##.#. 
            #######...##... 
            .###.#.#.####.# 
            ...#..##..##..# 
            ###....###..### 
            ...#.#..##..##. 
            ######..#....#. 
            ########..##..# 
            ..#..#.######## 
            """.SplitLines()
        ];

        long total = 0;
        foreach (var mirror in mirrors)
        {
            var m = mirror
                .Select(_ => _.Select(c => c == '#' ? '1' : '0').CreateString())
                .Select(_ => Convert.ToInt64(_, 2))
                .ToList();

            var r = mirror
                .Select(_ => _.Select(c => c == '#' ? '1' : '0').Reverse().CreateString())
                .Select(_ => Convert.ToInt64(_, 2))
                .ToList();

            var value = FindMirror(m, r, mirror[0].Length);
            if (value == -1)
            {
                var tm = mirror
                    .Transpose()
                    .Select(_ => _.Select(c => c == '#' ? '1' : '0').CreateString())
                    .Select(_ => Convert.ToInt64(_, 2))
                    .ToList();

                var tr = mirror
                    .Transpose()
                    .Select(_ => _.Select(c => c == '#' ? '1' : '0').Reverse().CreateString())
                    .Select(_ => Convert.ToInt64(_, 2))
                    .ToList();

                value = FindMirror(tm, tr, mirror.Length) * 100;

                Print(mirror);
            }

            total += value;
        }

        return total;
    }

    void Print(string[] mirror)
    {
        for (int y = 0; y < mirror.Length; y++)
        {
            for (int x  = 0; x < mirror[0].Length; x++)
            {
                Console.Write(mirror[y][x]);
            }

            Console.WriteLine();
        }
    }

    long FindMirror(List<long> mirror, List<long> reflection, int length, bool back = false)
    {
        var r1 = reflection.ToList();

        int count = 0;
        while (count < length - 1)
        {
            bool ismirror = true;
            for (int i = 0; i < r1.Count; ++i)
                if ((r1[i] & mirror[i]) != r1[i])
                {
                    ismirror = false;
                    break;
                }

            if (ismirror)
                return ((length - count) / 2) + 1;

            count++;
            for (int i = 0; i < r1.Count; i++)
            {
                if (back)
                    r1[i] <<= 1;
                else
                    r1[i] >>= 1;
            }

            foreach (var m in mirror)
                Console.WriteLine(Convert.ToString(m, 2).PadLeft(20));
            Console.WriteLine();
            foreach (var r in r1)
                Console.WriteLine(Convert.ToString(r, 2).PadLeft(20));
            Console.ReadKey(false);
            Console.Clear();
        }

        return back ? -1 : FindMirror(mirror, reflection, length, back: true);
    }
}






