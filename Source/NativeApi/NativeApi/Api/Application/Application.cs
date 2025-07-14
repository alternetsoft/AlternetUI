#pragma warning disable
using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_app.html
    public class Application
    {
        public static void SetGtkCss(bool inject, string css) { }
        
        public PropertyUpdateResult SetAppearance(ApplicationAppearance appearance)
            => default;

        public static void GetEventIdentifiers(int[] eventIdentifiers) { }

        public string EventArgString { get; }

        public static void ThrowError(int value) { }

        public static void SetSystemOptionInt(string name, int value) { }

        public Application() => throw new Exception();

        public string Name { get; set; }

        public void Run(Window window) { }

        public event EventHandler Idle;
        public event EventHandler LogMessage;
        public event EventHandler QueryEndSession;
        public event EventHandler EndSession;
        public event EventHandler ActivateApp;
        public event EventHandler Hibernate;
        public event EventHandler DialupConnected;
        public event EventHandler DialupDisconnected;
        public event EventHandler ExceptionInMainLoop;
        public event EventHandler UnhandledException;
        public event EventHandler FatalException;
        public event EventHandler AssertFailure;

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

        public static void WakeUpIdle() { }

        public void Exit() { }

        public bool InvokeRequired { get; }

        public static void SuppressDiagnostics(int flags) { }

        public void BeginInvoke([CallbackMarshal(freeAfterFirstCall: true)] Action action) { }

        public void ProcessPendingEvents() { }

        public bool HasPendingEvents() => default;

        // Get display mode that is used use.
        public IntPtr GetDisplayMode() => default;

        // Returns true if the application will exit when the top-level frame is deleted.
        public bool GetExitOnFrameDelete() => default;

        // Return the layout direction for the current locale or wxLayout_Default if it's unknown.
        public int GetLayoutDirection() => default;

        // Returns true if the application will use the best visual on systems that support different
        // visuals, false otherwise. 
        public bool GetUseBestVisual() => default;

        // Returns true if the application is active, i.e. if one of its windows is currently in
        // the foreground. 
        public bool IsActive() => default;

        // This function is similar to wxYield(), except that it disables the user input to
        // all program windows before calling wxAppConsole::Yield and re-enables it again afterwards. 
        public bool SafeYield(IntPtr window, bool onlyIfNeeded) => default;

        // Works like SafeYield() with onlyIfNeeded == true except that it allows the caller
        // to specify a mask of events to be processed. 
        public bool SafeYieldFor(IntPtr window, long eventsToProcess) => default;

        // Set display mode to use. 
        public bool SetDisplayMode(IntPtr videoMode) => default;

        // Allows the programmer to specify whether the application will exit when the
        // top-level frame is deleted. 
        public void SetExitOnFrameDelete(bool flag) { }

        // Allows runtime switching of the UI environment theme.  
        public bool SetNativeTheme(string theme) => default;

        // Sets the 'top' window.  
        public void SetTopWindow(IntPtr window) { }

        // Allows the programmer to specify whether the application will use the best
        // visual on systems that support several visual on the same display.  
        public void SetUseBestVisual(bool flag, bool forceTrueColor = false) { }
    }
}