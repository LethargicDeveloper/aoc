using BenchmarkDotNet.Attributes;
using static MoreLinq.Extensions.ForEachExtension;
using System.Text.RegularExpressions;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    static readonly List<Point3D> Rotations = new()
    {
        (0, 0, 0),
        (1, 0, 0),
        (1, 1, 0),
        (1, 0, 1),
        (1, 1, 1),
        (0, 1, 0),
        (0, 1, 1),
        (0, 0, 1)
    };

    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var beacons = input
            .SplitEmptyLines()
            .Select(_ => _.SplitLines())
            .Select(Parse)
            .ToDictionary(k => k.Id, v => v.Beacons);

        var world = new Dictionary<int, HashSet<Point3D>>
        {
            [0] = beacons[0] 
        };

        var queue = new Queue<KeyValuePair<int, HashSet<Point3D>>>();
        beacons.Where(_ => _.Key > 0).ForEach(queue.Enqueue);

        while(queue.TryDequeue(out var sensor))
        {
            if (Overlaps(sensor.Value, world, out var reorientedSensor))
            {
                world[sensor.Key] = reorientedSensor;
                continue;
            }

            queue.Enqueue(sensor);
        }

        return 0;
    }

    static bool Overlaps(HashSet<Point3D> beacons, Dictionary<int, HashSet<Point3D>> world, out HashSet<Point3D> reorientedBeacons)
    {
        foreach (var sensor in world)
        {
            for (int facing = 0; facing < 3; ++facing)
            {
                var facingBeacons = beacons.Select(b =>
                {
                    if (facing == 1)
                        return new(b.Z, b.X, b.Y);

                    if (facing == 2)
                        return new(b.Y, b.Z, b.X);
                        
                    return b;
                }).ToHashSet();

                foreach (var (rotx, roty, rotz) in Rotations)
                {
                    for (int deg = 0; deg <= 3; ++deg)
                    {
                        var rotatedBeacons = facingBeacons.Select(b =>
                        {
                            b = b.RotateX(Point3D.Zero, 90 * deg * rotx);
                            b = b.RotateY(Point3D.Zero, 90 * deg * roty);
                            b = b.RotateZ(Point3D.Zero, 90 * deg * rotz);
                            return b;
                        }).ToHashSet();

                        foreach (var worldBeacon in sensor.Value)
                        {
                            if (TryReorientToPoint(rotatedBeacons, sensor.Value, worldBeacon, out reorientedBeacons))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        reorientedBeacons = new();
        return false;
    }

    static bool TryReorientToPoint(
        HashSet<Point3D> beacons,
        HashSet<Point3D> worldbeacons,
        Point3D point, 
        out HashSet<Point3D> reorientedBeacons)
    {
        foreach (var beacon in beacons)
        {
            var delta = beacon.Delta(point);
            reorientedBeacons = beacons.Select(_ => _ + delta).ToHashSet();
            if (reorientedBeacons.Where(worldbeacons.Contains).Count() >= 12)
            {
                // TODO: not finding matches
                return true;
            }
        }

        reorientedBeacons = new();
        return false;
    }

    static (int Id, HashSet<Point3D> Beacons) Parse(string[] input)
    {
        var scanner = int.Parse(ScannerRegex().Match(input.First()).Value);
        var beacons = input
            .Skip(1)
            .Select(_ => _.Split(",") switch
            {
                var x => new Point3D
                (
                    int.Parse(x[0]),
                    int.Parse(x[1]),
                    int.Parse(x[2])
                )
            }).ToHashSet();
        return (scanner, beacons);
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex ScannerRegex();
}