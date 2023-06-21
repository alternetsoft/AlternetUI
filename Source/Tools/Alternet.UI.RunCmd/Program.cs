// See https://aka.ms/new-console-template for more information
using Alternet.UI;

Console.WriteLine("Alternet.UI.RunCmd");
Console.WriteLine("Copyright © 2023 AlterNET Software");

CommonUtils.ParseCmdLine(args);

var s = CommonUtils.ToString(args);

Console.WriteLine($"Arguments: {s}");
Console.WriteLine();



