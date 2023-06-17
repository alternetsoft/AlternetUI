using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Application
    {
        public Application() => throw new Exception();

        public string Name { get; set; }

        public void Run(Window window) => throw new Exception();

        public event EventHandler Idle { add => throw new Exception(); remove => throw new Exception(); }

        public Keyboard Keyboard { get; }
        public Mouse Mouse { get; }
        public Clipboard Clipboard { get; }
        public string DisplayName { get; set; }
        public string AppClassName { get; set; }
        public string VendorName { get; set; }
        public string VendorDisplayName { get; set; }

        public bool InUixmlPreviewerMode { get; set; }

        public void WakeUpIdle() => throw new Exception();

        public void Exit() => throw new Exception();

        public bool InvokeRequired { get; }

        public static void SuppressDiagnostics(int flags) => throw new Exception();

        public void BeginInvoke([CallbackMarshal(freeAfterFirstCall: true)] Action action) => throw new Exception();
    }
}