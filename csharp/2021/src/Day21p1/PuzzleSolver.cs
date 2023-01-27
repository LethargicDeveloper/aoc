using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

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
        var functions = this.input
            .SplitLines()
            .Select(_ => $"public long {_.Replace(":", "() {")} }}")
            .Select(_ => MathRegex().Replace(_, m =>
                $"return {m.Groups[1].Value}() {m.Groups[2].Value} {m.Groups[3].Value}();"))
            .Select(_ => NumberRegex().Replace(_, m =>
                $"return {m.Groups[1].Value};"));

        var @class = """
            public class Solver {
            %%func%%
            }
            """.Replace("%%func%%", string.Join(Environment.NewLine, functions));

        var syntaxTree = CSharpSyntaxTree.ParseText(@class);
        var references = new[] {
              MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
              MetadataReference.CreateFromFile(typeof(ValueTuple<>).GetTypeInfo().Assembly.Location)
        };
        var options = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            optimizationLevel: OptimizationLevel.Debug);
        var compliation = CSharpCompilation.Create(
            "InMemoryAssembly",
            references: references,
            options: options).AddSyntaxTrees(syntaxTree);

        using var stream = new MemoryStream();
        var emitResult = compliation.Emit(stream);

        long result = 0;
        if (emitResult.Success)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(stream.ToArray());
            var type = assembly.GetType("Solver")!;
            var solver = Activator.CreateInstance(type);
            result = (long)type.InvokeMember("root", BindingFlags.Default | BindingFlags.InvokeMethod, null, solver, null)!;
        }

        return result;
    }

    [GeneratedRegex("(....) ([\\+\\-\\*\\/]) (....)")]
    private static partial Regex MathRegex();

    [GeneratedRegex("(\\d+)")]
    private static partial Regex NumberRegex();
}