using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static partial class CommonProcs
    {
        public static readonly string ResNamePrefix = "ControlsTest.";

        private static readonly string MyLogFilePath =
            Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");

        private static readonly string[] StringSplitToArrayChars =
        [
            Environment.NewLine,
        ];

        public static string StringFormatJs = "yyyy-MM-ddTHH:mm:ss.fffK";
        private static bool cmdLineTest = false;
        private static bool cmdLineLog = false;
        private static bool cmdLineNoMfcDedug = false;
        public static string? cmdLineExecCommands = null;

        public static bool CmdLineTest => cmdLineTest;

        public static bool CmdLineLog => cmdLineLog;

        public static bool CmdLineNoMfcDedug => cmdLineNoMfcDedug;

        public static string? CmdLineExecCommands => cmdLineExecCommands;

        public static void Nop()
        {
        }

        public static void LogException(Exception e)
        {
            LogToFile("====== EXCEPTION:");
            LogToFile(e.ToString());
            LogToFile("======");
        }

        public static void DeleteLog()
        {
            if (File.Exists(MyLogFilePath))
                File.Delete(MyLogFilePath);
        }

        public static string ToString(string[] args)
        {
            string result = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                string text = args[i];
                result += "["+text+"] ";
            }
            return result;
        }

        public static void ParseCmdLine(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string text = args[i];

                Fn("-log", out cmdLineLog);
                Fn("-nomfcdebug", out cmdLineNoMfcDedug);
                Fn("-test", out cmdLineTest);

                if (text.StartsWith(
                        "-r=",
                        StringComparison.CurrentCultureIgnoreCase))
                    cmdLineExecCommands = text.Substring("-r=".Length).Trim();

                void Fn(string strFlag, out bool boolFlag)
                {
                    boolFlag = text.ToLower() == strFlag;
                }
            }

            if (!CmdLineTest)
            {
                cmdLineTest = File.Exists(GetFileWithExt("test"));
            }
        }

        public static string PathAddBackslash(string? path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            path = path.TrimEnd();
            if (PathEndsWithDirectorySeparator())
                return path;
            return path + GetDirectorySeparatorUsedInPath();

            char GetDirectorySeparatorUsedInPath()
            {
                if (path.Contains(Path.AltDirectorySeparatorChar))
                    return Path.AltDirectorySeparatorChar;
                return Path.DirectorySeparatorChar;
            }

            bool PathEndsWithDirectorySeparator()
            {
                if (path.Length == 0)
                    return false;
                char c = path[path.Length - 1];
                if (c != Path.DirectorySeparatorChar)
                    return c == Path.AltDirectorySeparatorChar;
                return true;
            }
        }

        public static string StringFromStream(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        }

        public static string StringFromFile(string filename)
        {
            using FileStream stream = File.OpenRead(filename);
            return StringFromStream(stream);
        }

        public static Stream GetMyResourceStream(string resName)
        {
            Stream stream = ProcessResPrefix("rsrc://")!;
            if (stream != null)
                return stream;
            if (resName.StartsWith("file://"))
                resName = resName.Remove(0, "file://".Length);

            resName = Path.Combine(CommonUtils.GetAppFolder(), resName);
            return File.OpenRead(resName);

            Stream? ProcessResPrefix(string resPrefix)
            {
                if (resName.StartsWith(resPrefix))
                {
                    resName = resName.Remove(0, resPrefix.Length);
                    resName = resName.Replace('/', '.');
                    string name = ResNamePrefix + resName;
                    return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                }

                return null;
            }
        }

        public static void LogToFile(string s)
        {
            if (!CommonProcs.CmdLineTest && !CommonProcs.CmdLineLog)
                return;

            string dt = System.DateTime.Now.ToString(StringFormatJs);
            string[] result = s.Split(StringSplitToArrayChars, StringSplitOptions.None);

            string contents = string.Empty;

            foreach (string s2 in result)
                contents += $"{dt} :: {s2}{Environment.NewLine}";
            File.AppendAllText(MyLogFilePath, contents);
        }

        public static string GetFileWithExt(string ext)
        {
            string sPath1 = Assembly.GetExecutingAssembly().Location;
            string sPath2 = Path.ChangeExtension(sPath1, ext);
            return sPath2;
        }

        public static void RemoveFileWithExt(string ext)
        {
            var s = GetFileWithExt(ext);
            if (File.Exists(s))
                File.Delete(s);
        }

        public static bool ProcessStart(string filePath, string args, string? folder)
        {
            Process process = new ();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = args;
            if(folder!=null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateFileWithExt(string ext, string data = "")
        {
            var s = GetFileWithExt(ext);
            var streamWriter = File.CreateText(s);
            streamWriter.WriteLine(data);
            streamWriter.Close();
        }

        public class DebugTraceListener : TraceListener
        {
            public DebugTraceListener()
            {
            }

#pragma warning disable IDE0079
#pragma warning disable CS8765
            public override void Write(string message)
            {
                CommonProcs.Nop();
            }

            public override void WriteLine(string message)
            {
                CommonProcs.Nop();
            }
#pragma warning restore CS8765
#pragma warning restore IDE0079
        }
    }
}
