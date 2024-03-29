﻿using AocLib;

namespace _2022.Day25;

public partial class Part01 : PuzzleSolver<string>
{
    protected override string InternalSolve()
    {
        var dec = input
            .SplitLines()
            .Select(_ => new Snafu(_))
            .Select(_ => _.ToDecimal())
            .Sum();

        return Snafu.FromDecimal(dec);
    }

    readonly struct Snafu
    {
        readonly string value;

        public Snafu(string value)
        {
            this.value = value;
        }

        public long ToDecimal()
        {
            long dec = 0;
            for (int i = 0; i < value.Length; ++i)
            {
                char digit = value[i];
                long place = (long)Math.Pow(5, value.Length - i - 1);
                dec += digit switch
                {
                    '-' => -1 * place,
                    '=' => -2 * place,
                    var n => int.Parse(n.ToString()) * place
                };
            }

            return dec;
        }

        public static Snafu FromDecimal(long dec)
        {
            var num = "";
            while (dec > 0)
            {
                num += (dec % 5) switch
                {
                    0 => "0",
                    1 => "1",
                    2 => "2",
                    3 => "=",
                    4 => "-",
                    _ => throw new Exception("bad result")
                };

                dec = (long)Math.Round(dec / 5.0);
            }

            return new Snafu(new string(num.Reverse().ToArray()));
        }

        public static implicit operator string(Snafu snafu)
            => snafu.value;
    }
}
