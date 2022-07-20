using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
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

        static void Log(string message) => Console.WriteLine(message);

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
        
        //interface IAppInitializer
        //{
        //   IAlternetUIRemoteTransportConnection ConfigureApp(IAlternetUIRemoteTransportConnection transport, CommandLineArgs args, object obj);
        //}

        //class AppInitializer<T> : IAppInitializer where T : AppBuilderBase<T>, new()
        //{
        //    public IAlternetUIRemoteTransportConnection ConfigureApp(IAlternetUIRemoteTransportConnection transport,
        //        CommandLineArgs args, object obj)
        //    {
        //        var builder = (AppBuilderBase<T>)obj;
        //        if (args.Method == Methods.AlternetUIRemote)
        //            builder.UseWindowingSubsystem(() => PreviewerWindowingPlatform.Initialize(transport));
        //        if (args.Method == Methods.Html)
        //        {
        //            transport = new HtmlWebSocketTransport(transport,
        //                args.HtmlMethodListenUri ?? new Uri("http://localhost:5000"));
        //            builder.UseWindowingSubsystem(() =>
        //                PreviewerWindowingPlatform.Initialize(transport));
        //        }

        //        if (args.Method == Methods.Win32)
        //            builder.UseWindowingSubsystem("AlternetUI.Win32");
        //        builder.SetupWithoutStarting();
        //        return transport;
        //    }
        //}

        private const string BuilderMethodName = "BuildAlternetUIApp";

        [STAThread]
        public static void Main(string[] cmdline)
        {
            //Test();
            //return;

            args = ParseCommandLineArgs(cmdline);
            var transport = CreateTransport(args);
            if (transport is ITransportWithEnforcedMethod enforcedMethod)
                args.Method = enforcedMethod.PreviewerMethod;
            //var asm = Assembly.LoadFile(System.IO.Path.GetFullPath(args.AppPath));
            //var entryPoint = asm.EntryPoint;
            //if (entryPoint == null)
            //    throw Die($"Assembly {args.AppPath} doesn't have an entry point");
            //var builderMethod = entryPoint.DeclaringType.GetMethod(BuilderMethodName,
            //    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null);
            //if (builderMethod == null)
            //    throw Die($"{entryPoint.DeclaringType.FullName} doesn't have a method named {BuilderMethodName}");
            //Design.IsDesignMode = true;
            //Log($"Obtaining AppBuilder instance from {builderMethod.DeclaringType.FullName}.{builderMethod.Name}");
            //var appBuilder = builderMethod.Invoke(null, null);
            //Log($"Initializing application in design mode");
            //var initializer =(IAppInitializer)Activator.CreateInstance(typeof(AppInitializer<>).MakeGenericType(appBuilder.GetType()));
            //transport = initializer.ConfigureApp(transport, args, appBuilder);
            s_transport = transport;
            transport.OnMessage += OnTransportMessage;
            transport.OnException += (t, e) => Die(e.ToString());
            transport.Start();
            Log("Sending StartDesignerSessionMessage");
            transport.Send(new StartDesignerSessionMessage {SessionId = args.SessionId});

#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#endif

            UixmlLoader.DisableComponentInitialization = true;

            EnsureApplicationCreated();

            AttachEventHandler(application, "Idle", Application_Idle);

            application.Run(new Window { ShowInTaskbar = false, StartLocation = WindowStartLocation.Manual, Location = new Point(20000, 20000) });

            RunApplication();

            //DispatcherLoop.Current.Run();
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
            lock (actions)
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue()();
                }
            }

            //if (!_saved)
            //{
            //    GetHandle(_c, @"c:\temp\1.png");
            //    _saved = true;
            //}
        }

        static MethodInfo getHandleMethod;

        static IntPtr GetHandle(Control control)
        {
            if (getHandleMethod == null)
                getHandleMethod = typeof(ControlHandler).GetMethod("GetHandle", BindingFlags.Instance | BindingFlags.NonPublic);

            return (IntPtr)getHandleMethod.Invoke(control.Handler, new object[0]);
        }

        static void RunApplication()
        {
            var runMethod = typeof(Application).GetMethod("RunWithNoWindow", BindingFlags.Instance | BindingFlags.NonPublic);

            runMethod.Invoke(application, new object[0]);
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

        private static void RebuildPreFlight()
        {
            //PreviewerWindowingPlatform.PreFlightMessages = new List<object>
            //{
            //    s_supportedPixelFormats,
            //    s_viewportAllocatedMessage,
            //    s_renderInfoMessage
            //};
        }

        //private static Window s_currentWindow;
        static Control currentControl;
        private static CommandLineArgs args;

        static Queue<Action> actions = new Queue<Action>();

        static void BeginInvoke(Action action)
        {
            lock (actions)
                actions.Enqueue(action);
        }

        private static void OnTransportMessage(IAlternetUIRemoteTransportConnection transport, object obj) =>
//            DispatcherLoop.Current.BeginInvoke(new Task(() =>
            BeginInvoke((() =>
        {
                if (obj is ClientSupportedPixelFormatsMessage formats)
            {
                s_supportedPixelFormats = formats;
                RebuildPreFlight();
            }
            if (obj is ClientRenderInfoMessage renderInfo)
            {
                s_renderInfoMessage = renderInfo;
                RebuildPreFlight();
            }
            if (obj is ClientViewportAllocatedMessage viewport)
            {
                s_viewportAllocatedMessage = viewport;
                RebuildPreFlight();
            }
            if (obj is UpdateXamlMessage xaml)
            {
                try
                {
                    if (currentControl != null)
                    {
                        currentControl.Dispose();
                        currentControl = null;
                    }

                    var appAssembly = Assembly.LoadFrom(xaml.AssemblyPath);
                    using var stream = new MemoryStream(Encoding.Default.GetBytes(xaml.Xaml));

                    currentControl = (Control)new UixmlLoader().Load(stream, appAssembly);
                    
                    if (currentControl is Window window)
                    {
                        window.ShowInTaskbar = false;
                    }

                    //currentControl.Show();

                    var handle = GetHandle(currentControl);
                    s_transport.Send(new PreviewDataMessage() { WindowHandle = (long)handle });
                }
                catch(Exception e)
                {
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
