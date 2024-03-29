using AocLib;
using System.Text.RegularExpressions;

namespace _2023.Day02;

// 66909
public partial class Part02 : PuzzleSolver<long>
{
    record Round(int R, int G, int B);
    record Game(int G, Round[] Rounds);

    int GetGame(string g)
        => int.Parse(Regex.Match(g, "\\d+").Value);

    Round GetRound(MatchCollection m)
        => new(R: GetColor("red", m),
            G: GetColor("green", m),
            B: GetColor("blue", m));

    int GetColor(string c, MatchCollection m)
    {
        var match = m.FirstOrDefault(_ => _.Value.Contains(c));
        return match != null
            ? int.Parse(match.Value.Split(' ')[0])
            : 0;
    }

    Round[] GetRounds(string g)
        => g.Split(':')[1]
            .Split(';')
            .Select(_ => Regex
                .Matches(_, "(\\d+ blue)|(\\d+ red)|(\\d+ green)")
                .Pipe(GetRound))
            .ToArray();

    Round MinRound(Game game)
    {
        var r = game.Rounds.Select(_ => _.R).Max();
        var g = game.Rounds.Select(_ => _.G).Max();
        var b = game.Rounds.Select(_ => _.B).Max();
        return new Round(r, g, b);
    }

    long RoundPower(Round r) => r.R * r.G * r.B;

    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(_ => new Game(GetGame(_), GetRounds(_)))
            .Select(MinRound)
            .Select(RoundPower)
            .Sum();
    }
}
