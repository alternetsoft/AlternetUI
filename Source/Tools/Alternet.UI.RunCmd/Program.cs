// See https://aka.ms/new-console-template for more information
using Alternet.UI;
using System.Diagnostics;
using System.IO;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);
CommandLineArgs.Default.ParseArgs(args);
//CommandLineArgs.Default.ParseDefaults();

//Console.WriteLine($"Arguments: {CommonUtils.ToString(args)}");
//Console.WriteLine($"{CommonUtils.CmdLineExecCommands}");
//Console.WriteLine($"{CommandLineArgs.Default}");

Console.WriteLine();

// download command
if (CommonUtils.CmdLineExecCommands == "download")
{
    // -r=download Url="https://alternetsoftware.blob.core.windows.net/alternet-ui/wxWidgets-bin-noobjpch-3.2.2.1.zip" Path="e:/file.zip"
    string docUrl = CommandLineArgs.Default.ArgAsString("Url");
    string filePath = CommandLineArgs.Default.ArgAsString("Path");
    filePath = Path.GetFullPath(filePath);
    await CommonUtils.DownloadFileWithConsoleProgress(docUrl, filePath);
    return;
}

// runControlsSample command
if (CommonUtils.CmdLineExecCommands == "runControlsSample")
{
    string path = Path.Combine(
        CommonUtils.GetAppFolder(), 
        "..", "..", "..", "..", "..", "Samples","ControlsSample", "ControlsSample.csproj");
    path = Path.GetFullPath(path);
    string? pathFolder = Path.GetDirectoryName(path)?.TrimEnd('\\')?.TrimEnd('/');
    Console.WriteLine("Run ControlsSample: "+path);
    CommonUtils.ProcessStart("dotnet", $"run --framework net6.0", pathFolder);
    return;
}

// waitAnyKey command
if (CommonUtils.CmdLineExecCommands == "waitAnyKey")
{
    Console.WriteLine("Press any key to close this window...");
    Console.ReadKey();
    return;
}

// waitEnter command
if (CommonUtils.CmdLineExecCommands == "waitEnter")
{
    Console.WriteLine("Press ENTER to close this window...");
    Console.ReadLine();
    return;
}


void DeleteBinObjFiles(string path)
{
    path = Path.GetFullPath(path);

    var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".csproj") || s.EndsWith(".vcxproj"));

    foreach (string projFile in files)
    {
        var projPath = Path.GetDirectoryName(projFile);

        if(Path.GetFileName(projFile) == "Alternet.UI.RunCmd.csproj")
        {
            continue;
        }

        var projPathBin = Path.Combine(projPath!, "bin");
        var projPathObj = Path.Combine(projPath!, "obj");
        var filesToDelete = new List<string>();

        if (Directory.Exists(projPathBin))
        {
            var projPathBinFiles = Directory.EnumerateFiles(projPathBin, "*.*", SearchOption.AllDirectories);
            filesToDelete.AddRange(projPathBinFiles);
        }
        
        if (Directory.Exists(projPathObj))
        {
            var projPathObjFiles = Directory.EnumerateFiles(projPathObj, "*.*", SearchOption.AllDirectories);
            filesToDelete.AddRange(projPathObjFiles);
        }
        
        foreach (var s in filesToDelete)
        {
            Console.WriteLine("Deleting file: " + s);
            try
            {
                File.Delete(s);
            }
            catch (Exception)
            {
                Console.WriteLine("WARNING. Error deleting file: " + s);
            }
        }
            
    }
}

if (CommonUtils.CmdLineExecCommands == "deleteBinFolders")
{
    string path = Path.Combine(
        CommonUtils.GetAppFolder(),"..", "..", "..", "..", "..");
    DeleteBinObjFiles(path);
    return;
}