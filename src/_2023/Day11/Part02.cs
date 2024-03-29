using AocLib;
using System.Diagnostics.CodeAnalysis;

namespace _2023.Day11;

// 717878258016
public class Part02 : PuzzleSolver<long>
{
    record Pair(Point P1, Point P2);
    class PairComparer : IEqualityComparer<Pair>
    {
        public bool Equals(Pair? x, Pair? y)
            => x?.P1 == y?.P1 && x?.P2 == y?.P2 ||
            x?.P1 == y?.P2 || x?.P2 == y?.P1;

        public int GetHashCode([DisallowNull] Pair obj)
            => HashCode.Combine(obj.P1, obj.P2) & HashCode.Combine(obj.P2, obj.P1);
    }

    protected override long InternalSolve()
    {
        var space = input
            .SplitLines()
            .Select(_ => _.ToList())
            .ToList();

        var galaxies = new List<Point>();
        for (int x = 0; x < space[0].Count; ++x)
            for (int y = 0; y < space.Count; ++y)
                if (space[y][x] == '#')
                    galaxies.Add((x, y));

        var pairs = (
            from p1 in galaxies
            from p2 in galaxies
            where p1 != p2
            select new Pair(p1, p2)
        ).ToHashSet(new PairComparer());

        var (blankX, blankY) = Expansion(space);

        long Distance(Pair pair)
        {
            const long factor = 999999L;

            var (p1, p2) = pair;
            var dist = p1.ManhattanDistance(p2);
            var xs = blankX.Where(x => x >= Math.Min(p1.X, p2.X) && x <= Math.Max(p1.X, p2.X));
            var ys = blankY.Where(y => y >= Math.Min(p1.Y, p2.Y) && y <= Math.Max(p1.Y, p2.Y));
            var xfactor = xs.Count() * factor;
            var yfactor = ys.Count() * factor;

            return dist + xfactor + yfactor;
        }

        return pairs
            .Select(Distance)
            .Sum();
    }

    (HashSet<int>, HashSet<int>) Expansion(List<List<char>> space)
    {
        var blankX = new HashSet<int>();
        var blankY = new HashSet<int>();
        for (int y = 0; y < space.Count; ++y)
            if (!space[y].Contains('#'))
                blankY.Add(y);

        for (int x = 0; x < space[0].Count; ++x)
        {
            bool empty = true;
            for (int y = 0; y < space.Count; ++y)
            {
                if (space[y][x] == '#')
                    empty = false;
            }

            if (empty)
                blankX.Add(x);
        }

        return (blankX, blankY);
    }
}

