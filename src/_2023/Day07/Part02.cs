using AocLib;

namespace _2023.Day07;

// 243101568
public class Part02 : PuzzleSolver<long>
{
    class Hand(string cards, int bid) : IComparable<Hand>
    {
        Dictionary<char, int> Ranks = new()
        {
            {'A', 14},
            {'K', 13},
            {'Q', 12},
            {'T', 10},
            {'9', 9},
            {'8', 8},
            {'7', 7},
            {'6', 6},
            {'5', 5},
            {'4', 4},
            {'3', 3},
            {'2', 2},
            {'J', 1}
        };

        public string Cards => cards;
        public int Bid => bid;

        public int CompareTo(Hand? other)
        {
            if (other!.Cards == Cards) return 0;

            var r1 = GetRank(other);
            var r2 = GetRank(this);

            if (r1 == r2) return 0;
            return r1 > r2 ? -1 : 1;
        }

        int GetRank(Hand other)
        {
            int v = -10;
            foreach (var rank in Ranks.Keys)
            {
                var cards = other.Cards.Replace('J', rank);
                var hand = new Hand(cards, 0);

                int r = 0;
                if (Kind(hand, 5)) r = 60 + Compare(other);
                else if (Kind(hand, 4)) r = 50 + Compare(other);
                else if (Kind(hand, 3) && Kind(hand, 2)) r = 40 + Compare(other);
                else if (Kind(hand, 3)) r = 30 + Compare(other);
                else if (Pair(hand)) r = 20 + Compare(other);
                else if (Kind(hand, 2)) r = 10 + Compare(other);
                else if (r == 0) r = Compare(other);
                if (r > v) v = r;
            }

            return v;
        }

        int Compare(Hand other)
        {
            var cards = Cards.Zip(other.Cards);
            foreach (var card in cards)
            {
                var r1 = Ranks[card.First];
                var r2 = Ranks[card.Second];
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
