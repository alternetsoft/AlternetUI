using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Alternet.Drawing;
using Alternet.UI.Integration.Remoting;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    class Test
    {
        public static void Test2()
        {
            //var appPath = @"C:\Users\yezo\source\repos\AlternetUIApplication3\AlternetUIApplication3\bin\Debug\net6.0\AlternetUIApplication3.dll";
            //var uixml = File.ReadAllText(@"C:\Users\yezo\source\repos\AlternetUIApplication3\AlternetUIApplication3\MainWindow.uixml");

            var appPath = @"C:\Work\UI\Source\Samples\HelloWorldSample\bin\Debug\net6.0\HelloWorldSample.dll";
            var uixml = File.ReadAllText(@"C:\Work\UI\Source\Samples\HelloWorldSample\MainWindow.uixml ");

            using var stream = new MemoryStream(Encoding.Default.GetBytes(uixml));

#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#endif

            EnsureApplicationCreated();

            var appAssembly = Assembly.LoadFrom(appPath);

            //var control = CreateControlForUixml(uixml, appAssembly);
            //new UixmlLoader().LoadExisting(stream, control);

            UixmlLoader.DisableComponentInitialization = true;

            var control = (Window)new UixmlLoader().Load(stream, appAssembly);
            var handle = GetHandle(control);

            //_c = control;

            //AttachEventHandler(application, "Idle", Application_Idle);

            //application.Run(control);

        }

        static void Log(string message) => File.AppendAllText(@"c:\temp\UIXMLPreviewerProcess.log", message + "\n");

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log(e.ExceptionObject.ToString());
        }

        public static void Test1()
        {
#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#endif
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            EnsureApplicationCreated();

            UixmlLoader.DisableComponentInitialization = true;
            SetUixmlPreviewerMode();

            AttachEventHandler(application, "Idle", Application_IdleTest);

            var timer = new Timer(TimeSpan.FromMilliseconds(100), OnTimerTick);

            new System.Threading.Timer(_ => WakeUpIdle(), null, 0, 200);
            new System.Threading.Timer(_ => testPulse = true, null, 0, 3000);

            application.Run(new Window { IsToolWindow = true, StartLocation = WindowStartLocation.Manual, Location = new Point(20000, 20000) });
        }

        static volatile bool testPulse;

        private static void Application_IdleTest(object sender, EventArgs e)
        {
            if (!testPulse)
                return;

            testPulse = false;

            var appAssembly = Assembly.LoadFrom(@"C:\Users\yezo\source\repos\AlternetUIApplication3\AlternetUIApplication3\bin\Debug\net6.0\AlternetUIApplication3.dll");
            using var stream = new MemoryStream(Encoding.Default.GetBytes(File.ReadAllText(@"C:\Users\yezo\source\repos\AlternetUIApplication3\AlternetUIApplication3\MainWindow.uixml")));

            var control = (Control)new UixmlLoader().Load(stream, appAssembly);

            //var control = new Window();

            Window window = null;

            if (control is Window w)
            {
                window = w;
                //window.ShowInTaskbar = false;
                //window.Resizable = false;
                window.IsToolWindow = true;
            }

            //currentWindow = window;
            window.Show();

            var timer = new Timer(TimeSpan.FromMilliseconds(500));

            timer.Tick += (o, e) =>
            {
                timer.Stop();
                timer.Dispose();

                //var targetDirectoryPath = Path.Combine(Path.GetTempPath(), "AlterNET.UIXMLPreviewer", "UIImages");
                //if (!Directory.Exists(targetDirectoryPath))
                //    Directory.CreateDirectory(targetDirectoryPath);

                //var targetFilePath = Path.Combine(targetDirectoryPath, Guid.NewGuid().ToString("N") + ".png");
                //SaveScreenshot(window, targetFilePath);

                window.Close();
                window.Dispose();
            };

            timer.Start();
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
            ProcessQueue();
        }

        static void AttachEventHandler(object obj, string eventName, EventHandler handler)
        {
            static Delegate ConvertDelegate(Delegate originalDelegate, Type targetDelegateType)
            {
                return Delegate.CreateDelegate(
                    targetDelegateType,
                    originalDelegate.Target,
                    originalDelegate.Method);
            }

            var eventInfo = obj.GetType().GetEvent(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
            var convertedHandler = ConvertDelegate(handler, eventInfo.EventHandlerType);
            var addMethod = eventInfo.GetAddMethod(true);
            addMethod.Invoke(obj, new[] { convertedHandler });
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            ProcessQueue();

            //if (!_saved)
            //{
            //    GetHandle(_c, @"c:\temp\1.png");
            //    _saved = true;
            //}
        }

        private static void ProcessQueue()
        {
            lock (actions)
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue()();
                }
            }
        }

        static MethodInfo getHandleMethod;
        static MethodInfo saveScreenshotMethod;

        static IntPtr GetHandle(Control control)
        {
            if (getHandleMethod == null)
                getHandleMethod = typeof(ControlHandler).GetMethod("GetHandle", BindingFlags.Instance | BindingFlags.NonPublic);

            return (IntPtr)getHandleMethod.Invoke(control.Handler, new object[0]);
        }

        static void SaveScreenshot(Control control, string fileName)
        {
            if (saveScreenshotMethod == null)
                saveScreenshotMethod = typeof(ControlHandler).GetMethod("SaveScreenshot", BindingFlags.Instance | BindingFlags.NonPublic);

            saveScreenshotMethod.Invoke(control.Handler, new object[] { fileName });
        }

        static void RunApplication()
        {
            var runMethod = typeof(Application).GetMethod("RunWithNoWindow", BindingFlags.Instance | BindingFlags.NonPublic);

            runMethod.Invoke(application, new object[0]);
        }

        static void SetUixmlPreviewerMode()
        {
            var property = typeof(Application).GetProperty("InUixmlPreviewerMode", BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(application, true);
        }

        static MethodInfo wakeUpIdleMethod;

        static void WakeUpIdle()
        {
            if (wakeUpIdleMethod == null)
                wakeUpIdleMethod = typeof(Application).GetMethod("WakeUpIdle", BindingFlags.Instance | BindingFlags.NonPublic);
            wakeUpIdleMethod.Invoke(application, new object[0]);
        }


        private static void SetParentOverrideHandle(IntPtr parentWindowHandle)
        {
            var property = typeof(Application).GetProperty("ParentOverrideHandle", BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(application, parentWindowHandle);
        }

        static Application application;

        private static void EnsureApplicationCreated()
        {
            if (application == null)
                application = new Application();
        }

        static string GetUixmlClassName(string uixml)
        {
            var document = XDocument.Parse(uixml);
            return document.Root.Attribute((XNamespace)"http://schemas.alternetsoft.com/ui/2021/uixml" + "Class").Value;
        }

        private static Control CreateControlForUixml(string uixml, Assembly asm)
        {
            var controlType = asm.GetType(GetUixmlClassName(uixml));
            var control = Activator.CreateInstance(controlType);

            return (Control)control;
        }

#if NETCOREAPP
        private static Assembly AssemblyLoadContext_Resolving(System.Runtime.Loader.AssemblyLoadContext context, AssemblyName name)
        {
            if (name.Name == "Alternet.UI")
                return typeof(Alternet.UI.Window).Assembly;
            
            if (name.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
            {
                // See https://github.com/dotnet/runtime/issues/7077
                return null;
            }

            return context.LoadFromAssemblyName(name);
        }
#endif

        static Queue<Action> actions = new Queue<Action>();

        static void BeginInvoke(Action action)
        {
            lock (actions)
                actions.Enqueue(action);
        }

        const int SRCCOPY = 0xCC0020;

        [DllImport("gdi32.dll")]
        static extern int BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, int rop);

        static void SaveWindowToBitmap(IntPtr hwnd, string targetFilePath)
        {
            PInvoke.User32.GetWindowRect(hwnd, out var rect);
            var size = new System.Drawing.Size(rect.right - rect.left, rect.bottom - rect.top);

            //            PInvoke.User32.SetWindowPos(hwnd, new IntPtr(1), 0, 0, size.Width, size.Height, PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);
            //            PInvoke.User32.UpdateWindow(hwnd);

            var bmp = new System.Drawing.Bitmap(size.Width, size.Height);
            using (var bmpGraphics = System.Drawing.Graphics.FromImage(bmp))
            {
                var bmpDC = bmpGraphics.GetHdc();
                using (var formGraphics = System.Drawing.Graphics.FromHwnd(hwnd))
                {
                    var formDC = formGraphics.GetHdc();
                    BitBlt(bmpDC, 0, 0, size.Width, size.Height, formDC, 0, 0, SRCCOPY);
                    formGraphics.ReleaseHdc(formDC);
                }

                bmpGraphics.ReleaseHdc(bmpDC);
            }

            //PInvoke.User32.SetWindowPos(hwnd, IntPtr.Zero, rect.left, rect.top, size.Width, size.Height, PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);

            bmp.Save(targetFilePath);
        }
    }
}
