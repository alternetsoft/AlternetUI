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
    /// <summary>
    /// Provides functionality to run processes with notifications for their lifecycle events.
    /// </summary>
    public static class ProcessRunnerWithNotification
    {
        /// <summary>
        /// Specifies the default encoding used for standard input, output, and error streams
        /// when running a process.
        /// </summary>
        /// <remarks>
        /// If this property is not set, <see cref="Encoding.UTF8"/> is used as the default encoding.
        /// This encoding is applied to the <see cref="ProcessStartInfo.StandardOutputEncoding"/>,
        /// <see cref="ProcessStartInfo.StandardErrorEncoding"/>, and optionally to the input stream
        /// if supported by the platform.
        /// </remarks>
        public static Encoding? DefaultInputOutputEncoding;

        /// <summary>
        /// Prefix for error output messages.
        /// </summary>
        public static string? ErrorOutputPrefix = "Error> ";

        /// <summary>
        /// Prefix for standard output messages.
        /// </summary>
        public static string? OutputPrefix = "Output> ";

        /// <summary>
        /// Separator message added to log when a process exits.
        /// </summary>
        public static string? ProcessExitedSeparator = "-";

        /// <summary>
        /// Message template for logging process exit results.
        /// </summary>
        public static string? ProcessExitedWithResult = "Process exited with result: {0}";

        private static readonly
            ConcurrentDictionary<ObjectUniqueId, WeakReferenceValue<IProcessRunnerNotification>> subscribers
            = new();

        private static Process? runningProcess;

        /// <summary>
        /// Initializes static members of the <see cref="ProcessRunnerWithNotification"/> class.
        /// </summary>
        static ProcessRunnerWithNotification()
        {
        }

        /// <summary>
        /// Occurs when the currently running process is disposed.
        /// </summary>
        public static event EventHandler? RunningProcessDisposed;

        /// <summary>
        /// Occurs when the currently running process exits.
        /// </summary>
        public static event EventHandler? RunningProcessExited;

        /// <summary>
        /// Gets or sets the currently running process.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether there is a running process that has not exited.
        /// </summary>
        public static bool HasRunningProcess
        {
            get
            {
                return RunningProcess is not null && !RunningProcess.HasExited;
            }
        }

        /// <summary>
        /// Unbinds a subscriber from receiving process notifications.
        /// </summary>
        /// <param name="subscriber">The subscriber to unbind.</param>
        public static void Unbind(IProcessRunnerNotification subscriber)
        {
            subscribers.TryRemove(subscriber.UniqueId, out _);
        }

        /// <summary>
        /// Binds a subscriber to receive process notifications.
        /// </summary>
        /// <param name="subscriber">The subscriber to bind.</param>
        public static void Bind(IProcessRunnerNotification subscriber)
        {
            subscribers.TryAdd(subscriber.UniqueId, new(subscriber));
        }

        /// <summary>
        /// Kills the currently running process, if any.
        /// </summary>
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

        /// <summary>
        /// Runs a process using the specified start information and optionally waits for it to exit.
        /// </summary>
        /// <param name="startInfo">The start information for the process.</param>
        /// <param name="wait">If <c>true</c>, waits for the process to exit before returning.</param>
        /// <returns>The exit code of the process if <paramref name="wait"/> is <c>true</c>;
        /// otherwise, 0.</returns>
        public static int RunProcessAndWait(ProcessStartInfo startInfo, bool wait)
        {
            var process = RunProcess(startInfo);

            if (wait)
            {
                process.WaitForExit();
                return process.ExitCode;
            }

            return 0;
        }

        /// <summary>
        /// Binds event handlers to the specified <see cref="Process"/> to handle its output,
        /// error, exit, and disposal events.
        /// Notifies all subscribers about process lifecycle events and logs output and error data.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> to bind events to.</param>
        public static void BindProcessEvents(Process process)
        {
            var outputComplete = false;
            var errorComplete = false;

            process.Disposed += (x, y) =>
            {
                if (x is not Process process)
                    return;
                if (x == RunningProcess)
                    RunningProcess = null;
                NotifyDisposed(process);
            };

            process.Exited += (x, y) =>
            {
                while (true)
                {
                    if (outputComplete && errorComplete)
                        break;
                    Task.Delay(100).Wait();
                }

                if (x is not Process process)
                    return;

                if (x == RunningProcess)
                    RunningProcess = null;

                NotifyExited(process);

                var exitCode = process.ExitCode;

                if (ProcessExitedWithResult is not null)
                    LogString(process, string.Format(ProcessExitedWithResult, exitCode));
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

                var process = x as Process;

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

                var process = x as Process;

                if (process != RunningProcess || process is null)
                    return;

                if (string.IsNullOrWhiteSpace(y.Data))
                    return;
                LogString(process, $"{ErrorOutputPrefix}{y.Data}", LogItemKind.Error);
            };
        }

        /// <summary>
        /// Runs a process using the specified start information.
        /// </summary>
        /// <param name="startInfo">The start information for the process.</param>
        /// <returns>The <see cref="Process"/> instance representing the running process.</returns>
        public static Process RunProcess(ProcessStartInfo startInfo)
        {
            KillRunningProcess();

            Process Internal()
            {
                var encoding = DefaultInputOutputEncoding ?? Encoding.UTF8;

                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.StandardErrorEncoding = encoding;

                var result = AssemblyUtils.TrySetMemberValue(
                    startInfo,
                    "StandardInputEncoding",
                    Encoding.UTF8);

                startInfo.StandardOutputEncoding = encoding;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardError = true;
                if (!string.IsNullOrEmpty(startInfo.WorkingDirectory))
                {
                    startInfo.WorkingDirectory
                        = PathUtils.AddDirectorySeparatorChar(startInfo.WorkingDirectory);
                }

                var process = new Process();

                BindProcessEvents(process);

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

        /// <summary>
        /// Notifies all subscribers of a specific action.
        /// </summary>
        /// <param name="action">The action to perform for each subscriber.</param>
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

        /// <summary>
        /// Notifies all subscribers that a process has exited.
        /// </summary>
        /// <param name="process">The process that has exited.</param>
        private static void NotifyExited(Process process)
        {
            BaseObject.Invoke(() =>
            {
                RunningProcessExited?.Invoke(process, EventArgs.Empty);
                Notify((subscriber) => subscriber.OnRunningProcessExited(process));
            });
        }

        /// <summary>
        /// Notifies all subscribers that a process has been disposed.
        /// </summary>
        /// <param name="process">The process that has been disposed.</param>
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