using System.Drawing;
using System.Reflection;
using Alternet.UI;
using WinFormsImport;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("ALternet.UI WinFormsImport");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

var path = CommonUtils.GetUIFolder("Tools");
if(path is null)
{
    Console.WriteLine("Error: Alternet.UI folder not found.");        
    return;
}

var folderName = "Drawing.System.Imported";
path = CommonProcs.PathAddBackslash(Path.Combine(path, folderName));

Console.WriteLine($"Graphics import folder: {path}");

Emit.ImportAssembly(typeof(System.Drawing.Point).Assembly, path);

Emit.Import(typeof(System.Windows.Forms.ScrollBar), path);

