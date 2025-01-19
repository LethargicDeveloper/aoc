using MoreLinq;

namespace _2024.Day24;

public class Part02 : PuzzleSolver<string>
{
    protected override string InternalSolve()
    {
        var parsedInput = input.SplitEmptyLines();
        var gates = parsedInput[0]
            .SplitLines()
            .ToDictionary(k => k[..3], v => v[5..] == "1");
        
        var connections = parsedInput[1]
            .SplitLines()
            .Select(c =>
            {
                var split =  c.S(' ');
                var g1 = split[0];
                var op = split[1];
                var g2 = split[2];
                var output = split[4];

                return (Gate1: g1, Gate2: g2, Op: op, OutputGate: output);
            }).ToList();

        var startingConnections = connections
            .Where(c => (c.Gate1.StartsWith('x') && c.Gate2.StartsWith('y')) ||
                        (c.Gate1.StartsWith('y') && c.Gate2.StartsWith('x')))
            .OrderBy(c => string.Join(",", new[] { c.Gate1, c.Gate2 }.Order()))
            .ThenByDescending(c => c.Op);

        (string Gate1, string Gate2, string Op, string OutputGate) Find(string gate1, string gate2, string op) =>
            connections.FirstOrDefault(c => ((c.Gate1 == gate1 && c.Gate2 == gate2) || (c.Gate1 == gate2 && c.Gate2 == gate1)) && c.Op == op);

        (string Gate1, string Gate2, string Op, string OutputGate) FindByOutput(string cin, string output, string op) =>
            connections.FirstOrDefault(c => ((c.Gate1 == cin) || (c.Gate2 == cin)) && c.OutputGate == output && c.Op == op);
        
        (string Gate1, string Gate2, string Op, string OutputGate) FindCGate(string cin, string op) =>
            connections.FirstOrDefault(c => ((c.Gate1 == cin) || (c.Gate2 == cin)) && c.Op == op);
        
        var swap = new HashSet<string>();
        
        string cin = "";
        foreach (var connection in startingConnections.Batch(2).ToList())
        {
            var con1 = connection[0];
            if (con1.Op != "XOR")
                throw new Exception("you done messed up");
            
            var con2 = connection[1];
            if (con2.Op != "AND")
                throw new Exception("you done messed up");

            var xyXOR = con1.OutputGate;
            var xyAND = con2.OutputGate;
            if (string.IsNullOrEmpty(cin))
            {
                cin = con2.OutputGate;
            }
            else
            {
                /*
                 *  X -----*
                 *         |--> XOR -------*
                 *         |               |
                 *         |--> AND --*    |
                 *  Y -----*          |    |--> XOR -----> Z
                 *                    |    |
                 *                    |    |--> AND --*
                 *                    |    |          |--> OR C
                 *  C ----------------|----*          |
                 *                    *---------------*
                 */
                var zgate = $"z{con1.Gate1[1..]}";
                
                // xyXOR, Cin -> XOR = Z
                
                var xyXORcXOR = Find(xyXOR, cin, "XOR");
                if (string.IsNullOrEmpty(xyXORcXOR.Gate1))
                {
                    var z = FindByOutput(cin, zgate, "XOR");
                    var wrongGate = z.Gate1 == cin ? z.Gate2 : z.Gate1;
                    
                    swap.Add(xyXOR);
                    swap.Add(wrongGate);
                    //Console.WriteLine($"xyXORcXOR : {xyXOR} x {wrongGate}");

                    if (xyAND == wrongGate)
                        xyAND = xyXOR;
                    
                    xyXOR = wrongGate;
                }
                else if (!xyXORcXOR.OutputGate.StartsWith("z"))
                {
                    swap.Add(xyXORcXOR.OutputGate);
                    swap.Add(zgate);
                    //Console.WriteLine($"xyXORcXOR (z) : {xyXORcXOR.OutputGate} x {zgate}");

                    if (xyAND == zgate)
                        xyAND = xyXORcXOR.OutputGate;
                }
                
                // xyXOR, Cin -> AND = xyXORcAND
                
                var xyXORcAND = Find(xyXOR, cin, "AND");
                if (xyXORcAND.OutputGate.StartsWith("z"))
                {
                    xyXORcAND = xyXORcAND with
                    {
                        OutputGate = xyXORcXOR.OutputGate
                    };
                }
                
                // xyAND, xyXORcAND -> OR = Cout
                
                var cout = Find(xyAND, xyXORcAND.OutputGate, "OR");
                if (cout.OutputGate.StartsWith("z"))
                {
                    cout = cout with
                    {
                        OutputGate = xyXORcXOR.OutputGate
                    };
                }
                
                cin = cout.OutputGate;
            }
        }

        return string.Join(",", swap.OrderBy());
    }
}