using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpCompress.Common;

namespace Alternet.UI
{
    internal static partial class Commands
    {
        private static readonly Dictionary<string, Action<CommandLineArgs>> commands = new();
        private static readonly List<string> originalCommandNames = new();

        static Commands()
        {
            RegisterCommands();
        }

        public static IEnumerable<string> CommandNames
        {
            get
            {
                return originalCommandNames;
            }
        }

        public static void CmdDownload(CommandLineArgs args)
        {
            // -r=download Url="http://localhost/file.7z" Path="e:/file.7z"
            string docUrl = args.AsString("Url");
            string filePath = args.AsString("Path");
            filePath = Path.GetFullPath(filePath);
            DownloadUtils.DownloadFileWithConsoleProgress(docUrl, filePath).Wait();
        }

        public static void CmdDownloadAndUnzip(CommandLineArgs args)
        {
            // -r=downloadAndUnzip Url="http://localhost/file.zip" Path="e:/file.zip" ExtractTo="e:/ExtractedZip"
            // -r=downloadAndUnzip Url="http://localhost/file.7z" Path="e:/file.7z" ExtractTo="e:/Extracted7z"
            string docUrl = CommandLineArgs.Default.AsString("Url");
            string filePath = CommandLineArgs.Default.AsString("Path");
            string extractToPath = CommandLineArgs.Default.AsString("ExtractTo");
            filePath = Path.GetFullPath(filePath);
            DownloadUtils.DownloadFileWithConsoleProgress(docUrl, filePath).Wait();
            Console.WriteLine();
            SharpCompressUtils.Unzip(filePath, extractToPath);
        }

        public static void CmdWaitEnter(CommandLineArgs args)
        {
            Console.WriteLine("Press ENTER to close this window...");
            Console.ReadLine();
        }

        public static void CmdRunControlsSample(CommandLineArgs args)
        {
            string path = Path.Combine(
                CommonUtils.GetAppFolder(),
                "..", "..", "..", "..", "..", "Samples", "ControlsSample", "ControlsSample.csproj");
            path = Path.GetFullPath(path);
            string? pathFolder = Path.GetDirectoryName(path)?.TrimEnd('\\')?.TrimEnd('/');
            Console.WriteLine("Run ControlsSample: " + path);
            CommonProcs.ProcessStart("dotnet", $"run --framework net8.0", pathFolder);
        }

        public static void CmdWaitAnyKey(CommandLineArgs args)
        {
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();
        }

        public static void CmdUnzip(CommandLineArgs args)
        {
            // -r=unzip Path="e:/file.zip" ExtractTo="e:/ExtractedZip"
            string filePath = CommandLineArgs.Default.AsString("Path");
            string extractToPath = CommandLineArgs.Default.AsString("ExtractTo");
            filePath = Path.GetFullPath(filePath);
            SharpCompressUtils.Unzip(filePath, extractToPath);
        }

        public static void CmdDeleteBinFolders(CommandLineArgs args)
        {
            string path = Path.Combine(
                CommonUtils.GetAppFolder(), "..", "..", "..", "..", "..");
            CommonProcs.DeleteBinObjFiles(path);
        }

        public static void CmdZipFolder(CommandLineArgs args)
        {
            string pathToFolder = Path.GetFullPath(CommandLineArgs.Default.AsString("Folder"));
            string pathToArch = Path.GetFullPath(CommandLineArgs.Default.AsString("Result"));
            var compressionType = CompressionType.Deflate;

            Console.WriteLine($"Command: zipFolder");
            Console.WriteLine($"Folder: {pathToFolder}");
            Console.WriteLine($"Result: {pathToArch}");
            Console.WriteLine($"CompressionType: {compressionType}");

            if (!Directory.Exists(pathToFolder))
            {
                Console.WriteLine("Folder doesn't exist");
                return;
            }

            if (File.Exists(pathToArch))
                File.Delete(pathToArch);

            SharpCompressUtils.ZipFolder(
                pathToFolder,
                pathToArch,
                compressionType);

            Console.WriteLine("Completed");
        }

        public static void CmdFilterLog(CommandLineArgs args)
        {
            string logFilter = CommandLineArgs.Default.AsString("Filter").ToLower();
            string logPath = CommandLineArgs.Default.AsString("Log");
            string resultPath = CommandLineArgs.Default.AsString("Result");
            logPath = Path.GetFullPath(logPath);
            resultPath = Path.GetFullPath(resultPath);

            Console.WriteLine($"Command: filterLog");
            Console.WriteLine($"logPath: {logPath}");
            Console.WriteLine($"resultPath: {resultPath}");
            Console.WriteLine($"logFilter: {logFilter}");

            IEnumerable<string> lines = File.ReadLines(logPath);

            string contents = string.Empty;

            foreach (string s in lines)
            {
                if (s.Contains(logFilter, StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(s);
                    contents += $"{s}{Environment.NewLine}";
                }
            }

            File.WriteAllText(resultPath, contents);
        }

        public static bool RunCommand(string? originalName, CommandLineArgs args)
        {
            if (originalName is null || string.IsNullOrWhiteSpace(originalName))
                return false;

            var name = originalName.ToLower().Trim().Trim('"','\'');

            if(commands.TryGetValue(name, out var action))
            {
                if (action is null)
                    return false;
                try
                {
                    action(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error running command: {originalName}");
                    LogUtils.LogException(e);
                    LogUtils.LogExceptionToConsole(e);
                    return false;                    
                }
                return true;
            }

            return false;
        }

        public static void RegisterCommand(string name, Action<CommandLineArgs> action)
        {
            originalCommandNames.Add(name);
            commands.Add(name.ToLower(), action);
        }

        public static void RegisterCommands()
        {
            RegisterCommand("zipFolder", CmdZipFolder);
            RegisterCommand("unzip", CmdUnzip);
            RegisterCommand("runControlsSample", CmdRunControlsSample);
            RegisterCommand("waitAnyKey", CmdWaitAnyKey);
            RegisterCommand("waitEnter", CmdWaitEnter);
            RegisterCommand("deleteBinFolders", CmdDeleteBinFolders);
            RegisterCommand("filterLog", CmdFilterLog);
            RegisterCommand("download", CmdDownload);
            RegisterCommand("downloadAndUnzip", CmdDownloadAndUnzip);
        }
    }
}