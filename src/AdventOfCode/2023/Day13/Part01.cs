using AocLib;

namespace AdventOfCode._2023.Day13;

public class Part01 : PuzzleSolver<long>
{
    //   94508
    // < 56708
    //   
    // > 26208
    // > 19408
    //   14508
    public override long Solve()
    {
        var grid = input
            .SplitEmptyLines()
            .Select(_ => _.SplitLines()
                .Select(s => s.Replace("#", "1").Replace(".", "0"))
                .ToArray())
            .ToArray();

        var h = grid
            .Select(s => Convert.ToInt64($"{s}", 2))
            .ToArray();

        var m = grid
            .Select(_ => _.Zip(_).Select(s => Convert.ToInt64($"{s.First}{s.Second}", 2)).ToArray())
            .ToArray();

        var r = grid
            .Select(_ => _.Select(s => s.Reverse().CreateString()))
            .Select(_ => _.Select(s => Convert.ToInt64($"{s}{s}", 2)).ToArray())
            .ToArray();

        long vcount = 0;
        long hcount = 0;
        for (int i = 0; i < m.Length; i++)
        {
            var vval = CheckV(r[i], m[i], grid[i][0].Length);
            if (vval > -1)
            {
                vcount += vval;
            }

            var hval = CheckH(h);
            if (hval > 0)
            {
                hcount += hval;
            }
        }

        return vcount + (100 * hcount);
    }

    int CheckV(long[] r, long[] m, int length)
    {
        return 0;

        //for (int shift = 0; shift < length; shift++)
        //{
        //    bool found = true;
        //    for (int i = 0; i < r.Length; i++)
        //    {
        //        r[i] = r[i] >>> shift;
        //        if ((r[i] & m[i]) != r[i])
        //        {
        //            found = false;
        //            break;
        //        }
        //    }

        //    if (found) return (length + shift) / 2;
        //}

        //return -1;
    }

    int CheckH(long[] m)
    {
        return 0;

        //var i = 0;
        //var back = m.Length - 1;
        ////var found = false;
        //var foundCount = 0;
        
        //for (; i < m.Length; i++)
        //{
        //    if (i == back)
        //    {
        //        return foundCount == 0 ? -1 : foundCount;
        //    }

        //    if (m[i] != m[back])
        //    {
        //        if (foundCount == 0) return -1;
        //        continue;
        //    }

        //    foundCount++;
        //    back--;

        //    if (back <= i) break;
        //}

        //return foundCount == 0 ? -1: i + 1;
    }
}
