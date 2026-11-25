using Alternet.UI;

using System.Diagnostics;
using System.IO;

Commands.RunApplication(args);

if (DebugUtils.IsDebugDefinedAndAttached)
{
    Console.WriteLine();
    Commands.CmdWaitEnter(new CommandLineArgs());
}