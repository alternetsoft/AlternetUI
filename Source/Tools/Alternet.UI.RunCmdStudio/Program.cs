using System.Diagnostics;
using System.IO;

using Alternet.Common.License;
using Alternet.UI;

var application = new Application();

RegisterCommands();

Commands.RunApplication(args, true);

application.Dispose();


void RegisterCommands()
{
    Commands.RegisterCommand("logLicense", CmdLogLicense, "-r=logLicense");
}

static void CmdLogLicense(CommandLineArgs args)
{
    Console.WriteLine($"Command: logLicense");

    var provider = new Alternet.Common.License.UILicenseProvider();
    provider.LogExistingLicenses(LogWriter.Console.WriteLineStr);

    Console.WriteLine("Completed");
}
