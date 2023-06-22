// See https://aka.ms/new-console-template for more information
using Alternet.UI;
using System.Diagnostics;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);
CommandLineArgs.Default.ParseArgs(args);
//CommandLineArgs.Default.ParseDefaults();

//Console.WriteLine($"Arguments: {CommonUtils.ToString(args)}");
//Console.WriteLine($"{CommonUtils.CmdLineExecCommands}");
//Console.WriteLine($"{CommandLineArgs.Default}");

Console.WriteLine();

// Download command
if (CommonUtils.CmdLineExecCommands == "download")
{
    // -r=download Url="https://alternetsoftware.blob.core.windows.net/alternet-ui/wxWidgets-bin-noobjpch-3.2.2.1.zip" Path="e:/file.zip"
    string docUrl = CommandLineArgs.Default.ArgAsString("Url");
    string filePath = CommandLineArgs.Default.ArgAsString("Path");
    await CommonUtils.DownloadFileWithConsoleProgress(docUrl, filePath);
    return;
}

// Run ControlsSample command
if (CommonUtils.CmdLineExecCommands == "runControlsSample")
{
    string path = Path.Combine(
        CommonUtils.GetAppFolder(), 
        "..", "..", "..", "..", "..", "Samples","ControlsSample", "ControlsSample.csproj");
    path = Path.GetFullPath(path);
    string pathFolder = Path.GetDirectoryName(path).TrimEnd('\\').TrimEnd('/');
    Console.WriteLine("Run ControlsSample: "+path);
    CommonUtils.ProcessStart("dotnet", $"run --framework net6.0", pathFolder);
    return;
}