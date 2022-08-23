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
    public class RemoteDesignerEntryPoint
    {
        private static ClientSupportedPixelFormatsMessage s_supportedPixelFormats;
        private static ClientViewportAllocatedMessage s_viewportAllocatedMessage;
        private static ClientRenderInfoMessage s_renderInfoMessage;

        private static IAlternetUIRemoteTransportConnection s_transport;
        class CommandLineArgs
        {
            public string AppPath { get; set; }
            public Uri Transport { get; set; }
            public Uri HtmlMethodListenUri { get; set; }
            public string Method { get; set; } = Methods.AlternetUIRemote;
            public string SessionId { get; set; } = Guid.NewGuid().ToString();
        }

        internal static class Methods
        {
            public const string AlternetUIRemote = "alternet-ui-remote";
            public const string Win32 = "win32";
            public const string Html = "html";
        }

        static Exception Die(string error)
        {
            if (error != null)
            {
                Console.Error.WriteLine(error);
                Console.Error.Flush();
            }
            Environment.Exit(1);
            return new Exception("APPEXIT");
        }

        static void Log(string message) => File.AppendAllText(@"c:\temp\UIXMLPreviewerProcess.log", message + "\n");

        static Exception PrintUsage()
        {
            Console.Error.WriteLine("Usage: --transport transport_spec --session-id sid --method method app");
            Console.Error.WriteLine();
            Console.Error.WriteLine("--transport: transport used for communication with the IDE");
            Console.Error.WriteLine("    'tcp-bson' (e. g. 'tcp-bson://127.0.0.1:30243/') - TCP-based transport with BSON serialization of messages defined in AlternetUI.Remote.Protocol");
            Console.Error.WriteLine("    'file' (e. g. 'file://C://my/file.xaml' - pseudo-transport that triggers XAML updates on file changes, useful as a standalone previewer tool, always uses http preview method");
            Console.Error.WriteLine();
            Console.Error.WriteLine("--session-id: session id to be sent to IDE process");
            Console.Error.WriteLine();
            Console.Error.WriteLine("--method: the way the XAML is displayed");
            Console.Error.WriteLine("    'alternet-ui-remote' - binary image is sent via transport connection in FrameMessage");
            Console.Error.WriteLine("    'win32' - XAML is displayed in win32 window (handle could be obtained from UpdateXamlResultMessage), IDE is responsible to use user32!SetParent");
            Console.Error.WriteLine("    'html' - Previewer starts an HTML server and displays XAML previewer as a web page");
            Console.Error.WriteLine();
            Console.Error.WriteLine("--html-url - endpoint for HTML method to listen on, e. g. http://127.0.0.1:8081");
            Console.Error.WriteLine();
            Console.Error.WriteLine("Example: --transport tcp-bson://127.0.0.1:30243/ --session-id 123 --method alternet-ui-remote MyApp.exe");
            Console.Error.Flush();
            return Die(null);
        }
        
        static CommandLineArgs ParseCommandLineArgs(string[] args)
        {
            var rv = new CommandLineArgs();
            Action<string> next = null;
            try
            {
                foreach (var arg in args)
                {
                    if (next != null)
                    {
                        next(arg);
                        next = null;
                    }
                    else if (arg == "--transport")
                        next = a => rv.Transport = new Uri(a, UriKind.Absolute);
                    else if (arg == "--method")
                        next = a => rv.Method = a;
                    else if (arg == "--html-url")
                        next = a => rv.HtmlMethodListenUri = new Uri(a, UriKind.Absolute);
                    else if (arg == "--session-id")
                        next = a => rv.SessionId = a;
                    else if (rv.AppPath == null)
                        rv.AppPath = arg;
                    else
                        PrintUsage();

                }
                if (rv.AppPath == null || rv.Transport == null)
                    PrintUsage();
            }
            catch
            {
                PrintUsage();
            }

            if (next != null)
                PrintUsage();
            return rv;
        }

        static IAlternetUIRemoteTransportConnection CreateTransport(CommandLineArgs args)
        {
            var transport = args.Transport;
            if (transport.Scheme == "tcp-bson")
            {
                return new BsonTcpTransport().Connect(IPAddress.Parse(transport.Host), transport.Port).Result;
            }

            if (transport.Scheme == "file")
            {
                return new FileWatcherTransport(transport, args.AppPath);
            }
            PrintUsage();
            return null;
        }

        [STAThread]
        public static void Main(string[] cmdline)
        {
            //Test();
            //return;

            Thread.Sleep(500);
            //MessageBox.Show("Attach.");

#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#endif
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            EnsureApplicationCreated();

            UixmlLoader.DisableComponentInitialization = true;
            SetUixmlPreviewerMode();

            args = ParseCommandLineArgs(cmdline);
            var transport = CreateTransport(args);
            if (transport is ITransportWithEnforcedMethod enforcedMethod)
                args.Method = enforcedMethod.PreviewerMethod;
            s_transport = transport;
            transport.OnMessage += OnTransportMessage;
            transport.OnException += (t, e) => Die(e.ToString());
            transport.Start();
            Log("Sending StartDesignerSessionMessage");
            transport.Send(new StartDesignerSessionMessage {SessionId = args.SessionId});

            AttachEventHandler(application, "Idle", Application_Idle);

            var timer = new Timer(TimeSpan.FromMilliseconds(100), OnTimerTick);

            new System.Threading.Timer(_ => WakeUpIdle(), null, 0, 200);

            application.Run(new Window { ShowInTaskbar = false, StartLocation = WindowStartLocation.Manual, Location = new Point(20000, 20000) });

            //RunApplication();

            //DispatcherLoop.Current.Run();
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
            ProcessQueue();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log(e.ExceptionObject.ToString());
        }

        static string GetUixmlClassName(string uixml)
        {
            var document = XDocument.Parse(uixml);
            return document.Root.Attribute((XNamespace)"http://schemas.alternetsoft.com/ui/2021/uixml" + "Class").Value;
        }

        private static void Test()
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

        static Control _c;

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

        //static Window currentWindow;
        private static CommandLineArgs args;

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

        private static void OnTransportMessage(IAlternetUIRemoteTransportConnection transport, object obj) =>
            BeginInvoke((() =>
        {
            if (obj is UpdateXamlMessage xaml)
            {
                try
                {
                    //if (currentWindow != null)
                    //{
                    //    currentWindow.Dispose();
                    //    currentWindow = null;
                    //}

                    var appAssembly = Assembly.LoadFrom(xaml.AssemblyPath);
                    using var stream = new MemoryStream(Encoding.Default.GetBytes(xaml.Xaml));

                    var control = (Control)new UixmlLoader().Load(stream, appAssembly);

                    Window window;

                    if (control is Window w)
                    {
                        window = w;
                        //window.ShowInTaskbar = false;
                        //window.Resizable = false;
                        window.IsToolWindow = true;
                    }
                    else
                    {
                        window = new Window { ShowInTaskbar = false, HasBorder = false, HasTitleBar = false, Resizable = false, IsToolWindow = true };
                        window.Children.Add(control);
                    }

                    //currentWindow = window;
                    window.Show();

                    var timer = new Timer(TimeSpan.FromMilliseconds(10));

                    timer.Tick += (o, e) =>
                    {
                        timer.Stop();
                        timer.Dispose();

                        //var handle = GetHandle(window);

                        var targetDirectoryPath = Path.Combine(Path.GetTempPath(), "AlterNET.UIXMLPreviewer", "UIImages");
                        if (!Directory.Exists(targetDirectoryPath))
                            Directory.CreateDirectory(targetDirectoryPath);

                        var targetFilePath = Path.Combine(targetDirectoryPath, Guid.NewGuid().ToString("N") + ".png");
                        SaveScreenshot(window, targetFilePath);
                        //SaveWindowToBitmap(handle, targetFilePath);

                        window.Close();
                        window.Dispose();

                        s_transport.Send(
                            new PreviewDataMessage()
                            {
                                ImageFileName = targetFilePath,
                                DesiredWidth = (int)window.Width,
                                DesiredHeight = (int)window.Height
                            });
                    };

                    timer.Start();
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    s_transport.Send(new UpdateXamlResultMessage
                    {
                        Error = "Fail",
                        Exception = new ExceptionDetails(e),
                    });
                }
            }
        }));
    }
}
