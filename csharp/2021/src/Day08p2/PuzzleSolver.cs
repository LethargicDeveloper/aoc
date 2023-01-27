using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public partial class PuzzleSolver
{
    static readonly int[][] Digits = new int[10][]
    {
        new[] { 1, 1, 1, 0, 1, 1, 1 },
        new[] { 0, 0, 1, 0, 0, 1, 0 },
        new[] { 1, 0, 1, 1, 1, 0, 1 },
        new[] { 1, 0, 1, 1, 0, 1, 1 },
        new[] { 0, 1, 1, 1, 0, 1, 0 },
        new[] { 1, 1, 0, 1, 0, 1, 1 },
        new[] { 1, 1, 0, 1, 1, 1, 1 },
        new[] { 1, 0, 1, 0, 0, 1, 0 },
        new[] { 1, 1, 1, 1, 1, 1, 1 },
        new[] { 1, 1, 1, 1, 0, 1, 1 },
    };

    readonly string input;

    public PuzzleSolver()
    {
        this.input = File.ReadAllText("01.txt");
    }

    [Benchmark]
    public long Solve()
    {
        var inputs = input
            .SplitLines()
            .Select(_ =>
            {
                var entry = _.Split(" | ");
                var signals = entry[0].Split(' ');
                var output = entry[1].Split(' ');
                return (signals, output);
            }).ToList();

        int result = 0;

        var map = new char[7];
        foreach (var (signals, output) in inputs)
        {
            var one = signals.First(_ => _.Length == NumLength(1));
            var four = signals.First(_ => _.Length == NumLength(4));
            var seven = signals.First(_ => _.Length == NumLength(7));
            var eight = signals.First(_ => _.Length == NumLength(8));

            map[0] = seven.Except(one).First();
            map[4] = signals
                .Where(_ => _.Length == 5)
                .SelectMany(_ => _.Select(c => c))
                .GroupBy()
                .Where(_ => _.Count() == 1)
                .Select(_ => _.Key)
                .Except(four)
                .First();
            map[1] = signals
                .Where(_ => _.Length == 5)
                .SelectMany(_ => _.Select(c => c))
                .GroupBy()
                .Where(_ => _.Count() == 1)
                .Select(_ => _.Key)
                .Intersect(four)
                .First();
            map[3] = four.Except(map).Except(one).First();
            map[6] = eight.Except(map).Except(one).First();
            map[2] = signals
                .Where(_ => _.Length == 6)
                .SelectMany(_ => _.Select(c => c))
                .GroupBy()
                .Where(_ => _.Count() == 2)
                .Select(_ => _.Key)
                .Intersect(one)
                .First();
            map[5] = eight.Except(map).First();

            var digits = new List<string>();
            foreach (var o in output)
            {
                string digit = FindDigit(o, map);
                digits.Add(digit);
            }

            result += int.Parse(string.Join("", digits));
            for (int i = 0; i < map.Length; ++i) map[i] = '\0';

        }

        return result;
    }

    static int NumLength(int num) => Digits[num].Where(_ => _ == 1).Count();

    static string FindDigit(string num, char[] map)
    {
        var indexes = map
            .Select((_, i) => (m: _, i))
            .Join(num, lk => lk.m, rk => rk, (lk, _) => (int?)lk.i);

        for (int i = 0; i < Digits.Length; ++i)
        {
            var found = true;
            var digit = Digits[i];
            for (int d = 0; d < digit.Length; ++d)
            {
                var ix = indexes.FirstOrDefault(_ => _ == d);
                var invalid_letter = ix != null && digit[d] == 0;
                var missing_letter = ix == null && digit[d] == 1;
                if (invalid_letter || missing_letter)
                {
                    found = false;
                    break;
                }
            }

            if (found) return i.ToString();
        }

        throw new Exception("Number not found :(");
    }
}