using AocLib;
using System.Text.RegularExpressions;

namespace _2023.Day02;

// 2156
public partial class Part01 : PuzzleSolver<long>
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

    bool IsGameValid(Game g)
        => g.Rounds.All(_ => _.R <= 12 && _.G <= 13 && _.B <= 14);

    protected override long InternalSolve()
    {
        return input
            .SplitLines()
            .Select(_ => new Game(GetGame(_), GetRounds(_)))
            .Where(IsGameValid)
            .Select(_ => _.G)
            .Sum();
    }
}
