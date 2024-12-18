using System.Text.RegularExpressions;

namespace _2024.Day17;

public class Part01 : PuzzleSolver<string>
{
    private readonly double registerA;
    
    public Part01() {}

    public Part01(double registerA)
    {
        this.registerA = registerA;
    }
    
    protected override string InternalSolve()
    {
        var data = input.SplitEmptyLines();
        
        var registers = data[0]
            .SplitLines()
            .Select(s => Regex.Match(s, "(.): (\\d+)"))
            .ToMatchDictionary(k => k[1], v => double.Parse(v[2]));

        var program = data[1].ParseNumbers<double>()[0];

        registers["A"] = registerA > 0
            ? registerA
            : registers["A"];
        
        var pointer = 0;
        var output = new List<double>();
        
        while (pointer < program.Count)
        {
            var opcode = program[pointer++];
            var operand = program[pointer++];

            switch (opcode)
            {
                case 0: // adv
                    registers["A"] /= Math.Pow(2, ComboOperand(operand));
                    break;
                
                case 1: // bxl
                    registers["B"] = (long)registers["B"] ^ (long)operand;
                    break;
                
                case 2: // bst
                    registers["B"] = ComboOperand(operand) % 8;
                    break;
                
                case 3: // jnz
                    if (registers["A"] != 0)
                        pointer = (int)operand;
                    break;
                
                case 4: // bxc
                    registers["B"] = (long)registers["B"] ^ (long)registers["C"];
                    break;
                
                case 5: // out
                    output.Add(ComboOperand(operand) % 8);
                    break;
                
                case 6: // bdv
                    registers["B"] = registers["A"] / Math.Pow(2, ComboOperand(operand));
                    break;
                
                case 7: // cdv
                    registers["C"] = registers["A"] / Math.Pow(2, ComboOperand(operand));
                    break;
                
                default:
                    throw new Exception($"Invalid opcode {opcode}");
            }
        }
        
        return string.Join(',', output).TrimEnd('4', ',');

        double ComboOperand(double operand) =>
            operand switch
            {
                >= 0 and <= 3 => operand,
                4 => registers["A"],
                5 => registers["B"],
                6 => registers["C"],
                7 => throw new Exception($"Reserved operand {operand}"),
                _ => throw new Exception($"Invalid operand {operand}")
            };
    }
}
