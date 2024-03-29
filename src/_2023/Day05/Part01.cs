using AocLib;

namespace _2023.Day05;

// 26273516
public class Part01 : PuzzleSolver<long>
{
    record Map(long Dest, long Source, long Range);
    class Seed : Dictionary<string, long> { }
    record Seeds(List<Seed> Values);

    protected override long InternalSolve()
    {
        var groups = input.SplitEmptyLines();
        var seeds = groups[0].Split(' ')
            .Skip(1)
            .Select(long.Parse)
            .Select(_ => new Map(_, _, 0))
            .ToArray();

        var maps = groups
            .Skip(1)
            .Select(Mapping.Create)
            .ToDictionary(k => k.Parent, m => m);

        var state = new Seeds(seeds
            .Select(_ => new Seed() { { "seed", _.Source } })
            .ToList());

        var key = "seed";
        for (int i = 0; i < maps.Count; ++i)
        {
            var map = maps[key];

            foreach (var seed in state.Values)
            {
                var seedValue = seed[key];
                foreach (var m in map.Map)
                {
                    if (seedValue >= m.Source && seedValue <= m.Source + m.Range)
                    {
                        seed[map.Key] = m.Dest + (seedValue - m.Source);
                        break;
                    }
                }

                if (!seed.ContainsKey(map.Key))
                    seed[map.Key] = seedValue;
            }

            key = map.Key;
        }

        return state.Values.Select(_ => _["location"]).Min();
    }

    class Mapping
    {
        public string Key { get; set; } = string.Empty;
        public string Parent { get; set; } = string.Empty;
        public Map[] Map { get; set; } = null!;

        public static Mapping Create(string mapping)
        {
            var lines = mapping.SplitLines();

            var keyLine = lines[0].Split(' ')[0].Split('-');
            var sourceKey = keyLine[0];
            var destKey = keyLine[2];

            var maps = lines[1..]
                .Select(_ => _.Split(' ').Select(long.Parse).ToArray())
                .Select(_ => new Map(_[0], _[1], _[2]))
                .ToArray();

            return new()
            {
                Parent = sourceKey,
                Key = destKey,
                Map = maps
            };
        }
    }
}
