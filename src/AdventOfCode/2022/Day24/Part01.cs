using AdventOfCode.Abstractions;
using AocLib;

namespace AdventOfCode._2022.Day24;

public partial class Part01 : PuzzleSolver<long>
{
    public override long Solve() =>
        new BlizzardBasin(this.input).ShortestPath();

    class BlizzardBasin
    {
        class Map : List<List<char>>
        {
            public Map(List<List<char>> map)
            {
                this.AddRange(map);
            }

            public Point StartPos { get; init; }
            public Point EndPos { get; init; }
            public int Width => this[0].Count - 1;
            public int Height => this.Count - 1;
        }

        record Blizzard(Point Pos, char Dir);
        static readonly Dictionary<char, Point> Directions = new()
        {
            { '^', (0, -1) },
            { 'v', (0, 1) },
            { '<', (-1, 0) },
            { '>', (1, 0) }
        };

        readonly Map map;
        List<Blizzard> blizzardSpawnPos;

        public BlizzardBasin(string input)
        {
            (map, blizzardSpawnPos) = Initialize(input);
        }

        static (Map, List<Blizzard>) Initialize(string input)
        {
            var b = new List<Blizzard>();
            var m = input
                .SplitLines()
                .Select((line, y) => line.Select((c, x) =>
                {
                    if ("^v<>".Contains(c))
                    {
                        b.Add(new Blizzard((x, y), c));
                        return '.';
                    }

                    return c;
                }).ToList())
                .ToList();

            var startPos = (m[0].IndexOf('.'), 0);
            var endPos = (m[^1].IndexOf('.'), m.Count - 1);

            return (new Map(m)
            {
                StartPos = startPos,
                EndPos = endPos,
            }, b);
        }

        public int ShortestPath()
        {
            var queue = new Queue<State>();
            var hash = new HashSet<State>();
            queue.Enqueue(new State
            {
                Minute = 0,
                Pos = map.StartPos,
                Blizzards = blizzardSpawnPos
            });

            int shortestPath = 0;
            while (queue.TryDequeue(out var state))
            {
                if (state.Pos == map.EndPos)
                {
                    shortestPath = state.Minute;
                    break;
                }

                var newBlizzardPos = new List<Blizzard>();
                foreach (var blizzard in state.Blizzards)
                {
                    var pos = blizzard.Pos + Directions[blizzard.Dir];
                    if (pos.X < 1) pos = (map.Width - 1, pos.Y);
                    if (pos.X > map.Width - 1) pos = (1, pos.Y);
                    if (pos.Y < 1) pos = (pos.X, map.Height - 1);
                    if (pos.Y > map.Height - 1) pos = (pos.X, 1);

                    newBlizzardPos.Add(new Blizzard(pos, blizzard.Dir));
                }

                var neighbors = Directions.Values
                    .Select(pos => pos + state.Pos)
                    .Where(pos => !newBlizzardPos.Select(b => b.Pos).Contains(pos))
                    .Where(pos => pos == map.EndPos || (pos.X >= 1 && pos.X <= map.Width - 1 && pos.Y >= 1 && pos.Y <= map.Height - 1))
                    .Where(pos => map[pos.Y][pos.X] != '#')
                    .ToList();

                if (!newBlizzardPos.Any(_ => _.Pos == state.Pos))
                {
                    var newState = new State
                    {
                        Minute = state.Minute + 1,
                        Pos = state.Pos,
                        Blizzards = newBlizzardPos
                    };

                    if (hash.Add(newState))
                        queue.Enqueue(newState);
                }

                foreach (var neighbor in neighbors)
                {
                    var newState = new State
                    {
                        Minute = state.Minute + 1,
                        Pos = neighbor,
                        Blizzards = newBlizzardPos
                    };

                    if (hash.Add(newState))
                        queue.Enqueue(newState);
                }
            }

            return shortestPath;
        }

        class State : IEquatable<State?>
        {
            public int Minute { get; set; } = 0;
            public Point Pos { get; init; }
            public List<Blizzard> Blizzards { get; init; } = new();

            public override bool Equals(object? obj)
            {
                return Equals(obj as State);
            }

            public bool Equals(State? other)
            {
                return other is not null &&
                       Pos == other.Pos &&
                       Enumerable.SequenceEqual(Blizzards, other.Blizzards);
            }

            public override int GetHashCode()
            {
                var hash = new HashCode();
                hash.Add(Pos);
                Blizzards.ForEach(hash.Add);

                return hash.ToHashCode();
            }

            public static bool operator ==(State? left, State? right)
            {
                return EqualityComparer<State>.Default.Equals(left, right);
            }

            public static bool operator !=(State? left, State? right)
            {
                return !(left == right);

            }
        }
    }
}
