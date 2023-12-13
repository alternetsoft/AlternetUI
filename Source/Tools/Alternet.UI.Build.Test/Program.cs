using System.Diagnostics;
using System.Xml;
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

try
{
    GenerateCSharpOutput(CommonUtils.GetAppFolder() + "sample.xml");
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

