using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.UI
{
    public static class ProcessRunnerWithNotification
    {
        public static string? ErrorOutputPrefix = "Error> ";

        public static string? OutputPrefix = "Output> ";

        public static string? ProcessExitedSeparator = "-";

        public static string? ProcessExitedWithResult = "Process exited with result";

        private static readonly
            ConcurrentDictionary<ObjectUniqueId, WeakReferenceValue<IProcessRunnerNotification>>
            subscribers = new();

        private static Process? runningProcess;

        static ProcessRunnerWithNotification()
        {
        }

        public static event EventHandler? RunningProcessDisposed;

        public static event EventHandler? RunningProcessExited;

        public static Process? RunningProcess
        {
            get => runningProcess;
            set
            {
                if (runningProcess == value)
                    return;

                runningProcess = value;
            }
        }

        public static bool HasRunningProcess
        {
            get
            {
                return RunningProcess is not null && !RunningProcess.HasExited;
            }
        }

        public static void Unbind(IProcessRunnerNotification subscriber)
        {
            subscribers.TryRemove(subscriber.UniqueId, out _);
        }

        public static void Bind(IProcessRunnerNotification subscriber)
        {
            subscribers.TryAdd(subscriber.UniqueId, new(subscriber));
        }

        public static void KillRunningProcess()
        {
            if (RunningProcess != null)
            {
                if (!RunningProcess.HasExited)
                {
                    RunningProcess.Kill();
                }

                RunningProcess = null;
            }
        }

        public static int RunProcessOldStyle(ProcessStartInfo startInfo, bool wait)
        {
            var process = RunProcess(startInfo);

            if (wait)
            {
                process.WaitForExit();
                return process.ExitCode;
            }

            return 0;
        }

        public static Process RunProcess(ProcessStartInfo startInfo)
        {
            KillRunningProcess();

            Process Internal()
            {
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.StandardErrorEncoding = Encoding.UTF8;
                /*startInfo.StandardInputEncoding = Encoding.UTF8;*/
                startInfo.StandardOutputEncoding = Encoding.UTF8;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardError = true;
                if (!string.IsNullOrEmpty(startInfo.WorkingDirectory))
                {
                    startInfo.WorkingDirectory =
                        PathUtils.AddDirectorySeparatorChar(startInfo.WorkingDirectory);
                }

                var process = new Process();
                var outputComplete = false;
                var errorComplete = false;

                process.Disposed += (x, y) =>
                {
                    if (x == RunningProcess)
                        RunningProcess = null;
                    NotifyDisposed((Process)x);
                };

                process.Exited += (x, y) =>
                {
                    while (true)
                    {
                        if (outputComplete && errorComplete)
                            break;
                        Task.Delay(100).Wait();
                    }

                    process = x as Process;

                    if (process is null)
                        return;

                    if (x == RunningProcess)
                        RunningProcess = null;

                    NotifyExited((Process)x);

                    var exitCode = process.ExitCode;

                    if (ProcessExitedWithResult is not null)
                        LogString(process, $"{ProcessExitedWithResult}: {exitCode}");
                    LogString(process, ProcessExitedSeparator);
                };

                void LogString(Process process, string? s, LogItemKind kind = LogItemKind.Information)
                {
                    if (s is null)
                        return;
                    Notify((subscriber) => subscriber.OnRunningProcessLog(process, s, kind));
                }

                process.OutputDataReceived += (x, y) =>
                {
                    if (y.Data is null)
                    {
                        outputComplete = true;
                        return;
                    }

                    process = x as Process;

                    if (process != RunningProcess || process is null)
                        return;

                    if (string.IsNullOrWhiteSpace(y.Data))
                        return;
                    LogString(process, $"{OutputPrefix}{y.Data}", LogItemKind.Information);
                };

                process.ErrorDataReceived += (x, y) =>
                {
                    if (y.Data is null)
                    {
                        errorComplete = true;
                        return;
                    }

                    process = x as Process;

                    if (process != RunningProcess || process is null)
                        return;

                    if (string.IsNullOrWhiteSpace(y.Data))
                        return;
                    LogString(process, $"{ErrorOutputPrefix}{y.Data}", LogItemKind.Error);
                };

                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;

                RunningProcess = process;

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                Notify((subscriber) => subscriber.OnRunningProcessStarted(process));

                return process;
            }

            return Internal();
        }

        private static void Notify(Action<IProcessRunnerNotification> action)
        {
            BaseObject.Invoke(() =>
            {
                foreach (var subscriberRef in subscribers.Values)
                {
                    var subscriber = subscriberRef.Value;
                    if (subscriber is null)
                        continue;
                    action(subscriber);
                }
            });
        }

        private static void NotifyExited(Process process)
        {
            BaseObject.Invoke(() =>
            {
                RunningProcessExited?.Invoke(process, EventArgs.Empty);
                Notify((subscriber) => subscriber.OnRunningProcessExited(process));
            });
        }

        private static void NotifyDisposed(Process process)
        {
            BaseObject.Invoke(() =>
            {
                RunningProcessDisposed?.Invoke(process, EventArgs.Empty);
                Notify((subscriber) => subscriber.OnRunningProcessDisposed(process));
            });
        }
    }
}
