// See https://aka.ms/new-console-template for more information
using Alternet.UI;
using System.Diagnostics;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright © 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);

Console.WriteLine($"Arguments: {CommonUtils.ToString(args)}");
Console.WriteLine();
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

    Console.WriteLine("Downloading:");
    Console.WriteLine($"Url: {docUrl}");
    Console.WriteLine($"Path: {filePath}");

    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    await CommonUtils.DownloadFile(docUrl, filePath, CommonUtils.DownloadFileProgressChangedToConsole);

    stopWatch.Stop();
    TimeSpan ts = stopWatch.Elapsed;

    // Format and display the TimeSpan value. 
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
        ts.Hours, ts.Minutes, ts.Seconds);
    Console.WriteLine("Download Time: " + elapsedTime);

}