using System.Drawing;
using System.Reflection;
using Alternet.UI;
using WinFormsImport;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("ALternet.UI WinFormsImport");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

var path = PathUtils.GetAppSubFolder("Imported.Drawing.System");

Console.WriteLine($"Graphics import folder: {path}");
Emit.ImportAssembly(typeof(System.Drawing.Point).Assembly, path);

path = PathUtils.GetAppSubFolder("Imported.System.Windows.Forms");
Emit.Import(typeof(System.Windows.Forms.ScrollBar), path);

