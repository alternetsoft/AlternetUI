using System.Drawing;
using Alternet.UI;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("ALternet.UI WinFormsImport");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

var pointType = typeof(System.Drawing.Point);
var assembly = pointType.Assembly;

var types = assembly.DefinedTypes;

var path = CommonUtils.GetUIFolder("Tools");
if(path is null)
{
    Console.WriteLine("Error: Alternet.UI folder not found.");        
    return;
}

var folderName = "Drawing.System.Imported";
path = CommonProcs.PathAddBackslash(Path.Combine(path, folderName));

Console.WriteLine($"Graphics import folder: {path}");

var indentLev = 0;
string body = string.Empty;

void EmitNewLine(string s)
{
    Emit(s + Environment.NewLine);
}

void Emit(string s)
{
    string prefix = string.Empty;

    for (int i = 0; i < indentLev; i++)
        prefix += "    ";

    body += prefix + s;
}

void EmitBeginIndent()
{
    indentLev++;
}

void EmitEndIndent()
{
    indentLev--;
}

void EmitStartNameSpace(Type type)
{
    var namesp = type.Namespace;

    EmitNewLine("#pragma warning disable");
    EmitNewLine($"namespace Imported.{namesp}");
    EmitNewLine("{");
    EmitBeginIndent();
}

void EmitEndNameSpace(Type type)
{
    EmitEndIndent();
    EmitNewLine("}");
}

void EmitStartClass(Type type)
{
    var classKind = type.IsClass ? "class" : "struct";

    EmitNewLine("/// <summary>");
    EmitNewLine("/// This is imported class.");
    EmitNewLine("/// </summary>");

    EmitNewLine($"public {classKind} {type.Name}");
    EmitNewLine("{");
    EmitBeginIndent();
}

void EmitEndClass(Type type)
{
    EmitEndIndent();
    EmitNewLine("}");
}

List<string> badTypes =
[
    "User32",
];

foreach (var type in types)
{
    if (type.IsNotPublic)
        continue;

    if (type.Name.StartsWith("__"))
        continue;

    if (badTypes.IndexOf(type.Name) >= 0)
        continue;

    Console.WriteLine(type.Name);


    body = string.Empty;

    EmitStartNameSpace(type);
    EmitStartClass(type);
    EmitEndClass(type);
    EmitEndNameSpace(type);

    string filePath = path + "Imported." + type.Name+".cs";

    File.WriteAllText(filePath, body);
}




