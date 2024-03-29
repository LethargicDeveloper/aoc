using AocLib;

namespace _2023.Day07;

// 241344943
public class Part01 : PuzzleSolver<long>
{
    class Hand(string cards, int bid) : IComparable<Hand>
    {
        readonly Dictionary<char, int> Ranks = new()
        {
            {'A', 14},
            {'K', 13},
            {'Q', 12},
            {'J', 11},
            {'T', 10}
        };

        public string Cards => cards;
        public int Bid => bid;

        public int CompareTo(Hand? other)
        {
            var r1 = GetRank(other!);
            var r2 = GetRank(this);
            if (r1 == r2) return 0;
            return r1 > r2 ? -1 : 1;
        }

        int GetRank(Hand other)
        {
            if (Kind(other, 5)) return 60 + Compare(other);
            if (Kind(other, 4)) return 50 + Compare(other);
            if (Kind(other, 3) && Kind(other, 2)) return 40 + Compare(other);
            if (Kind(other, 3)) return 30 + Compare(other);
            if (Pair(other)) return 20 + Compare(other);
            if (Kind(other, 2)) return 10 + Compare(other);
            return Compare(other);
        }

        int Compare(Hand other)
        {
            var cards = Cards.Zip(other.Cards);
            foreach (var card in cards)
            {
                if (!int.TryParse(card.First.ToString(), out var r1))
                    r1 = Ranks[card.First];
                if (!int.TryParse(card.Second.ToString(), out var r2))
                    r2 = Ranks[card.Second];
                if (r1 == r2) continue;
                return r1 > r2 ? -1 : 1;
            }

            return 0;
        }

        bool Pair(Hand other)
        {
            var pairs = other.Cards
               .GroupBy()
               .Where(_ => _.Count() == 2)
               .ToList();

            return pairs.Count == 2;
        }

        bool Kind(Hand other, int num) => other.Cards
            .GroupBy()
            .Any(_ => _.Count() == num);

        public override string ToString() => Cards;
    }

    protected override long InternalSolve()
    {
        var hands = input
            .SplitLines()
            .Select(_ => _.Split(' ').Pipe(x => new Hand(x[0], int.Parse(x[1]))))
            .OrderBy()
            .Select((_, i) => (r: i + 1, c: _, b: (i + 1) * _.Bid))
            .ToList();

        return hands.Select(_ => _.b).Sum();
    }
}
