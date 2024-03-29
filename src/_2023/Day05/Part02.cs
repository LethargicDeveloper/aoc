using AocLib;

namespace _2023.Day05;

// 34039469
public class Part02 : PuzzleSolver<long>
{
    bool TryGetOverlappingRange(Range<long> r1, Range<long> r2, out Range<long> range)
    {
        var start = Math.Max(r1.Start, r2.Start);
        var end = Math.Min(r1.End, r2.End);

        range = new(start, end);

        return end >= start;
    }

    bool TryGetNonOverlappingRanges(Range<long> r1, Range<long> r2, out List<Range<long>> ranges)
    {
        ranges = [];
        if (r1.Start < r2.Start)
        {
            var under = new Range<long>(r1.Start, Math.Min(r1.End, r2.Start - 1));
            ranges.Add(under);
        }

        if (r1.End > r2.End)
        {
            var over = new Range<long>(Math.Max(r2.End + 1, r1.Start), r1.End);
            ranges.Add(over);
        }

        return ranges.Count > 0;
    }

    protected override long InternalSolve()
    {
        var groups = input.SplitEmptyLines();

        var seeds = groups[0].Split(' ')
            .Skip(1)
            .Select(long.Parse)
            .Chunk(2)
            .Select(_ => new Range<long>(_[0], _[0] + _[1] - 1))
            .ToList();

        var maps = groups
            .Skip(1)
            .Select(Mapping.Create)
            .ToDictionary(k => k.SourceKey, m => m);

        var state = new Dictionary<string, List<Range<long>>>
        {
            { "seed", seeds }
        };

        var key = "seed";
        for (int i = 0; i < maps.Count; ++i)
        {
            var map = maps[key];
            var srcRanges = state[key];
            var destRanges = map.Map;
            state[map.DestKey] = [];

            var newRanges = new List<Range<long>>();
            foreach (var srcRange in srcRanges)
            {
                var nonOverlappingRanges = new List<Range<long>>();
                var checkRanges = new Queue<Range<long>>();
                checkRanges.Enqueue(srcRange);

                foreach (var destRange in destRanges)
                {
                    while (checkRanges.TryDequeue(out var checkRange))
                    {
                        if (TryGetOverlappingRange(checkRange, destRange.Source, out var overlap))
                        {
                            var dxStart = overlap.Start - destRange.Source.Start;
                            var dxEnd = overlap.End - destRange.Source.Start;
                            var r = new Range<long>(
                                destRange.Dest.Start + dxStart,
                                destRange.Dest.Start + dxEnd);

                            newRanges.Add(r);
                        }

                        if (TryGetNonOverlappingRanges(checkRange, destRange.Source, out var nonOverlap))
                        {
                            foreach (var r in nonOverlap)
                                nonOverlappingRanges.Add(r);
                        }
                    }

                    foreach (var range in nonOverlappingRanges)
                        checkRanges.Enqueue(range);
                    nonOverlappingRanges.Clear();
                }

                while (checkRanges.TryDequeue(out var range))
                    newRanges.Add(range);
            }

            state[map.DestKey] = newRanges;
            key = map.DestKey;
        }

        return state["location"].Min(_ => _.Start);
    }

    record Map(Range<long> Dest, Range<long> Source);
    class Mapping
    {
        public string SourceKey { get; set; } = string.Empty;
        public string DestKey { get; set; } = string.Empty;
        public List<Map> Map { get; set; } = [];

        public static Mapping Create(string mapping)
        {
            var lines = mapping.SplitLines();

            var keyLine = lines[0].Split(' ')[0].Split('-');
            var sourceKey = keyLine[0];
            var destKey = keyLine[2];

            var maps = lines[1..]
                .Select(_ => _.Split(' ').Select(long.Parse).ToArray())
                .Select(_ => new Map(new(_[0], _[0] + _[2] - 1), new(_[1], _[1] + _[2] - 1)))
                .ToList();

            return new()
            {
                SourceKey = sourceKey,
                DestKey = destKey,
                Map = maps
            };
        }
    }
}
