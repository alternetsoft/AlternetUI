#pragma warning disable
using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Application
    {
        public static void ThrowError(int value) { }

        public static void SetSystemOptionInt(string name, int value) {}

        public Application() => throw new Exception();

        public string Name { get; set; }

        public void Run(Window window) { }

        public event EventHandler Idle;

        public Keyboard Keyboard { get; }
        public Mouse Mouse { get; }
        public Clipboard Clipboard { get; }
        public string DisplayName { get; set; }
        public string AppClassName { get; set; }
        public string VendorName { get; set; }
        public string VendorDisplayName { get; set; }

        public bool InUixmlPreviewerMode { get; set; }

        public IntPtr GetTopWindow() => default;

        public void ExitMainLoop() { }

        public void WakeUpIdle() { }

        public void Exit() { }

        public bool InvokeRequired { get; }

        public static void SuppressDiagnostics(int flags) { }

        public void BeginInvoke([CallbackMarshal(freeAfterFirstCall: true)] Action action) { }

        public void ProcessPendingEvents() { }

        public bool HasPendingEvents() => default;

    }
}