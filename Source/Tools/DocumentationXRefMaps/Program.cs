
using XRefMaps;

// From UnityXrefMaps

Console.WriteLine("DocumentationXRefMaps");


	var xrefMap = XrefMap.Load(@"E:\DIMA\AlternetUI\Documentation\Alternet.UI.Documentation\site\xrefmap.yml");
        xrefMap.FixHrefs($"https://docs.alternet-ui.com/api/");
        xrefMap.Save(@"E:\DIMA\AlternetStudio\Documentation\Alternet.Studio.Documentation\alternetui-xrefmap.yml");
