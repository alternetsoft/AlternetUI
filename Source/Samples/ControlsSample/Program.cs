using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.Drawing;

using Alternet.UI;

namespace ControlsSample
{
    class NativeRedirect
    {
        [DllImport("libc")]
        private static extern int pipe(int[] fds);

        [DllImport("libc")]
        private static extern int dup2(int oldfd, int newfd);

        private static int[] stdoutPipe = new int[2];

        public static void RedirectNativeStdoutToMemory()
        {
            pipe(stdoutPipe); // stdoutPipe[0] = read end, stdoutPipe[1] = write end
            dup2(stdoutPipe[1], 2); // redirect stdout to write end of pipe

            // Start a thread to read from the pipe
            var thr = new Thread(() =>
            {
                using var reader = new FileStream(new Microsoft.Win32.SafeHandles.SafeFileHandle((IntPtr)stdoutPipe[0], ownsHandle: false), FileAccess.Read);
                using var sr = new StreamReader(reader);
                while (true)
                {
                    string? line = sr.ReadLine();
                    if (line != null)
                        Console.WriteLine("Captured native stdout: " + line);
                }
            });
            thr.IsBackground = true;
            thr.Start();
        }
    
        [DllImport("libc")]
        private static extern int close(int fd);

        public static void CleanupRedirection()
        {
            close(stdoutPipe[0]);
            close(stdoutPipe[1]);
            // Optionally restore original stdout/stderr if saved
        }

}    
    internal class Program
    {
        static Program()
        {
            if (App.IsMacOS)
                NativeRedirect.RedirectNativeStdoutToMemory();
            KnownAssemblies.PreloadReferenced();
            FormulaEngine.Init();
        }

        public static void InitSamples()
        {
            PropertyGridSample.MainWindow.LimitedTypesStatic.Add(
                typeof(PropertyGridSample.ControlPainterPreview));

            PropertyGridSample.ObjectInit.Actions.Add(
                typeof(PropertyGridSample.ControlPainterPreview),
                (c) =>
                {
                    var control = (c as PropertyGridSample.ControlPainterPreview)!;
                    control.SuggestedSize = 200;
                });
        }

        public static void LogToFileSimple(object? obj = null)
        {
            string[] stringSplitToArrayChars =
            {
                Environment.NewLine,
                "\r\n",
                "\n\r",
                "\n",
            };

            var location = App.ExecutingAssemblyLocation;
            var logFilePath = Path.ChangeExtension(location, ".log");

            var msg = obj?.ToString() ?? string.Empty;

            string dt = System.DateTime.Now.ToString("HH:mm:ss");
            string[] result = msg.Split(stringSplitToArrayChars, StringSplitOptions.None);

            string contents = string.Empty;

            foreach (string s2 in result)
                contents += $"{dt} :: {s2}{Environment.NewLine}";
            File.AppendAllText(logFilePath, contents);
        }

        public static void LogSimple(string s)
        {
            if (!DebugUtils.DebugLoading)
                return;
            LogToFileSimple(s);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (DebugUtils.IsDebugDefined)
            {
                AbstractControl.UseLayoutMethod = AbstractControl.DefaultLayoutMethod.New;

                DebugUtils.DebugLoading = false;
            }

            LogSimple("===============================================");
            LogSimple("Alternet.UI ControlsSample application started.");

            LogUtils.ShowDebugWelcomeMessage = true;

            var testBadFont = false;

            AppUtils.SetSystemAppearanceIfDebug();

            var application = new Application();

            LogSimple("Application created.");

            try
            {
                CommandLineArgs.Default.Parse(args);

                var uiLanguage = CommandLineArgs.Default.AsString("UILanguage", "en");

                if (uiLanguage != "en" && DebugUtils.IsDebugDefined)
                {
                    if (uiLanguage == "ru")
                    {
                        PropertyGridSample.MainWindow.DoSampleLocalization = false;
                        LocalizationManagerRu.Initialize();
                    }
                }
            }
            catch
            {
            }

            InitSamples();

            LogSimple("InitSamples Done.");

            if (testBadFont)
                AbstractControl.DefaultFont = new Font("abrakadabra", 12);

            var window = new MainWindow();

            LogSimple("Main window created.");

            application.Run(window);

            LogSimple("After Application.Run.");

            window.Dispose();
            application.Dispose();
            if (App.IsMacOS)
                NativeRedirect.RedirectNativeStdoutToMemory();

        }
    }
}