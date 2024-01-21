using System.Drawing;
using System.Reflection;
using Alternet.UI;
using WinFormsImport;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("ALternet.UI WinFormsImport");
Console.WriteLine("Copyright (c) 2023-2024 AlterNET Software");

#pragma warning disable
void Import()
#pragma warning restore
{
    var path = PathUtils.GetAppSubFolder("Imported.Drawing.System");

    Console.WriteLine($"Graphics import folder: {path}");
    Emit.ImportAssembly(typeof(System.Drawing.Point).Assembly, path!);

    path = PathUtils.GetAppSubFolder("Imported.System.Windows.Forms");
    Emit.Import(typeof(System.Windows.Forms.ScrollBar), path);
}

void ShowIncompleteDocs()
{
    var controls = AssemblyUtils.GetTypeDescendants(typeof(Alternet.UI.Control), true);
    SortedList<string, Type> list = [];
    foreach(var control in controls)
    {
        list.Add(control.Name, control);
    }

    var path = CommonUtils.GetSamplesFolder("Tools")+ @"\..\..\Documentation\\Alternet.UI.Documentation\apidoc\";
    path = Path.GetFullPath(path);

    Console.WriteLine(path);

    var dirs = Directory.GetDirectories(path);

    foreach(var dir in dirs)
    {
        var name = Path.GetFileName(dir).TrimStart('_');
        var index = list.IndexOfKey(name);
        if (index >= 0)
            list.RemoveAt(index);
    }

    foreach(var item in list)
    {
        Console.WriteLine(item.Key);
    }
}

ShowIncompleteDocs();