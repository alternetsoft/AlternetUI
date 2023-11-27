// See https://aka.ms/new-console-template for more information
using Alternet.UI;
using System.Diagnostics;
using System.IO;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);
CommandLineArgs.Default.ParseArgs(args);
//CommandLineArgs.Default.ParseDefaults();

//Console.WriteLine($"{CommandLineArgs.Default}");

Console.WriteLine();


if(CommonUtils.CmdLineExecCommands is null)
{
    Console.WriteLine("No commands are specified.");
    return;
}

Console.WriteLine($"Arguments: {CommonUtils.ToString(args)}");
Console.WriteLine($"Commands: {CommonUtils.CmdLineExecCommands}");
Console.WriteLine();


bool IsCommand(string s)
{
    if (s is null || CommonUtils.CmdLineExecCommands is null)
        return false;
    return s.Equals(CommonUtils.CmdLineExecCommands, StringComparison.CurrentCultureIgnoreCase);
}

// download command
if (IsCommand("download"))
{
    // -r=download Url="https://alternetsoftware.blob.core.windows.net/alternet-ui/wxWidgets-bin-noobjpch-3.2.2.1.zip" Path="e:/file.zip"
    string docUrl = CommandLineArgs.Default.ArgAsString("Url");
    string filePath = CommandLineArgs.Default.ArgAsString("Path");
    filePath = Path.GetFullPath(filePath);
    await CommonUtils.DownloadFileWithConsoleProgress(docUrl, filePath);
    return;
}

// runControlsSample command
if (IsCommand("runControlsSample"))
{
    string path = Path.Combine(
        CommonUtils.GetAppFolder(), 
        "..", "..", "..", "..", "..", "Samples","ControlsSample", "ControlsSample.csproj");
    path = Path.GetFullPath(path);
    string? pathFolder = Path.GetDirectoryName(path)?.TrimEnd('\\')?.TrimEnd('/');
    Console.WriteLine("Run ControlsSample: "+path);
    CommonUtils.ProcessStart("dotnet", $"run --framework net8.0", pathFolder);
    return;
}

// waitAnyKey command
if (IsCommand("waitAnyKey"))
{
    Console.WriteLine("Press any key to close this window...");
    Console.ReadKey();
    return;
}

// waitEnter command
if (IsCommand("waitEnter"))
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

// deleteBinFolders command
if (IsCommand("deleteBinFolders"))
{
    string path = Path.Combine(
        CommonUtils.GetAppFolder(),"..", "..", "..", "..", "..");
    DeleteBinObjFiles(path);
    return;
}

// filterLog command
if (IsCommand("filterLog"))
{
    string logFilter = CommandLineArgs.Default.ArgAsString("Filter").ToLower();
    string logPath = CommandLineArgs.Default.ArgAsString("Log");
    string resultPath = CommandLineArgs.Default.ArgAsString("Result");
    logPath = Path.GetFullPath(logPath);
    resultPath = Path.GetFullPath(resultPath);

    Console.WriteLine($"Command: filterLog");
    Console.WriteLine($"logPath: {logPath}");
    Console.WriteLine($"resultPath: {resultPath}");
    Console.WriteLine($"logFilter: {logFilter}");

    IEnumerable<string> lines = File.ReadLines(logPath);

    string contents = string.Empty;

    foreach (string s in lines)
    {
        if(s.ToLower().Contains(logFilter))
        {
            Console.WriteLine(s);
            contents += $"{s}{Environment.NewLine}";
        }
    }
    File.WriteAllText(resultPath, contents);
    return;
}