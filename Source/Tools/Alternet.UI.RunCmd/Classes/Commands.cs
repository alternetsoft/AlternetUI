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
            string docUrl = args.AsString("Url");
            string filePath = args.AsString("Path");
            filePath = Path.GetFullPath(filePath);
            DownloadUtils.DownloadFileWithConsoleProgress(docUrl, filePath).Wait();
        }

        public static void CmdDownloadAndUnzip(CommandLineArgs args)
        {
            string docUrl = args.AsString("Url");
            string filePath = args.AsString("Path");
            string extractToPath = args.AsString("ExtractTo");
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
            string filePath = args.AsString("Path");
            string extractToPath = args.AsString("ExtractTo");
            filePath = Path.GetFullPath(filePath);
            SharpCompressUtils.Unzip(filePath, extractToPath);
        }

        public static void CmdDeleteBinObjSubFolders(CommandLineArgs args)
        {
            string filePath = args.AsString("Path");
            CommonProcs.DeleteBinObjFiles(filePath);
        }

        public static void CmdRemoveFolder(CommandLineArgs args)
        {
            string path = args.AsString("Path");

            Console.WriteLine($"RemoveFolder: [{path}]");
            try
            {
                CommonProcs.DeleteFilesByMasks(path, "*", Console.WriteLine);
                if(Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing folder: [{path}]");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdGenerateBindable(CommandLineArgs args)
        {
            string pathToSettings = args.AsString("Settings");
            pathToSettings = Path.GetFullPath(pathToSettings);

            Console.WriteLine($"Generate Bindable Properties...");
            Console.WriteLine($"Settings: {pathToSettings}");

            DebugUtils.DebugCallIf(false, GenerateBindableSettings.TestSave);

            if(!File.Exists(pathToSettings))
            {
                Console.WriteLine($"Settings file not found");
                return;
            }

            try
            {
                var settings = XmlUtils.DeserializeFromFile<GenerateBindableSettings>(pathToSettings);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                foreach (var item in settings.Items)
                {
                    if (item.Ignore ?? false)
                        continue;

                    item.Prepare();

                    Console.WriteLine($"======================");

                    Console.WriteLine($"RootFolder: {item.RootFolder}");
                    Console.WriteLine($"PathToDll: {item.PathToDll}");
                    Console.WriteLine($"TypeName: {item.TypeName}");
                    Console.WriteLine($"PathToResult: {item.PathToResult}");
                    Console.WriteLine($"ResultTypeName: {item.ResultTypeName}");

                    if (!File.Exists(item.PathToDll))
                    {
                        Console.WriteLine($"File not exists: {item.PathToDll}");
                        continue;
                    }

                    var destFolder = Path.GetDirectoryName(item.PathToResult);

                    if (!Path.Exists(destFolder))
                    {
                        Console.WriteLine($"Path not exists: {destFolder}");
                        continue;
                    }

                    item.Execute(settings);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error generating bindable properties");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdDeleteFilesByMasks(CommandLineArgs args)
        {
            string path = args.AsString("Path");
            string masks = args.AsString("Masks");
            CommonProcs.DeleteFilesByMasks(path, masks, Console.WriteLine);
        }

        public static void CmdDeleteBinFolders(CommandLineArgs args)
        {
            string path = Path.Combine(
                CommonUtils.GetAppFolder(), "..", "..", "..", "..", "..");
            CommonProcs.DeleteBinObjFiles(path);
        }

        public static void CmdZipFolder(CommandLineArgs args)
        {
            string pathToFolder = args.AsString("Folder");
            string pathToArch = args.AsString("Result");
            string rootSubFolder = args.AsString("RootSubFolder");

            var compressionType = CompressionType.Deflate;

            Console.WriteLine($"Command: zipFolder");
            Console.WriteLine($"Folder: {pathToFolder}");
            Console.WriteLine($"Result: {pathToArch}");
            Console.WriteLine($"RootSubFolder: {rootSubFolder}");
            Console.WriteLine($"CompressionType: {compressionType}");

            pathToFolder = Path.GetFullPath(pathToFolder);
            pathToArch = Path.GetFullPath(pathToArch);

            if (!Directory.Exists(pathToFolder))
            {
                Console.WriteLine($"Folder doesn't exist: [{pathToFolder}]");
                return;
            }

            if (File.Exists(pathToArch))
                File.Delete(pathToArch);

            SharpCompressUtils.ZipFolderWithRootSubFolder(
                pathToFolder,
                pathToArch,
                rootSubFolder,
                compressionType);

            Console.WriteLine("Completed");
        }

        public static void CmdReplaceInFiles(CommandLineArgs args)
        {
            var pathToConfig = args.AsString("Settings");
            var pathToConfigFull = Path.GetFullPath(pathToConfig);
            var configNames = args.AsString("Names");

            Console.WriteLine($"Command: replaceInFiles");
            Console.WriteLine($"Path to config: {pathToConfigFull}");
            Console.WriteLine($"Config names: {configNames}");

            configNames = $";{configNames};";

            if (!File.Exists(pathToConfigFull))
            {
                Console.WriteLine($"Settings file doesn't exist: [{pathToConfigFull}]");
                return;
            }

            try
            {
                var settings = XmlUtils.DeserializeFromFile<ReplaceInFilesSettings>(pathToConfigFull);

                if (settings is null)
                {
                    Console.WriteLine($"Settings file reading error");
                    return;
                }

                foreach (var item in settings.Items)
                {
                    if (!configNames.Contains($";{item.Name};"))
                        continue;

                    item.Prepare(settings, pathToConfigFull);

                    Console.WriteLine($"======================");
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"PathToFile: {item.PathToFile}");
                    Console.WriteLine($"PathToResultFile: {item.PathToResultFile}");

                    if (!File.Exists(item.PathToFile))
                    {
                        Console.WriteLine($"File not exists: {item.PathToFile}");
                        continue;
                    }

                    var destFolder = Path.GetDirectoryName(item.PathToResultFile);

                    if (!Path.Exists(destFolder))
                    {
                        Console.WriteLine($"Path not exists: {destFolder}");
                        continue;
                    }

                    item.Execute(settings);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error replace in files");
                Console.WriteLine($"Error info logged to file");
                LogUtils.LogExceptionToFile(e);
            }
        }

        public static void CmdFilterLog(CommandLineArgs args)
        {
            string logFilter = args.AsString("Filter").ToLower();
            string logPath = args.AsString("Log");
            string resultPath = args.AsString("Result");
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

        public static void RegisterCommand(
            string name,
            Action<CommandLineArgs> action,
            string? example = null)
        {
            originalCommandNames.Add(name);
            commands.Add(name.ToLower(), action);
        }

        public static void RegisterCommands()
        {
            RegisterCommand(
                "replaceInFiles",
                CmdReplaceInFiles,
                "");

            RegisterCommand(
                "del",
                CmdDeleteFilesByMasks,
                "");

            RegisterCommand(
                "rmdir",
                CmdRemoveFolder,
                "");

            RegisterCommand(
                "zipFolder",
                CmdZipFolder,
                "");

            RegisterCommand(
                "unzip",
                CmdUnzip,
                "-r=unzip Path=\"e:/file.zip\" ExtractTo=\"e:/ExtractedZip\"");

            RegisterCommand(
                "runControlsSample",
                CmdRunControlsSample,
                "");

            RegisterCommand(
                "waitAnyKey",
                CmdWaitAnyKey,
                "");

            RegisterCommand(
                "waitEnter",
                CmdWaitEnter,
                "");

            RegisterCommand(
                "deleteBinFolders",
                CmdDeleteBinFolders,
                "");

            RegisterCommand(
                "generateBindable",
                CmdGenerateBindable,
                "-r=generateBindable Settings=\"E:\\GenerateBindableSettings.xml\"");

            RegisterCommand(
                "deleteBinObjSubFolders",
                CmdDeleteBinObjSubFolders,
                "-r=deleteBinObjSubFolders Path=\"E:\\SomeFolder\"");

            RegisterCommand(
                "filterLog",
                CmdFilterLog,
                "");

            RegisterCommand(
                "download",
                CmdDownload,
                "-r=download Url=\"http://localhost/file.7z\" Path=\"e:/file.7z\"");

            RegisterCommand(
                "downloadAndUnzip",
                CmdDownloadAndUnzip,
                "-r=downloadAndUnzip Url=\"http://localhost/file.7z\" Path=\"e:/file.7z\" ExtractTo=\"e:/Extracted7z\"");

            originalCommandNames.Sort();
        }
    }
}