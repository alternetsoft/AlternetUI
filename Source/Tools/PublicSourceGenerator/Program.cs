using CommandLine;
using Alternet.UI.PublicSourceGenerator.Generators.Components;
using Alternet.UI.PublicSourceGenerator.Generators.Samples;
using System;

namespace Alternet.UI.PublicSourceGenerator
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                return CommandLine.Parser.Default.ParseArguments<ComponentsSourceGeneratorOptions, SamplesSourceGeneratorOptions>(args)
                  .MapResult(
                    (ComponentsSourceGeneratorOptions o) => ComponentsSourceGenerator.Generate(o),
                    (SamplesSourceGeneratorOptions o) => SamplesSourceGenerator.Generate(o),
                    errors => 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }
    }
}