using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using System;
using System.Text;

[Generator]
public class SampleIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // This one just always generates a class with a string constant
        context.RegisterPostInitializationOutput(ctx =>
        {
            // Example: Produce a value just to trigger generation
            var provider = context.CompilationProvider.Select((compilation, _) => compilation);

            // Register a source output step
            context.RegisterSourceOutput(provider, (spc, compilation) =>
            {
                // Emit a diagnostic during code generation
                spc.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            id: "GEN001",
                            title: "Info from incremental generator",
                            messageFormat: "Source generator executed at {0}",
                            category: "MyGenerator",
                            DiagnosticSeverity.Info,
                            isEnabledByDefault: true),
                        Location.None,
                        DateTime.Now.ToString("T")));

                // Optionally, generate some code too:
                string code = @"// Hello from source generator!";
                spc.AddSource("Hello.g.cs", SourceText.From(code, Encoding.UTF8));
            });
        });
    }
}