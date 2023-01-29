using BenchmarkDotNet.Attributes;

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
        var snailfish = new Snailfish();
        var lines = input.SplitLines();

        foreach (var line in lines)
            snailfish.Add(line);

        return snailfish.Magnitude();
    }
}

class Snailfish
{
    string snailfish = string.Empty;

    public Snailfish() { }
    public Snailfish(string initial)
    {
        this.snailfish = Reduce(initial);
    }

    public Snailfish Add(string snailfish)
    {
        this.snailfish = string.IsNullOrEmpty(this.snailfish)
            ? Reduce(snailfish)
            : Reduce($"[{this.snailfish},{snailfish}]");
        return this;
    }

    static List<string> Expand(string snailfish)
    {
        var expanded = new List<string>();
        for (int i = 0; i < snailfish.Length; ++i)
        {
            if (char.IsDigit(snailfish[i]))
            {
                int end = snailfish.AsSpan(i).IndexOfAny(',', ']');
                var num = snailfish.AsSpan(i, end).ToString();
                expanded.Add(num);
                i += end - 1;
            }
            else expanded.Add(snailfish[i].ToString());
        }

        return expanded;
    }

    static string Reduce(string snailfish)
    {
        var expanded = Expand(snailfish);

        List<string> result = expanded;
        int length = 0;

        while (length != result.Count)
        {
            while (length != result.Count)
            {
                length = result.Count;
                result = Explode(result);
            }

            length = result.Count;
            result = Split(result);
        }

        return string.Join(string.Empty, result);
    }

    static List<string> Explode(List<string> snailfish)
    {
        var exploded = new List<string>();
        var hasExploded = false;

        int parenCount = 0;
        for (int i = 0; i < snailfish.Count; ++i)
        {
            if (snailfish[i] == "[")
            {
                parenCount++;
                if (parenCount > 4 && !hasExploded)
                {
                    if (int.TryParse(snailfish[i + 1], out int n1) &&
                        int.TryParse(snailfish[i + 3], out int n2))
                    {
                        hasExploded = true;

                        var prevNum = GetPrevNum(snailfish, i);
                        var nextNum = GetNextNum(snailfish, i + 4);
                        if (prevNum.Index > -1) exploded[prevNum.Index] = (n1 + prevNum.Num).ToString();
                        if (nextNum.Index > -1) snailfish[nextNum.Index] = (n2 + nextNum.Num).ToString();
                        exploded.Add("0");
                        if (snailfish[i - 1] != ",")
                        {
                            exploded.Add(",");
                            i++;
                        }
                        i += 4;
                        continue;
                    }
                }
            }
            else if (snailfish[i] == "]") parenCount--;

            exploded.Add(snailfish[i]);
        }

        return exploded;
    }

    static (int Index, int Num) GetPrevNum(List<string> snailfish, int startIndex)
    {
        for (int i = startIndex; i > 0; --i)
            if (int.TryParse(snailfish[i], out var num))
                return (i, num);

        return (-1, 0);
    }

    static (int Index, int Num) GetNextNum(List<string> snailfish, int startIndex)
    {
        for (int i = startIndex; i < snailfish.Count; ++i)
            if (int.TryParse(snailfish[i], out var num))
                return (i, num);

        return (-1, 0);
    }

    static List<string> Split(List<string> snailfish)
    {
        var split = new List<string>();
        var hasSplit = false;

        for (int i = 0; i < snailfish.Count; ++i)
        {
            if (int.TryParse(snailfish[i], out var num))
            {
                if (num >= 10 && !hasSplit)
                {
                    hasSplit = true;
                    var n1 = (int)Math.Floor(num / 2.0);
                    var n2 = (int)Math.Ceiling(num / 2.0);
                    split.Add("[");
                    split.Add(n1.ToString());
                    split.Add(",");
                    split.Add(n2.ToString());
                    split.Add("]");
                    continue;
                }
            }

            split.Add(snailfish[i]);
        }

        return split;
    }

    public long Magnitude()
    {
        var expand = Expand(this.snailfish);

        var stack = new Stack<int>();
        for (int i = 0; i < expand.Count; ++i)
        {
            if (int.TryParse(expand[i], out var n1))
            {
                if (int.TryParse(expand[i + 2], out var n2))
                {
                    stack.Push((n1 * 3) + (n2 * 2));
                    i += 3;
                }
                else
                {
                    stack.Push(n1);
                }
            }
            else if (expand[i] == "]")
            {
                var n2 = stack.Pop();
                n1 = stack.Pop();
                stack.Push((n1 * 3) + (n2 * 2));
            }
        }

        return stack.Pop();
    }

    public override string ToString() => snailfish;
}