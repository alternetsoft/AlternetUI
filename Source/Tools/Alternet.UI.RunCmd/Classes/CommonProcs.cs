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

        public static void DeleteFilesByMasks(
            string path,
            string masks = "*",
            Action<string>? writeLine = null)
        {
            masks = masks.Trim();
            var splittedMasks = masks.Split(';');
            foreach(var mask in splittedMasks)
            {
                DeleteFilesByMask(path, mask, writeLine);
            }
        }

        public static void DeleteFilesByMask(
            string path,
            string mask = "*",
            Action<string>? writeLine = null)
        {
            var filesToDelete = new List<string>();

            if (Directory.Exists(path))
            {
                var files = Directory.EnumerateFiles(
                    path,
                    mask,
                    SearchOption.AllDirectories);
                filesToDelete.AddRange(files);
            }
            else
            {
                writeLine?.Invoke($"Folder doesn't exist: [{path}]");
                return;
            }

            foreach (var s in filesToDelete)
            {
                writeLine?.Invoke($"Deleting file: [{s}]");
                try
                {
                    File.Delete(s);
                }
                catch (Exception e)
                {
                    writeLine?.Invoke($"ERROR deleting file: [{s}]");
                    LogUtils.LogExceptionToFile(e);
                }
            }
        }

        public static void DeleteBinObjFiles(string pathToFolder)
        {
            if(string.IsNullOrWhiteSpace(pathToFolder))
            {
                Console.WriteLine($"Path parameter is not specified.");
                return;
            }

            var path = Path.GetFullPath(pathToFolder);

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Folder doesn't exist: [{path}].");
                return;
            }

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

                DeleteFilesByMask(projPathBin, "*", Console.WriteLine);
                DeleteFilesByMask(projPathObj, "*", Console.WriteLine);
            }
        }
    }
}
