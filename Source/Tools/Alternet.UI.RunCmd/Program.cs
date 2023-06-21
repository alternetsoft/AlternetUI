// See https://aka.ms/new-console-template for more information
using Alternet.UI;
using System.Diagnostics;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright (c) 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);

//Console.WriteLine($"Arguments: {CommonUtils.ToString(args)}");
//Console.WriteLine();
//Console.WriteLine($"{CommonUtils.CmdLineExecCommands}");
//Console.WriteLine();

CommandLineArgs.Default.ParseArgs(args);
//CommandLineArgs.Default.ParseDefaults();

//Console.WriteLine($"{CommandLineArgs.Default}");
Console.WriteLine();

if (CommonUtils.CmdLineExecCommands == "download")
{
    string docUrl = CommandLineArgs.Default.ArgAsString("Url");
    string filePath = CommandLineArgs.Default.ArgAsString("Path");
    await CommonUtils.DownloadFileWithConsoleProgress(docUrl, filePath);
}