using Alternet.UI;

using System.Diagnostics;
using System.IO;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright (c) 2023-2024 AlterNET Software");

CommandLineArgs.Default.Parse(args);

var commandNames = CommandLineArgs.Default.AsString("-r");

Console.WriteLine();

if(string.IsNullOrWhiteSpace(commandNames))
{
    Console.WriteLine("No commands are specified.");
    Console.WriteLine();
    Console.WriteLine("Known commands:");
    Console.WriteLine();

    var commands = Commands.CommandNames;

    foreach(var command in commands)
    {
        Console.WriteLine(command);
    }

    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("Alternet.UI.RunCmd.exe -r=<CommandName> [Parameters]");

    Console.WriteLine();
    Console.WriteLine("Usage example:");
    Console.WriteLine("Alternet.UI.RunCmd.exe -r=zipFolder Folder=\"d:\\testFolder\" Result=\"d:\\result.zip\"");

    return;
}

Console.WriteLine($"Commands: {commandNames}");
Console.WriteLine();
Console.WriteLine("Arguments:");

foreach(var a in args)
{
    Console.WriteLine($"[{a}]");
}

Console.WriteLine();

Commands.RunCommand(commandNames, CommandLineArgs.Default);

