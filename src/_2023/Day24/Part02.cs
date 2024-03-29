using AocLib;

using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<double>;

namespace _2023.Day24;

// 677656046662770
// https://numerics.mathdotnet.com/LinearEquations
// https://www.reddit.com/r/adventofcode/comments/18q40he/2023_day_24_part_2_a_straightforward_nonsolver/

public class Part02 : PuzzleSolver<double>
{
    record Hail(Vector Position, Vector Velocity);

    protected override double InternalSolve()
    {
        var hailstones = input
            .SplitLines()
            .Select(_ => _.Split('@', ',') switch
            {
                var x => x.Select(double.Parse).ToArray()
            })
            .Select(hail =>
            {
                var position = Vector.Build.Dense(hail[0..3]);
                var velocity = Vector.Build.Dense(hail[3..6]);
                return new Hail(position, velocity);
            })
            .ToArray();

        var data = hailstones
            .Skip(1)
            .Take(4)
            .Select((h2, i) =>
            {
                var h1 = hailstones[i];

                var r1 = new double[6]; // x, y
                r1[0] = h1.Velocity[1] - h2.Velocity[1];
                r1[1] = h2.Velocity[0] - h1.Velocity[0];
                r1[3] = h2.Position[1] - h1.Position[1];
                r1[4] = h1.Position[0] - h2.Position[0];

                var r2 = new double[6]; // x, z
                r2[0] = h1.Velocity[2] - h2.Velocity[2];
                r2[2] = h2.Velocity[0] - h1.Velocity[0];
                r2[3] = h2.Position[2] - h1.Position[2];
                r2[5] = h1.Position[0] - h2.Position[0];

                var r3 = new double[6]; // y, z
                r3[1] = h1.Velocity[2] - h2.Velocity[2];
                r3[2] = h2.Velocity[1] - h1.Velocity[1];
                r3[4] = h2.Position[2] - h1.Position[2];
                r3[5] = h1.Position[1] - h2.Position[1];

                var c1 = h2.Position[1] * h2.Velocity[0] - h2.Position[0] * h2.Velocity[1] - h1.Position[1] * h1.Velocity[0] + h1.Position[0] * h1.Velocity[1];
                var c2 = h2.Position[2] * h2.Velocity[0] - h2.Position[0] * h2.Velocity[2] - h1.Position[2] * h1.Velocity[0] + h1.Position[0] * h1.Velocity[2];
                var c3 = h2.Position[2] * h2.Velocity[1] - h2.Position[1] * h2.Velocity[2] - h1.Position[2] * h1.Velocity[1] + h1.Position[1] * h1.Velocity[2];

                return (r: new[] { r1, r2, r3 }, c: new[] { c1, c2, c3 });
            });

        var coefficients = Matrix.Build.DenseOfRowArrays(data.SelectMany(_ => _.r));
        var constants = Vector.Build.DenseOfEnumerable(data.SelectMany(_ => _.c));
        var solution = coefficients.Solve(constants);
        var coords = solution.Take(3).Select(_ => (long)Math.Round(_));

        return coords.Sum();
    }
}
