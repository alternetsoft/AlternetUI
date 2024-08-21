using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static partial class CommonProcs
    {
        public static bool ProcessStart(string filePath, string args, string? folder)
        {
            Process process = new ();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = args;
            if(folder!=null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteBinObjFiles(string path)
        {
            path = Path.GetFullPath(path);

            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".csproj") || s.EndsWith(".vcxproj"));

            foreach (string projFile in files)
            {
                var projPath = Path.GetDirectoryName(projFile);

                if (Path.GetFileName(projFile) == "Alternet.UI.RunCmd.csproj")
                {
                    continue;
                }

                var projPathBin = Path.Combine(projPath!, "bin");
                var projPathObj = Path.Combine(projPath!, "obj");
                var filesToDelete = new List<string>();

                if (Directory.Exists(projPathBin))
                {
                    var projPathBinFiles = Directory.EnumerateFiles(
                        projPathBin,
                        "*.*",
                        SearchOption.AllDirectories);
                    filesToDelete.AddRange(projPathBinFiles);
                }

                if (Directory.Exists(projPathObj))
                {
                    var projPathObjFiles = Directory.EnumerateFiles(
                        projPathObj,
                        "*.*",
                        SearchOption.AllDirectories);
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
    }
}
