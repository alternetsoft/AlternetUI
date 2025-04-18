using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a contract for receiving notifications about the lifecycle and events of a running process.
    /// </summary>
    public interface IProcessRunnerNotification : IDisposableObject
    {
        /// <summary>
        /// Gets the unique identifier for this notification instance.
        /// </summary>
        ObjectUniqueId UniqueId { get; }

        /// <summary>
        /// Called when a process starts running.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> that has started.</param>
        void OnRunningProcessStarted(Process process);

        /// <summary>
        /// Called when a process is disposed.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> that has been disposed.</param>
        void OnRunningProcessDisposed(Process process);

        /// <summary>
        /// Called when a process exits.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> that has exited.</param>
        void OnRunningProcessExited(Process process);

        /// <summary>
        /// Called when a log entry is generated for a running process.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> associated with the log entry.</param>
        /// <param name="data">The log message.</param>
        /// <param name="kind">The kind of log entry, such as information, warning, or error.
        /// Defaults to <see cref="LogItemKind.Information"/>.</param>
        void OnRunningProcessLog(Process process, string data, LogItemKind kind = LogItemKind.Information);
    }
}