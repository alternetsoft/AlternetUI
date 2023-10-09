using Alternet.UI;
using Alternet.UI.Build.Tasks;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Alternet.UI.Build.Test");

void GenerateCSharpOutput(string filename)
{
    using var stream = File.OpenRead(filename);

    var document = new UIXmlDocument(
        "SampleResource",
        stream,
        WellKnownApiInfo.Provider);

    var output = CSharpUIXmlCodeGenerator.Generate(document);

    File.WriteAllText(CommonUtils.GetAppFolder()+"output.g.cs", output);
}

GenerateCSharpOutput(CommonUtils.GetAppFolder() + "sample.xml");