using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.UI;

namespace Alternet.UI
{
    public interface IProcessRunnerNotification : IDisposableObject
    {
        ObjectUniqueId UniqueId { get; }

        void OnRunningProcessStarted(Process process);

        void OnRunningProcessDisposed(Process process);

        void OnRunningProcessExited(Process process);

        void OnRunningProcessLog(Process process, string data, LogItemKind kind = LogItemKind.Information);
    }
}
