using AocLib;

using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<double>;
using MathNet.Numerics;

namespace _2023.Day24;

// 20336
public class Part01 : PuzzleSolver<double>
{
    const float Min = 200000000000000;
    const float Max = 400000000000000;

    record Hail(Vector Position, Vector Velocity);

    protected override double InternalSolve()
    {
        var data = input
            .SplitLines()
            .Select(_ => _.Split('@', ',') switch
            {
                var x => x.Select(double.Parse).ToArray()
            })
            .Select(hail =>
            {
                var position = Vector.Build.Dense(hail[0..2]);
                var velocity = Vector.Build.Dense(hail[3..5]);
                return new Hail(position, velocity);
            })
            .ToArray();

        var allHailstones = 
            from h1 in data
            from h2 in data
            where h1 != h2
            select (h1, h2)
        ;

        var hailstones = new HashSet<(Hail, Hail)>();
        foreach (var combo in allHailstones)
            if (!hailstones.Contains((combo.h2, combo.h1)))
                hailstones.Add(combo);

        return hailstones
            .Sum(combo =>
            {
                var h1 = combo.Item1;
                var h2 = combo.Item2;

                var coefficients = Matrix.Build.DenseOfColumnVectors(h1.Velocity, -h2.Velocity);
                var constants = h2.Position - h1.Position;
                var solvedTimes = coefficients.Solve(constants);

                if (solvedTimes.All(t => t.IsFinite() && t >= 0))
                {
                    var intersection = h1.Position + h1.Velocity * solvedTimes[0];
                    return intersection.All(x => x >= Min && x <= Max) ? 1 : 0;
                }

                return 0;
            });
    }
}
