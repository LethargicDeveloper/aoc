using AocLib;

namespace _2023.Day11;

// 9693756
public class Part01 : PuzzleSolver<long>
{
    record Pair(Point P1, Point P2);
    class PairComparer : IEqualityComparer<Pair>
    {
        public bool Equals(Pair? x, Pair? y)
            => x?.P1 == y?.P1 && x?.P2 == y?.P2 ||
            x?.P1 == y?.P2 || x?.P2 == y?.P1;

        public int GetHashCode(Pair obj)
            => HashCode.Combine(obj.P1, obj.P2) & HashCode.Combine(obj.P2, obj.P1);
    }

    protected override long InternalSolve()
    {
        var space = input
            .SplitLines()
            .Select(_ => _.ToList())
            .ToList();

        Expand(space);

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

        return pairs
            .Select(_ => _.P1.ManhattanDistance(_.P2))
            .Sum();
    }

    void Expand(List<List<char>> space)
    {
        for (int y = 0; y < space.Count; ++y)
        {
            if (!space[y].Contains('#'))
            {
                space.Insert(y, [.. '.'.Repeat(space[0].Count)]);
                y++;
            }
        }

        for (int x = 0; x < space[0].Count; ++x)
        {
            bool empty = true;
            for (int y = 0; y < space.Count; ++y)
            {
                if (space[y][x] == '#')
                    empty = false;
            }

            if (empty)
            {
                for (int y = 0; y < space.Count; ++y)
                    space[y].Insert(x, '.');
                x++;
            }
        }
    }
}

