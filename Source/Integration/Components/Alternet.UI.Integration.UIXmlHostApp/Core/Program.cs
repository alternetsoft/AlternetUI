using Alternet.UI.Integration.Remoting;
using Alternet.UI.Integration.UIXmlHostApp.Remote;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Alternet.UI.Integration.UIXmlHostApp
{
    public static class Program
    {
        private static CommandLineArgs commandLineArgs;

        [STAThread]
        public static void Main(string[] cmdline)
        {
            //System.Windows.Forms.MessageBox.Show("Attach.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            commandLineArgs = ParseCommandLineArgs(cmdline);

            var transport = CreateTransport(commandLineArgs);
            if (transport is ITransportWithEnforcedMethod enforcedMethod)
                commandLineArgs.Method = enforcedMethod.PreviewerMethod;
            transport.OnException += (t, e) => Die(e.ToString());

            var alternetUIAssembly = Assembly.LoadFile(GetUIAssemblyPath());
#if NETCOREAPP
            SetDotNetCoreNativeDllImportResolver(alternetUIAssembly);
#endif

            new Engine(transport, commandLineArgs.SessionId, alternetUIAssembly).Run();
        }

#if NETCOREAPP
        private static void SetDotNetCoreNativeDllImportResolver(Assembly alternetUIAssembly)
        {
            NativeLibrary.SetDllImportResolver(alternetUIAssembly,
                (libraryName, assembly, searchPath) =>
                {
                    if (libraryName == "Alternet.UI.Pal")
                    {
                        var nativeDllFilePath = Path.Combine(
                            Path.GetDirectoryName(commandLineArgs.AppPath),
                            @"runtimes\win-x64\native\Alternet.UI.Pal.dll");

                        if (File.Exists(nativeDllFilePath))
                            return NativeLibrary.Load(nativeDllFilePath);
                    }

                    return IntPtr.Zero;
                });
        }
#endif

        private static string GetUIAssemblyPath()
        {
            var uiAssemblyPath = Path.Combine(Path.GetDirectoryName(commandLineArgs.AppPath), "Alternet.UI.dll");
            if (!File.Exists(uiAssemblyPath))
                Die("Alternet.UI.dll not found.");
            return uiAssemblyPath;
        }

        private static void Die(string error)
        {
            if (error != null)
                Logger.Instance.Fatal(error);

            Environment.Exit(1);
        }

        private static void PrintUsage()
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
            Die(null);
        }

        private static CommandLineArgs ParseCommandLineArgs(string[] args)
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

        private static IAlternetUIRemoteTransportConnection CreateTransport(CommandLineArgs args)
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.Fatal(e.ExceptionObject.ToString());
        }

        internal static class Methods
        {
            public const string AlternetUIRemote = "alternet-ui-remote";
            public const string Win32 = "win32";
            public const string Html = "html";
        }

        private class CommandLineArgs
        {
            public string AppPath { get; set; }

            public Uri Transport { get; set; }

            public Uri HtmlMethodListenUri { get; set; }

            public string Method { get; set; } = Methods.AlternetUIRemote;

            public string SessionId { get; set; } = Guid.NewGuid().ToString();
        }
    }
}