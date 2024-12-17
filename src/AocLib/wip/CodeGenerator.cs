using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace AocLib;

public class CodeBuilder
{
    record Function(string Name, string Body, string? Return = null, string[]? Args = null);

    readonly List<Function> functions = new();

    public CodeBuilder AddFunction(string name, string body, string? returnType = null, string[]? args = null)
    {
        functions.Add(new Function(name, body, returnType, args));
        return this;
    }

    public dynamic Build()
    {
        var functionCode = new StringBuilder();

        foreach (var func in functions)
        {
            var returnType = func.Return ?? "void";
            var args = func.Args == null ? "" : string.Join(",", func.Args);
            
            functionCode.AppendLine($"public {returnType} {func.Name}({args}) {{ {func.Body} }}");
        }

        var classCode = $"public class GeneratedClass {{ {functionCode} }}";

        var syntaxTree = CSharpSyntaxTree.ParseText(classCode);
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

        if (emitResult.Success)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(stream.ToArray());
            var type = assembly.GetType("GeneratedClass")!;
            return Activator.CreateInstance(type)!;
        }

        throw new Exception("Error generating code.");
    }
}