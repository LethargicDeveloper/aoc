namespace _2024.Day24;

public class Part01 : PuzzleSolver<long>
{
    protected override long InternalSolve()
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

        var queue = new Queue<(string, string, string, string)>();
        queue.EnqueueRange(connections);
        
        while (queue.TryDequeue(out var connection))
        {
            var (g1, g2, op, output) = connection;

            if (!gates.ContainsKey(g1) || !gates.ContainsKey(g2))
            {
                queue.Enqueue(connection);
                continue;
            }

            gates[output] = op switch
            {
                "AND" => gates[g1] & gates[g2],
                "OR" => gates[g1] | gates[g2],
                "XOR" => gates[g1] ^ gates[g2],
                _ => throw new Exception()
            };
        }
        
        var binary = gates
                .Where(g => g.Key[0] == 'z')
                .OrderByDescending(g => g.Key)
                .Select(g => g.Value ? '1' : '0')
                .AsString();

        return Convert.ToInt64(binary, 2);
    }
}
