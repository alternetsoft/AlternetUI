using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// General application utility methods.
    /// </summary>
    public static class AppUtils
    {
        /// <summary>
        /// Gets or sets a function that opens a URL in the default browser.
        /// This function can be used to override the default behavior
        /// and is called when <see cref="OpenUrl(string?)"/> is invoked.
        /// </summary>
        public static Func<string?, bool>? OpenUrlOverride;

        private static NetFrameworkIdentifier? frameworkIdentifier;

        /// <summary>
        /// Gets first child of the first window or an empty control.
        /// </summary>
        public static AbstractControl FirstWindowChildOrEmpty =>
            App.FirstWindow()?.FirstChild ?? ControlUtils.Empty;

        /// <summary>
        /// Gets identifier for the net framework used in the application.
        /// </summary>
        public static NetFrameworkIdentifier FrameworkIdentifier
        {
            get
            {
                if (frameworkIdentifier is not null)
                    return frameworkIdentifier.Value;

                var description = RuntimeInformation.FrameworkDescription;

                if (description.StartsWith(".NET Core"))
                {
                    frameworkIdentifier = NetFrameworkIdentifier.NetCore;
                    return NetFrameworkIdentifier.NetCore;
                }

                if (description.StartsWith(".NET Framework"))
                {
                    frameworkIdentifier = NetFrameworkIdentifier.NetFramework;
                    return NetFrameworkIdentifier.NetFramework;
                }

                if (description.StartsWith(".NET Native"))
                {
                    frameworkIdentifier = NetFrameworkIdentifier.NetNative;
                    return NetFrameworkIdentifier.NetNative;
                }

                if (description.StartsWith(".NET"))
                {
                    frameworkIdentifier = NetFrameworkIdentifier.Net;
                    return NetFrameworkIdentifier.Net;
                }

                frameworkIdentifier = NetFrameworkIdentifier.Other;
                return NetFrameworkIdentifier.Other;
            }
        }

        /// <summary>
        /// Creates clone of the first window and optionally shows it on the screen.
        /// </summary>
        /// <param name="show">Whether to show created window. Optional.
        /// Default is <c>true</c>.</param>
        /// <returns></returns>
        public static Window CreateFirstWindowClone(bool show = true)
        {
            var type = App.FirstWindow()?.GetType() ?? typeof(Window);
            var instance = (Window?)Activator.CreateInstance(type) ?? new Window();
            if (show)
                instance.Show();
            return instance;
        }

        /// <summary>
        /// Creates clone of the main window and optionally shows it on the screen.
        /// </summary>
        /// <param name="show">Whether to show created window. Optional.
        /// Default is <c>true</c>.</param>
        /// <returns></returns>
        public static Window CreateMainWindowClone(bool show = true)
        {
            var type = App.MainWindow?.GetType() ?? typeof(Window);
            var instance = (Window?)Activator.CreateInstance(type) ?? new Window();
            if (show)
                instance.Show();
            return instance;
        }

        /// <summary>
        /// Gets library version as string.
        /// </summary>
        /// <returns></returns>
        public static string? GetUIVersion()
        {
            Assembly thisAssembly = typeof(App).Assembly;
            AssemblyName thisAssemblyName = thisAssembly.GetName();
            Version? ver = thisAssemblyName?.Version;
            return ver?.ToString();
        }

        /// <summary>
        /// Opens log file <see cref="App.LogFilePath"/> in the default editor
        /// of the operating system.
        /// </summary>
        public static void OpenLogFile()
        {
            if (!File.Exists(App.LogFilePath))
                LogUtils.LogAppStartedToFile();

            ShellExecute(App.LogFilePath);
        }

        /// <summary>
        /// Repeats <paramref name="action"/> specified number of <paramref name="times"/>.
        /// </summary>
        /// <param name="times">Repeat count.</param>
        /// <param name="action">Action to repeat</param>
        public static void RepeatAction(int times, Action action)
        {
            for (int i = 0; i < times; i++)
                action();
        }

        /// <summary>
        /// Gets this application target framework in the form like 'net8.0' or 'net462'.
        /// </summary>
        /// <returns></returns>
        public static string GetMyTargetFrameworkName()
        {
            var version = Environment.Version;

            string result;

            if (FrameworkIdentifier == NetFrameworkIdentifier.NetFramework)
                result = $"net{version.Major}{version.Minor}";
            else
                result = $"net{version.Major}.{version.Minor}";
            return result;
        }

        /// <summary>
        /// Executes terminal command redirecting output and error streams
        /// to <see cref="App.Log"/>.
        /// </summary>
        /// <param name="waitResult">Whether to wait until command finishes its execution.</param>
        /// <param name="command">Terminal command.</param>
        /// <param name="logStdOut">Specifies whether to hook and log
        /// standard output and standard error streams.</param>
        /// <param name="folder">Value of <see cref="ProcessStartInfo.WorkingDirectory"/>.</param>
        /// <returns>
        /// Result of the command execution in case when <paramref name="waitResult"/> is <c>true</c>;
        /// otherwise <c>null</c>.
        /// </returns>
        public static ExecuteTerminalCommandResult ExecuteTerminalCommand(
            string command,
            string? folder = null,
            bool waitResult = false,
            bool logStdOut = true)
        {
            if (App.IsWindowsOS)
            {
                return ExecuteApp(
                    "cmd.exe",
                    @"/c """ + command + @"""",
                    folder,
                    waitResult,
                    logStdOut);
            }
            else
            {
                return ExecuteApp(
                    "/bin/bash",
                    @"-c """ + command + @"""",
                    folder,
                    waitResult,
                    logStdOut);
            }
        }

        /// <summary>
        /// Opens terminal and runs 'echo' command with the specified text string there.
        /// </summary>
        /// <param name="s">String to output.</param>
        public static bool OpenTerminalAndRunEcho(string s)
        {
            var command = $"echo \"{s}\"";

            if (App.IsWindowsOS)
            {
                return OpenTerminalAndRunCommand(command);
            }
            else
            {
                return OpenTerminalAndRunCommand(command);
            }
        }

        /// <summary>
        /// Opens the specified folder in the system's default file explorer application.
        /// </summary>
        /// <remarks>This method supports Windows, macOS, and Linux platforms. On unsupported platforms or
        /// if an error occurs, the method returns false. No exception is thrown if the operation fails.</remarks>
        /// <param name="path">The full path of the folder to open. Must refer to an existing directory.</param>
        /// <returns>true if the folder was successfully opened in the file explorer; otherwise, false.</returns>
        public static bool OpenFolderInExplorer(string path)
        {
            try
            {
                var p = $"\"{path}\"";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start("explorer.exe", p);
                    return true;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", p);
                    return true;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", p);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening folder in explorer: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Opens terminal and runs command there.
        /// </summary>
        /// <param name="command">Command to run.</param>
        /// <param name="folder">Value of <see cref="ProcessStartInfo.WorkingDirectory"/>.</param>
        /// <returns></returns>
        public static bool OpenTerminalAndRunCommand(
            string? command = default,
            string? folder = default)
        {
            folder ??= PathUtils.GetAppFolder();

            if (App.IsWindowsOS)
            {
                if (command is not null)
                    command = "/k" + command;
                return ShellExecute("cmd.exe", command, folder);
            }
            else
            {
                if (command is not null)
                    command = "-c \"" + command + "\"";
                return ShellExecute("/bin/bash", command, folder);
            }
        }

        /// <summary>
        /// Executes application with the specified parameters.
        /// </summary>
        public static ExecuteTerminalCommandResult ExecuteApp(
            string fileName,
            string arguments,
            string? folder = null,
            bool waitResult = false,
            bool logStdOut = true)
        {
            string? errorData = null;
            string? outputData = null;

            Process process = new();
            if (logStdOut)
                App.IdleLog("Run: " + fileName + " " + arguments);
            ProcessStartInfo processInfo = new(fileName, arguments)
            {
                // If the UseShellExecute property is true,
                // the CreateNoWindow property value is ignored
                // and a new window is created.
                // .NET Core does not support creating windows directly
                // on Unix/Linux/macOS and the property is ignored.
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
            };
            if (folder != null)
                processInfo.WorkingDirectory = folder;
            process.StartInfo = processInfo;
            process.OutputDataReceived += (x, y) =>
            {
                outputData += y.Data + Environment.NewLine;
                if (string.IsNullOrWhiteSpace(y.Data))
                    return;
                if (logStdOut)
                {
                    App.IdleLog($"Output> {y.Data}");
                }
            };
            process.ErrorDataReceived += (x, y) =>
            {
                errorData += y.Data + Environment.NewLine;
                if (string.IsNullOrWhiteSpace(y.Data))
                    return;
                if (logStdOut)
                {
                    App.IdleLog($"Error> {y.Data}");
                }
            };
            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (waitResult)
            {
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                // string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return new(outputData?.TrimEndEol(), errorData?.TrimEndEol(), process.ExitCode);
            }
            else
            {
                return new(null, null, 0);
            }
        }

        /// <summary>
        /// Uses <see cref="Process"/> to start the application.
        /// </summary>
        /// <param name="filePath">Path to the application.</param>
        /// <param name="args">Set of command-line arguments to use when starting the application.
        /// See <see cref="ProcessStartInfo.Arguments"/></param>
        /// <param name="folder">Initial directory.
        /// See <see cref="ProcessStartInfo.WorkingDirectory"/></param>
        /// <returns><c>true</c> if operation is successful; <c> false</c> otherwise.</returns>
        public static bool ProcessStart(
            string filePath,
            string? args = null,
            string? folder = null)
        {
            using Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            if (args is not null)
                process.StartInfo.Arguments = args;
            if (folder != null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Opens url in the default browser.
        /// </summary>
        /// <param name="url">Url to open.</param>
        /// <returns><c>true</c> if operation is successful; <c> false</c> otherwise.</returns>
        public static bool OpenUrl(string? url)
        {
            if(OpenUrlOverride is not null)
            {
                return OpenUrlOverride(url);
            }

            var result = ShellExecute(url);
            return result;
        }

        /// <remarks>
        /// Uses shell execute to start the process.
        /// </remarks>
        /// <param name="filePath">Path to the application.</param>
        /// <param name="args">Set of command-line arguments to use
        /// when starting the application.</param>
        /// <param name="folder">Initial directory.</param>
        public static bool ShellExecute(string? filePath, string? args = null, string? folder = null)
        {
            if(filePath == null)
                return false;

            using Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = true;
            if (args is not null)
                process.StartInfo.Arguments = args;
            if (folder != null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Uses <see cref="Process"/> to start the application.
        /// </summary>
        /// <param name="startInfo">Process start parameters.</param>
        /// <returns></returns>
        public static bool ProcessStartEx(ProcessStartInfo startInfo)
        {
            using Process process = new();
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Splits command line string into array and adds double quotes to the elements
        /// if they contain spaces.
        /// </summary>
        /// <param name="cmdLine">Command line string.</param>
        /// <returns></returns>
        public static string[] SegmentAndQuoteCommandLine(string? cmdLine)
        {
            var result = SegmentCommandLine(cmdLine);

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Contains(StringUtils.OneSpace))
                {
                    result[i] = StringUtils.PrefixSuffix(result[i], StringUtils.OneDoubleQuote);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the application's appearance to match the system appearance
        /// if the application is running in debug mode and a debugger is attached.
        /// Also sets the appearance to dark mode if the command line argument
        /// is specified to enable dark mode.
        /// </summary>
        /// <remarks>This method applies the system appearance only when the application
        /// is running on Windows 11 or later and a debugger is attached.
        /// It has no effect if these conditions are not met.</remarks>
        public static PropertyUpdateResult SetSystemAppearanceIfDebug()
        {
            if (App.IsWindows11AtLeast)
            {
                if (CommandLineArgs.ParseAndGetIsDark())
                {
                    var appearanceResult = App.SetAppearance(ApplicationAppearance.Dark);
                    return appearanceResult;
                }

                if (DebugUtils.IsDebugDefinedAndAttached)
                {
                    var appearanceResult = App.SetAppearance(ApplicationAppearance.System);
                    return appearanceResult;
                }
            }

            return PropertyUpdateResult.Failure;
        }

        /// <summary>
        /// Splits command line string into array.
        /// </summary>
        /// <param name="cmdLine">Command line string.</param>
        /// <param name="useSplit">Specifies whether to use simple string.Split.</param>
        /// <returns></returns>
        public static string[] SegmentCommandLine(string? cmdLine, bool useSplit = false)
        {
            if (cmdLine is null)
                return Array.Empty<string>();

            var s = cmdLine.Trim();

            if (useSplit)
                return cmdLine.Split(' ');
            else
            {
                unsafe
                {
                    fixed (char* p = s)
                    {
                        return SegmentCommandLineChar(p);
                    }
                }
            }
        }

        private static unsafe string[] SegmentCommandLineChar(char* cmdLine)
        {
            ArrayBuilder<string> arrayBuilder = default;
            Span<char> initialBuffer = stackalloc char[260];
            char* ptr = cmdLine;
            bool flag = false;
            ValueStringBuilder valueStringBuilder = new(initialBuffer);
            char c;
            bool flag3;
            do
            {
                if (*ptr == '"')
                {
                    flag = !flag;
                    c = *(ptr++);
                }
                else
                {
                    c = *(ptr++);
                    valueStringBuilder.Append(c);
                }

                bool flag2 = c != '\0';
                flag3 = flag2;
                if (flag3)
                {
                    bool flag4 = flag;
                    bool flag5 = flag4;
                    if (!flag5)
                    {
                        bool flag6 = c == '\t' || c == ' ';
                        flag5 = !flag6;
                    }

                    flag3 = flag5;
                }
            }
            while (flag3);
            if (c == '\0')
            {
                ptr--;
            }

            valueStringBuilder.Length--;
            arrayBuilder.Add(valueStringBuilder.ToString());
            flag = false;
            while (true)
            {
                if (*ptr != 0)
                {
                    while (true)
                    {
                        char c2 = *ptr;
                        if ((c2 != '\t' && c2 != ' ') || 1 == 0)
                        {
                            break;
                        }

                        ptr++;
                    }
                }

                if (*ptr == '\0')
                {
                    break;
                }

                valueStringBuilder = new ValueStringBuilder(initialBuffer);
                while (true)
                {
                    bool flag7 = true;
                    int num = 0;
                    while (*ptr == '\\')
                    {
                        ptr++;
                        num++;
                    }

                    if (*ptr == '"')
                    {
                        if (num % 2 == 0)
                        {
                            if (flag && ptr[1] == '"')
                            {
                                ptr++;
                            }
                            else
                            {
                                flag7 = false;
                                flag = !flag;
                            }
                        }

                        num /= 2;
                    }

                    while (num-- > 0)
                    {
                        valueStringBuilder.Append('\\');
                    }

                    bool flag8 = *ptr == '\0';
                    bool flag9 = flag8;
                    if (!flag9)
                    {
                        bool flag10 = !flag;
                        bool flag11 = flag10;
                        if (flag11)
                        {
                            char c2 = *ptr;
                            bool flag6 = c2 == '\t' || c2 == ' ';
                            flag11 = flag6;
                        }

                        flag9 = flag11;
                    }

                    if (flag9)
                    {
                        break;
                    }

                    if (flag7)
                    {
                        valueStringBuilder.Append(*ptr);
                    }

                    ptr++;
                }

                arrayBuilder.Add(valueStringBuilder.ToString());
            }

            return arrayBuilder.ToArray();
        }

        /// <summary>
        /// Represents the result of executing a terminal command,
        /// including output, error messages, and the exit code.
        /// </summary>
        public struct ExecuteTerminalCommandResult
        {
            /// <summary>
            /// The standard output from the terminal command.
            /// </summary>
            public string? Output;

            /// <summary>
            /// The error messages, if any, from the terminal command.
            /// </summary>
            public string? Error;

            /// <summary>
            /// The exit code returned by the terminal command, indicating its success or failure.
            /// </summary>
            public int ExitCode;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExecuteTerminalCommandResult"/>
            /// struct with the specified output, error messages, and exit code.
            /// </summary>
            /// <param name="output">The standard output from the terminal command.</param>
            /// <param name="error">The error messages, if any, from the terminal command.</param>
            /// <param name="exitCode">The exit code returned by the terminal command.</param>
            public ExecuteTerminalCommandResult(string? output, string? error, int exitCode)
            {
                Output = output;
                Error = error;
                ExitCode = exitCode;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ExecuteTerminalCommandResult"/> struct.
            /// </summary>
            public ExecuteTerminalCommandResult()
            {
            }
        }
    }
}
