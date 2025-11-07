using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static int Main(string[] args)
    {
        int allowedPid = 0;
        if (args.Length > 0)
        {
            int.TryParse(args[0], out allowedPid);
        }

        try
        {
            var procs = Process.GetProcessesByName("dotnet")
                               .Where(p => p.Id != allowedPid)
                               .OrderBy(p => p.Id)
                               .ToList();

            if (procs.Count == 0)
            {
                Console.WriteLine("No running dotnet.exe processes detected.");
                return 0;
            }

            Console.Error.WriteLine("ERROR: One or more dotnet.exe processes are running which may lock files required for the build/installation.");
            Console.Error.WriteLine();

            foreach (var p in procs)
            {
                string path = "";
                try
                {
                    // May throw for protected/system processes; swallow exceptions
                    path = p.MainModule?.FileName ?? "";
                }
                catch { /* ignore access errors */ }

                string start = "";
                try { start = p.StartTime.ToString("o"); } catch { start = ""; }

                Console.Error.WriteLine($"  PID: {p.Id}  Name: {p.ProcessName}  Start: {start}  Path: {path}");

                // Try to show the command line by reading the process's command line via ProcessStartInfo if possible:
                // Note: Process.StartInfo doesn't give command line of an existing process — so we skip that here.
                Console.Error.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to enumerate processes: " + ex.Message);
            return 1;
        }

        // non-zero exit to signal MSBuild/installer to stop
        return 1;
    }
}