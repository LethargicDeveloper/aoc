using AocLib;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Day11;

public partial class Part02 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var monkeys = this.input
            .SplitEmptyLines()
            .Select(Monkey.Parse)
            .ToList();

        var testValue = monkeys.Aggregate(1, (acc, cur) => acc * cur.TestValue);
        for (int i = 0; i < 10000; ++i)
        {
            foreach (var monkey in monkeys)
            {
                monkey.ThrowItems(monkeys, testValue);
            }
        }

        return monkeys
            .Select(_ => _.InspectedItems)
            .OrderDescending()
            .Take(2)
            .Product();
    }

    partial class Monkey
    {
        readonly Queue<long> Items = new();
        string operand1 = "";
        string operand2 = "";
        string @operator = "";
        int divisor = 1;
        int monkey1;
        int monkey2;

        public long InspectedItems { get; private set; }
        public int TestValue => divisor;

        public void ThrowItems(List<Monkey> monkeys, int testValue)
        {
            while (Items.TryDequeue(out long item))
            {
                InspectedItems++;
                item = CalculateWorryLevel(item);

                var monkey = item % divisor == 0 ? monkey1 : monkey2;
                item %= testValue;

                monkeys[monkey].Items.Enqueue(item);
            }
        }

        long CalculateWorryLevel(long item)
        {
            var op1 = operand1 == "old" ? item : long.Parse(operand1);
            var op2 = operand2 == "old" ? item : long.Parse(operand2);
            return @operator switch
            {
                "+" => op1 + op2,
                "*" => op1 * op2,
                _ => throw new InvalidOperationException()
            };
        }

        public static Monkey Parse(string input)
        {
            var lines = input.Split("\r\n");
            var id = int.Parse(NumberRegex().Match(lines[0]).Value);
            var items = NumberRegex().Matches(lines[1]).Select(_ => long.Parse(_.Value));
            var func = FuncRegex().Matches(lines[2])[0].Groups;
            var divisor = int.Parse(NumberRegex().Match(lines[3]).Value);
            var test1 = TestRegex().Matches(lines[4])[0].Groups;
            var test2 = TestRegex().Matches(lines[5])[0].Groups;

            var monkey = new Monkey();
            foreach (var item in items)
                monkey.Items.Enqueue(item);
            monkey.operand1 = func[1].Value;
            monkey.@operator = func[2].Value;
            monkey.operand2 = func[3].Value;
            monkey.divisor = divisor;
            monkey.monkey1 = test1[1].Value == "true" ? int.Parse(test1[2].Value) : int.Parse(test2[2].Value);
            monkey.monkey2 = test2[1].Value == "false" ? int.Parse(test2[2].Value) : int.Parse(test1[2].Value);
            return monkey;
        }

        [GeneratedRegex("\\d+")]
        private static partial Regex NumberRegex();

        [GeneratedRegex("new = (old|\\d+) ([*|+]) (old|\\d+)")]
        private static partial Regex FuncRegex();

        [GeneratedRegex("(true|false):.*?(\\d+)")]
        private static partial Regex TestRegex();
    }
}
