using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class RezipCommandSetting
    {
        public bool? Ignore { get; set; }

        public string? RootFolder { get; set; }

        public string? PathToZip { get; set; }

        public string? ZipMask { get; set; }

        public string? PathToResult { get; set; }

        public string? FileMasks { get; set; }

        public string? CommandToRun { get; set; }

        public string? CommandArgs { get; set; }

        public bool? RecreateZip { get; set; }

        public string? ProcessZipFile(string zipFile, string pathToResult)
        {
            Console.WriteLine($"Processing zip file: {zipFile}");

            var tempPath = PathUtils.GetTempPathUniquePerApp("rezip");
            var tempPathForZip = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(zipFile));

            var extractToFolder = Path.Combine(pathToResult, tempPathForZip);
            SharpCompressUtils.Unzip(zipFile, extractToFolder);

            var files = FileUtils.FindFiles(extractToFolder, FileMasks ?? "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Console.WriteLine($"File in zip: {file}");

                var fileFolder = Path.GetDirectoryName(file);

                if (CommandToRun is null)
                    continue;

                var commandArgs = CommandArgs?.Replace("$(FilePath)", file).Replace("$(FileFolder)", fileFolder ?? "");

                Console.WriteLine($"Executing \"{CommandToRun}\" with arguments: {commandArgs}");

                if (CommandToRun == "ExecuteTerminalCommand")
                {
                    if (string.IsNullOrWhiteSpace(commandArgs))
                    {
                        Console.WriteLine("ExecuteTerminalCommand arguments are empty. Skipping command execution.");
                        continue;
                    }

                    ExecuteTerminalCommand(commandArgs, fileFolder);
                }
                else
                if (CommandToRun == "SignToolAlternet")
                {
                    SignTool(file);
                }
                else
                {
                    Console.WriteLine($"Command is unknown");
                }

                static void SignTool(string file)
                {
                    var signToolPath = MswUtils.SignToolPath.Value;

                    if (string.IsNullOrEmpty(signToolPath) || !File.Exists(signToolPath))
                    {
                        Console.WriteLine("SignTool.exe not found.");
                        return;
                    }

                    string cmd = $"\"{signToolPath}\" sign /fd SHA256 /td SHA256 /tr http://timestamp.digicert.com /n \"ALTERNET SOFTWARE PTY LTD\" \"{file}\"";

                    Console.WriteLine($"Executing SignTool command: {cmd}");
                    ExecuteTerminalCommand(cmd, Path.GetDirectoryName(file));
                }

                static void ExecuteTerminalCommand(string command, string? fileFolder)
                {
                    Console.WriteLine(">>>>>>>>>>");

                    var commandResult = AppUtils.ExecuteTerminalCommand(
                        command,
                        fileFolder,
                        waitResult: true,
                        logStdOut: true);

                    if (commandResult.ExitCode != 0)
                    {
                        Console.WriteLine($"Command failed with exit code: {commandResult.ExitCode}");
                    }

                    if (!string.IsNullOrEmpty(commandResult.Output))
                    {
                        Console.WriteLine($"{commandResult.Output}");
                    }

                    if (!string.IsNullOrEmpty(commandResult.Error))
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Command error output: \n{commandResult.Error}");
                        Console.WriteLine();
                    }

                    Console.WriteLine("<<<<<<<<<<");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            return extractToFolder;
        }

        public void Prepare(RezipCommandSettings globals)
        {
            RootFolder = ReplaceParams(RootFolder);

            if (!string.IsNullOrEmpty(RootFolder))
                RootFolder = Path.GetFullPath(RootFolder);
            else
                RootFolder = null;

            PathToZip = ReplaceParams(PathToZip);
            PathToResult = ReplaceParams(PathToResult);
            CommandToRun = ReplaceParams(CommandToRun);
            CommandArgs = ReplaceParams(CommandArgs);

            string? ReplaceParams(string? s)
            {
                if (s is null)
                    return null;

                var result = s;

                if (!string.IsNullOrEmpty(RootFolder))
                {
                    result = result.Replace("$(RootFolder)", RootFolder);
                }

                if (!string.IsNullOrEmpty(globals.SettingsFolder))
                {
                    result = result.Replace("$(SettingsFolder)", globals.SettingsFolder);
                }

                return result;
            }
        }
    }
}
