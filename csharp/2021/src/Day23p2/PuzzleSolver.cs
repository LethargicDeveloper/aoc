﻿using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        return new ElfMover(input).FirstRoundNoElvesMoved();
    }
}

class ElfMover
{
    enum Direction { N, NE, E, SE, S, SW, W, NW };

    static readonly Dictionary<Direction, Point> Directions = new()
    {
        { Direction.N, (0, -1) },
        { Direction.NE, (1, -1) },
        { Direction.E, (1, 0) },
        { Direction.SE, (1, 1) },
        { Direction.S, (0, 1) },
        { Direction.SW, (-1, 1) },
        { Direction.W, (-1, 0) },
        { Direction.NW, (-1, -1) },
    };

    static readonly List<List<Point>> CheckDirections = new()
    {
        new() { (0, -1), (1, -1), (-1, -1) },
        new() { (0, 1), (1, 1), (-1, 1) },
        new() { (-1, 0), (-1, -1), (-1, 1)},
        new() { (1, 0), (1, -1), (1, 1) },
    };

    readonly HashSet<Point> elves = new();
    readonly int rounds;

    public ElfMover(string input, int rounds)
    {
        this.elves = Parse(input);
        this.rounds = rounds;
    }

    public ElfMover(string input)
        : this(input, 0) { }

    public int FirstRoundNoElvesMoved()
    {
        return MoveElves();
    }

    public int MoveElves()
    {
        var checkDirection = 0;

        int round = 0;
        while (rounds == 0 || round <= rounds)
        {
            var startElves = this.elves.ToHashSet();
            var movableElves = this.elves.Where(HasAdjacentElf);

            var proposedMoves = new Dictionary<Point, List<Point>>();
            foreach (var moveableElf in movableElves)
            {
                if (TryGetEmptyDirection(moveableElf, checkDirection, out var emptyDir))
                {
                    if (!proposedMoves.ContainsKey(emptyDir))
                    {
                        proposedMoves[emptyDir] = new();
                    }

                    proposedMoves[emptyDir].Add(moveableElf);
                }
            }

            var elvesToMove = proposedMoves
                .Where(_ => _.Value.Count == 1)
                .Select(_ => (elf: _.Value.First(), pos: _.Key));

            foreach (var elfToMove in elvesToMove)
            {
                elves.Remove(elfToMove.elf);
                elves.Add(elfToMove.pos);
            }


            checkDirection = (checkDirection + 1) % CheckDirections.Count;
            round++;

            if (this.elves.SetEquals(startElves))
                break;
        }

        return round;
    }

    bool TryGetEmptyDirection(Point elf, int checkDirection, out Point emptyDir)
    {
        for (int i = 0; i < CheckDirections.Count; ++i)
        {
            var index = (i + checkDirection) % CheckDirections.Count;
            var directions = CheckDirections[index];

            if (directions.All(dir => !elves.Contains(elf + dir)))
            {
                emptyDir = elf + directions[0];
                return true;
            }
        }

        emptyDir = (0, 0);
        return false;
    }

    bool HasAdjacentElf(Point elf)
        => Directions.Values.Any(dir => elves.Contains(elf + dir));

    static HashSet<Point> Parse(string input)
    {
        var hash = new HashSet<Point>();

        var lines = input.Split("\r\n");
        for (int y = 0; y < lines.Length; ++y)
            for (int x = 0; x < lines[0].Length; ++x)
            {
                if (lines[y][x] == '#')
                    hash.Add((x, y));
            }

        return hash;
    }
}