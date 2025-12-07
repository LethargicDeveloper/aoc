using AocLib;

namespace _2015.Day02;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(x => x.Split('x').Select(long.Parse).ToArray())
            .Select(x => new Rectangle(x))
            .Sum(x => x.SurfaceArea + MathEx.Min(x.LengthArea, x.WidthArea, x.HeightArea));
    }

    class Rectangle(long[] dimensions)
    {
        public long LengthArea => dimensions[0] * dimensions[1];
        public long WidthArea => dimensions[1] * dimensions[2];
        public long HeightArea => dimensions[2] * dimensions[0];
        
        public long SurfaceArea => (2 * LengthArea) + (2 * WidthArea) + (2 * HeightArea);
    }
}
