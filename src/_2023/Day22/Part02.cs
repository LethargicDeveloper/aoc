using AocLib;
using QuikGraph;

namespace _2023.Day22;

// 75784
public class Part02 : PuzzleSolver<long>
{
    record Brick(Point3D P1, Point3D P2)
    {
        public int MinZ = Math.Min(P1.Z, P2.Z);
        public int MaxZ = Math.Max(P1.Z, P2.Z);
        public int MinX = Math.Min(P1.X, P2.X);
        public int MaxX = Math.Max(P1.X, P2.X);
        public int MinY = Math.Min(P1.Y, P2.Y);
        public int MaxY = Math.Max(P1.Y, P2.Y);
        public int DiffZ => MaxZ - MinZ;

        public IEnumerable<Point3D> GetPoints()
        {
            if (P1.Z != P2.Z)
                for (int z = MinZ; z <= MaxZ; z++)
                    yield return new Point3D(P1.X, P1.Y, z);

            else if (P1.X != P2.X)
                for (int x = MinX; x <= MaxX; x++)
                    yield return new Point3D(x, P1.Y, P1.Z);

            else if (P1.Y != P2.Y)
                for (int y = MinY; y <= MaxY; y++)
                    yield return new Point3D(P1.X, y, P1.Z);

            else yield return P1;
        }

        public Brick MoveDown()
        {
            return new Brick(
                new Point3D(P1.X, P1.Y, P1.Z - 1),
                new Point3D(P2.X, P2.Y, P2.Z - 1));
        }

        public bool CollidesWith(Brick brick)
        {
            var p1s = GetPoints().Select(_ => new Point(_.X, _.Y));
            var p2s = brick.GetPoints().Select(_ => new Point(_.X, _.Y));
            return MinZ == brick.MaxZ + 1 && p1s.Any(p => p2s.Contains(p));
        }
    }

    protected override long InternalSolve()
    {
        var bricks = input
            .SplitLines()
            .Select((_, i) => _.Split('~') switch
            {
                var x => new Brick(Point3D.Parse(x[0]), Point3D.Parse(x[1]))
            })
            .OrderBy(b => b.MinZ)
            .ToArray();

        var tower = Simulate(bricks).ToUndirectedGraph<Brick, SEdge<Brick>>();

        var count = 0;

        foreach (var brick in bricks)
        {
            var queue = new Queue<Brick>();
            queue.Enqueue(brick);

            var state = new HashSet<Brick> { brick };

            while (queue.TryDequeue(out var testBrick))
            {
                var onTop = tower
                    .AdjacentEdges(testBrick)
                    .Where(b => b.Source != b.Target)
                    .Where(b => b.Source == testBrick)
                    .Select(b => b.Target)
                    .ToList();

                var bricksToCheck = onTop
                    .Select(top => (brick: top, supports: tower
                        .AdjacentEdges(top)
                        .Where(b => b.Source != b.Target)
                        .Where(b => b.Target == top && b.Source != brick)
                        .Select(b => b.Source)));

                foreach (var brickToCheck in bricksToCheck)
                {
                    if (!brickToCheck.supports.Any() || brickToCheck.supports.All(state.Contains))
                    {
                        if (state.Add(brickToCheck.brick))
                        {
                            count++;
                            queue.Enqueue(brickToCheck.brick);
                        }
                    }
                }
            }
        }

        return count;
    }

    List<SEdge<Brick>> Simulate(Brick[] bricks)
    {
        var edges = new List<SEdge<Brick>>();

        for (int i = 0; i < bricks.Length; i++)
        {
            var brick1 = bricks[i];
            var bricksToCheck = bricks[..i];
            bool collided = false;

            var minZ = brick1.MinZ;
            for (int j = minZ; j > 1; j--)
            {
                var collisions = bricksToCheck
                    .Where(brick1.CollidesWith)
                    .ToList();

                if (collisions.Count == 0)
                {
                    brick1 = brick1.MoveDown();
                    bricks[i] = brick1;
                }
                else
                {
                    foreach (var collision in collisions)
                        edges.Add(new SEdge<Brick>(collision, brick1));

                    collided = true;

                    break;
                }
            }

            if (!collided)
            {
                edges.Add(new SEdge<Brick>(brick1, brick1));
            }
        }

        return edges;
    }
}
