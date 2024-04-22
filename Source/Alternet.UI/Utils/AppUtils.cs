using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// Opens log file <see cref="BaseApplication.LogFilePath"/> in the default editor
        /// of the operating system.
        /// </summary>
        public static void OpenLogFile()
        {
            if (!File.Exists(Application.LogFilePath))
                LogUtils.LogToFileAppStarted();

            AppUtils.ShellExecute(Application.LogFilePath);
        }

        /// <summary>
        /// Logs environment versions.
        /// </summary>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogVersion()
        {
            if (!LogUtils.ShowDebugWelcomeMessage)
                return;
            if (LogUtils.Flags.HasFlag(LogUtils.LogFlags.VersionLogged))
                return;
            LogUtils.Flags |= LogUtils.LogFlags.VersionLogged;
            var wxWidgets = WebBrowser.GetLibraryVersionString();
            var bitsOS = BaseApplication.Is64BitOS ? "x64" : "x86";
            var bitsApp = BaseApplication.Is64BitProcess ? "x64" : "x86";
            var net = $"Net: {Environment.Version}, OS: {bitsOS}, App: {bitsApp}";
            var dpi = $"DPI: {Application.FirstWindow()?.GetDPI().Width}";
            var ui = $"UI: {WebBrowser.DoCommandGlobal("UIVersion")}";
            var counterStr = $"Counter: {Application.BuildCounter}";
            var s = $"{ui}, {net}, {wxWidgets}, {dpi}, {counterStr}";
            BaseApplication.Log(s);
            if (BaseApplication.LogFileIsEnabled)
                BaseApplication.DebugLog($"Log File = {BaseApplication.LogFilePath}");
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
#if NETFRAMEWORK
            var result = $"net{version.Major}{version.Minor}";
#else
            var result = $"net{version.Major}.{version.Minor}";
#endif
            return result;
        }

        /// <summary>
        /// Executes terminal command redirecting output and error streams
        /// to <see cref="BaseApplication.Log"/>.
        /// </summary>
        /// <param name="waitResult">Whether to wait until command finishes its execution.</param>
        /// <param name="command">Terminal command.</param>
        /// <param name="logStdOut">Specifies whether to hook and log stdout and stderr.</param>
        /// <param name="folder">Value of <see cref="ProcessStartInfo.WorkingDirectory"/>.</param>
        /// <returns>
        /// Result of the command execution in case when <paramref name="waitResult"/> is <c>true</c>;
        /// otherwise <c>null</c>.
        /// </returns>
        public static (string? Output, string? Error, int ExitCode) ExecuteTerminalCommand(
            string command,
            string? folder = null,
            bool waitResult = false,
            bool logStdOut = true)
        {
            if (Application.IsWindowsOS)
                return ExecuteApp("cmd.exe", "/c " + command, folder, waitResult, logStdOut);
            else
                return ExecuteApp("/bin/bash", "-c \"" + command + "\"", folder, waitResult, logStdOut);
        }

        /// <summary>
        /// Executes application with the specified parameters.
        /// </summary>
        public static (string? Output, string? Error, int ExitCode) ExecuteApp(
            string fileName,
            string arguments,
            string? folder = null,
            bool waitResult = false,
            bool logStdOut = true)
        {
            string? errorData = null;
            string? outputData = null;

            Process process = new();
            if(logStdOut)
                Application.IdleLog("Run: " + fileName + " " + arguments);
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
                    Application.IdleLog($"Output> {y.Data}");
                }
            };
            process.ErrorDataReceived += (x, y) =>
            {
                errorData += y.Data + Environment.NewLine;
                if (string.IsNullOrWhiteSpace(y.Data))
                    return;
                if (logStdOut)
                {
                    Application.IdleLog($"Error> {y.Data}");
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
                return (outputData?.TrimEndEol(), errorData?.TrimEndEol(), process.ExitCode);
            }
            else
            {
                return (null, null, 0);
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
        public static bool OpenUrl(string url)
        {
            var result = ShellExecute(url);
            return result;
        }

        /// <inheritdoc cref="ProcessStart"/>
        /// <remarks>
        /// Uses shell execute to start the process.
        /// </remarks>
        public static bool ShellExecute(string filePath, string? args = null, string? folder = null)
        {
            using Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = true;
            if(args is not null)
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

            for(int i = 0; i < result.Length; i++)
            {
                if (result[i].Contains(StringUtils.OneSpace))
                {
                    result[i] = StringUtils.PrefixSuffix(result[i], StringUtils.OneDoubleQuote);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits command line string into array.
        /// </summary>
        /// <param name="cmdLine">Command line string.</param>
        /// <returns></returns>
        public static string[] SegmentCommandLine(string? cmdLine)
        {
            if (cmdLine is null)
                return Array.Empty<string>();

            var s = cmdLine.Trim();

#if NET6_0_OR_GREATER
            unsafe
            {
                fixed (char* p = s)
                {
                    return SegmentCommandLineChar(p);
                }
            }
#else
            return cmdLine.Split(' ');
#endif
        }

#if NET6_0_OR_GREATER
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
                            bool flag6 = (c2 == '\t' || c2 == ' ') ? true : false;
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
#endif
    }
}
