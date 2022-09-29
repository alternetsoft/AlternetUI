using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI.Integration
{
    internal class UIXmlPreviewerService
    {
        private readonly Action<IDictionary<string, object>> onUixmlUpdateSuccess;
        private readonly Action<IDictionary<string, object>> onUixmlUpdateFailure;
        private readonly Action onTick;
        private readonly string screenshotsDirectory;

        private Application? application;
        private Timer? queueTimer;
        private System.Threading.Timer? wakeUpIdleTimer;

        public UIXmlPreviewerService(
            Action<IDictionary<string, object>> onUixmlUpdateSuccess,
            Action<IDictionary<string, object>> onUixmlUpdateFailure,
            Action onTick,
            string screenshotsDirectory)
        {
            this.onUixmlUpdateSuccess = onUixmlUpdateSuccess;
            this.onUixmlUpdateFailure = onUixmlUpdateFailure;
            this.onTick = onTick;
            this.screenshotsDirectory = screenshotsDirectory;
        }

        public void Run()
        {
#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#else
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
#endif

            if (application != null)
                throw new InvalidOperationException();

            application = new Application();

            UixmlLoader.DisableComponentInitialization = true;
            application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            application.InUixmlPreviewerMode = true;

            application.Idle += Application_Idle;

            queueTimer = new Timer(TimeSpan.FromMilliseconds(100), OnQueueTimerTick);

            wakeUpIdleTimer = new System.Threading.Timer(_ => application.WakeUpIdle(), null, 0, 200);

            application.Run(new Window { IsToolWindow = true, StartLocation = WindowStartLocation.Manual, Location = new Point(20000, 20000) });
        }

        public void ProcessUixmlUpdate(IDictionary<string, object> parameters)
        {
            try
            {
                var control = LoadControlFromUixml(parameters);
                var window = GetUixmlControlWindow(control);

                window.Show();

                var timer = new Timer(TimeSpan.FromMilliseconds(10));

                timer.Tick += (o, e) =>
                {
                    MoveWindowToScreenUnderneath(
                        window,
                        new System.Drawing.Point((int)parameters["OwnerWindowX"], (int)parameters["OwnerWindowY"]));

                    timer.Stop();
                    timer.Dispose();

                    var screenshotFileName = SaveWindowScreenshotToFile(window);

                    window.Close();
                    window.Dispose();

                    onUixmlUpdateSuccess(new Dictionary<string, object> { { "ImageFileName", screenshotFileName } });
                };

                timer.Start();
            }
            catch (Exception e)
            {
                onUixmlUpdateFailure(
                    new Dictionary<string, object>
                    {
                        { "Message", "Error while updating UIXML preview." },
                        { "Exception", e }
                    });
            }
        }

        private void OnQueueTimerTick(object? sender, EventArgs e)
        {
            onTick();
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            onTick();
        }

        private string SaveWindowScreenshotToFile(Window window)
        {
            var targetFilePath = Path.Combine(screenshotsDirectory, Guid.NewGuid().ToString("N") + ".bmp");
            window.Handler.SaveScreenshot(targetFilePath);
            return targetFilePath;
        }

        private void MoveWindowToScreenUnderneath(Window window, System.Drawing.Point ownerWindowLocation)
        {
            var hwnd = window.Handler.GetHandle();
            PInvoke.User32.GetWindowRect(hwnd, out var rect);

            var size = new System.Drawing.Size(rect.right - rect.left, rect.bottom - rect.top);

            PInvoke.User32.SetWindowPos(
                hwnd,
                new IntPtr(1),
                Math.Max(0, ownerWindowLocation.X),
                Math.Max(0, ownerWindowLocation.Y),
                size.Width,
                size.Height,
                PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);

            PInvoke.User32.UpdateWindow(hwnd);
        }

        private Control LoadControlFromUixml(IDictionary<string, object> parameters)
        {
            var appAssembly = Assembly.Load(File.ReadAllBytes((string)parameters["AssemblyPath"])); // Load bytes to avoid locking the file.
            Control control;
            using (var stream = new MemoryStream(Encoding.Default.GetBytes((string)parameters["Uixml"])))
            {
                control = (Control)new UixmlLoader().Load(stream, appAssembly);
            }

            return control;
        }

        private Window GetUixmlControlWindow(Control control)
        {
            Window window;
            if (control is Window w)
                window = w;
            else
            {
                window = new Window();
                window.Children.Add(control);
            }

            SetOffscreenToolWindowProperties(window);
            return window;
        }

        private void SetOffscreenToolWindowProperties(Window window)
        {
            window.IsToolWindow = true;
            window.StartLocation = WindowStartLocation.Manual;
            window.Location = new Point(20000, 20000);
        }

        public class UixmlUpdateFailure
        {
            public UixmlUpdateFailure(string message, Exception exception)
            {
                Message = message;
                Exception = exception;
            }

            public string Message { get; }

            public Exception Exception { get; }
        }

#if NETCOREAPP

        private Assembly? AssemblyLoadContext_Resolving(System.Runtime.Loader.AssemblyLoadContext context, AssemblyName name)
        {
            try
            {
                // you need to unsubscribe here to avoid StackOverflowException,
                // as LoadFromAssemblyName will go in recursion here otherwise
                System.Runtime.Loader.AssemblyLoadContext.Default.Resolving -= AssemblyLoadContext_Resolving;

                if (name.Name == "Alternet.UI")
                    return typeof(Alternet.UI.Window).Assembly;

                if (name.Name == null)
                    throw new Exception();

                if (name.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                {
                    // See https://github.com/dotnet/runtime/issues/7077
                    return null;
                }

                var assemblyFromDirectory = TryLoadAssemblyFromApplicationDirectory(name);
                if (assemblyFromDirectory != null)
                    return assemblyFromDirectory;

                return context.LoadFromAssemblyName(name);
            }
            finally
            {
                // don't forget to restore our load handler
                System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
            }
        }
#else
        private Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return TryLoadAssemblyFromApplicationDirectory(new AssemblyName(args.Name));
        }
#endif

        Assembly? TryLoadAssemblyFromApplicationDirectory(AssemblyName name)
        {
            var directory = Path.GetDirectoryName(GetType().Assembly.Location) ?? throw new Exception();
            var file = Path.Combine(directory, name.Name + ".dll");
            if (!File.Exists(file))
                return null;

            return Assembly.LoadFile(file);
        }

        private class PInvoke
        {
            public class User32
            {
                [Flags]
                public enum SetWindowPosFlags : uint
                {
                    SWP_ASYNCWINDOWPOS = 0x4000,
                    SWP_DEFERERASE = 0x2000,
                    SWP_DRAWFRAME = 0x0020,
                    SWP_FRAMECHANGED = 0x0020,
                    SWP_HIDEWINDOW = 0x0080,
                    SWP_NOACTIVATE = 0x0010,
                    SWP_NOCOPYBITS = 0x0100,
                    SWP_NOMOVE = 0x0002,
                    SWP_NOOWNERZORDER = 0x0200,
                    SWP_NOREDRAW = 0x0008,
                    SWP_NOREPOSITION = 0x0200,
                    SWP_NOSENDCHANGING = 0x0400,
                    SWP_NOSIZE = 0x0001,
                    SWP_NOZORDER = 0x0004,
                    SWP_SHOWWINDOW = 0x0040,
                }

                [DllImport(nameof(User32), SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

                [DllImport(nameof(User32), SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool SetWindowPos(
                    IntPtr hWnd,
                    IntPtr hWndInsertAfter,
                    int X,
                    int Y,
                    int cx,
                    int cy,
                    SetWindowPosFlags uFlags);

                [DllImport(nameof(User32), SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool UpdateWindow(IntPtr hWnd);

                public struct RECT
                {
                    public int left;
                    public int top;
                    public int right;
                    public int bottom;
                }
            }
        }
    }
}