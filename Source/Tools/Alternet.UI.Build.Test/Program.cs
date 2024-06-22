using System.Diagnostics;
using System.Xml;
using Alternet.UI;
using Alternet.UI.Build.Tasks;

Console.WriteLine("Alternet.UI.Build.Test");

void GenerateCSharpOutput(string filename)
{
    using var stream = File.OpenRead(filename);

    var document = new UIXmlDocument(
        "SampleResource",
        stream,
        WellKnownApiInfo.Provider);

    var output = CSharpUIXmlCodeGenerator.Generate(document, null!, null!);

    File.WriteAllText(CommonUtils.GetAppFolder()+"output.g.cs", output);
}

try
{
#pragma warning disable
    var sample1 = "sample.xml";
    var sample2 = "sample-editorscheme.xml";
#pragma warning restore

    GenerateCSharpOutput(CommonUtils.GetAppFolder() + sample2);
}
catch (Exception e)
{
    if(e is XmlException xmlException)
    {
        var lineNumber = xmlException.LineNumber;
        var linePos = xmlException.LinePosition;
        var sourceUri = xmlException.SourceUri;

        Debug.WriteLine($"{sourceUri} {lineNumber} {linePos}");
    }    

	throw;
}

