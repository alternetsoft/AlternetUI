using Alternet.UI.Integration.Remoting;
using Alternet.UI.Integration.UIXmlHostApp.Remote;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Alternet.UI.Integration.UIXmlHostApp
{
    public static class Program
    {
        private static string dllPath;
        private static CommandLineArgs commandLineArgs;
        private static Engine engine;

        private static Assembly interfacesUIAssembly;
        private static Assembly commonUIAssembly;
        private static Assembly alternetUIAssembly;

        [STAThread]
        public static void Main(string[] cmdline)
        {
            Logger.Instance.Information("========");
            Logger.Instance.Information("Parameters:");
            foreach (var s in cmdline)
            {
                Logger.Instance.Information(s);
            }
            Logger.Instance.Information("========");

            //System.Windows.Forms.MessageBox.Show("Attach.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            commandLineArgs = ParseCommandLineArgs(cmdline);

            dllPath = Path.GetDirectoryName(commandLineArgs.AppPath);

            WindowsNativeModulesLocator.SetNativeModulesDirectory();

            var transport = CreateTransport(commandLineArgs);
            if (transport is ITransportWithEnforcedMethod enforcedMethod)
                commandLineArgs.Method = enforcedMethod.PreviewerMethod;
            transport.OnException += (t, e) => Die(e.ToString());

            interfacesUIAssembly = Assembly.LoadFile(GetUIAssemblyPath("Alternet.UI.Interfaces.dll"));

            /*AppDomain.CurrentDomain.AssemblyResolve += AppDomain_AssemblyResolve;*/

            commonUIAssembly = Assembly.LoadFile(GetUIAssemblyPath("Alternet.UI.Common.dll"));

            var alternetUIAssembly = Assembly.LoadFile(GetUIAssemblyPath("Alternet.UI.dll"));

#if NETCOREAPP
            /*SetDotNetCoreNativeDllImportResolver(alternetUIAssembly);*/
#endif

            /*var loaderType = alternetUIAssembly.GetType("Alternet.UI.Integration.UIXmlPreviewLoader");

            var propInfo  = loaderType?.GetProperty("DllPath");

            propInfo?.SetValue(null, dllPath);*/


            /*var alternetUIAssembly = typeof(Alternet.UI.Application).Assembly;*/

            engine = new Engine(transport, commandLineArgs.SessionId, alternetUIAssembly);
            engine.Run();
        }

        /*private static Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Alternet.UI.Interfaces,"))
                return interfacesUIAssembly;
            if (args.Name.StartsWith("Alternet.UI.Common,"))
                return commonUIAssembly;
            if (args.Name.StartsWith("Alternet.UI,"))
                return alternetUIAssembly;
            return args.RequestingAssembly;
        }*/

#if NETCOREAPP
        
        static IntPtr palDll = default;

        private static void SetDotNetCoreNativeDllImportResolver(Assembly alternetUIAssembly)
        {
            NativeLibrary.SetDllImportResolver(alternetUIAssembly,
                (libraryName, assembly, searchPath) =>
                {

                    if (palDll != default)
                        return palDll;

                    IntPtr LoadL(string name, string subFolder)
                    {
                        var nativeDllFilePath =
                            Path.Combine(
                                Path.GetDirectoryName(commandLineArgs.AppPath),
                                subFolder + $"{name}");

                        if (File.Exists(nativeDllFilePath))
                            return NativeLibrary.Load(nativeDllFilePath);
                        return IntPtr.Zero;
                    }

                    if (libraryName == "Alternet.UI.Pal")
                    {
                        palDll = LoadL("Alternet.UI.Pal", @"runtimes\win-x64\native\");
                        return palDll;
                    }
                    else
                        return LoadL(libraryName, string.Empty);
                });
        }
#endif

        private static string GetUIAssemblyPath(string asmName)
        {
            var uiAssemblyPath =
                Path.Combine(Path.GetDirectoryName(commandLineArgs.AppPath), asmName);
            if (!File.Exists(uiAssemblyPath))
                Die($"{asmName} not found.");
            return uiAssemblyPath;
        }

        private static void Die(string error)
        {
            if (error != null)
            {
                Logger.Instance.Fatal(error);
                Console.WriteLine(error);
                Debug.WriteLine(error);
            }

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
            Console.WriteLine(e.ExceptionObject.ToString());
            Debug.WriteLine(e.ExceptionObject.ToString());
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

        private class WindowsNativeModulesLocator
        {
            public static void SetNativeModulesDirectory()
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return;
                /*
                var assemblyDirectory = Path.GetDirectoryName(
                    new Uri(
                        typeof(NativeApiProvider).Assembly.EscapedCodeBase).LocalPath)!;
                */
                var assemblyDirectory = dllPath;
                var nativeModulesDirectory =
                    Path.Combine(assemblyDirectory, IntPtr.Size == 8 ? "x64" : "x86");
                if (!Directory.Exists(nativeModulesDirectory))
                    return;

                var ok = SetDllDirectory(nativeModulesDirectory);
                if (!ok)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern bool SetDllDirectory(string path);
        }
    }
}