using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ControlsSample
{
    internal static partial class CommonUtils
    {
        public static bool CmdLineTest = false;
        public static bool CmdLineLog = false;
        public static bool CmdLineNoMfcDedug = false;
        public static string? CmdLineExecCommands = null;

        public static void Nop() { }

        public static void ParseCmdLine(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string text = args[i];

                fn("-log", out CmdLineLog);
                fn("-nomfcdebug", out CmdLineNoMfcDedug);
                fn("-test", out CmdLineTest);

                if (text.StartsWith("-r=", StringComparison.CurrentCultureIgnoreCase))
                    CmdLineExecCommands = text.Substring("-r=".Length).Trim();
                
                void fn(string strFlag, out bool boolFlag)
                {
                    boolFlag = (text.ToLower() == strFlag);
                }
            }

            if (!CmdLineTest)
            {
                CmdLineTest = File.Exists(GetFileWithExt("test"));
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

        public static string PrepareUrl(string s)
        {
            s = s.Replace('\\', '/')
                .Replace(":", "%3A")
                .Replace(" ", "%20");
            return s;
        }

        public static string PrepareFileUrl(string filename)
        {
            string url = "file:///" + PrepareUrl(filename);
            return url;
        }

        public static string GetAppFolder()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string s = Path.GetDirectoryName(location)!;
            return PathAddBackslash(s);
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

        public static string ResNamePrefix = "ControlsSample.";
        public static Stream GetMyResourceStream(string resName)
        {
            Stream stream = ProcessResPrefix("rsrc://")!;
            if (stream != null)
                return stream;
            if (resName.StartsWith("file://"))
                resName = resName.Remove(0, "file://".Length);

            resName = Path.Combine(GetAppFolder(), resName);
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

        public static void CreateFileWithExt(string ext, string data = "")
        {
            var s = GetFileWithExt(ext);
            var streamWriter = File.CreateText(s);
            streamWriter.WriteLine(data);
            streamWriter.Close();
        }

        //scheme:///C:/example/docs.zip;protocol=zip/main.htm
        public static string PrepareZipUrl(string schemeName, string arcPath, string fileInArchivePath)
        {
            static string prepareUrl(string s)
            {
                s = s.Replace('\\', '/');
                return s;
            }

            string url = schemeName + ":///" + prepareUrl(arcPath) + ";protocol=zip/" +
                prepareUrl(fileInArchivePath);
            return url;
        }

    }

    class DebugTraceListener : TraceListener
    {
#pragma warning disable IDE0079
#pragma warning disable CS8765
        public override void Write(string message)
        {
            CommonUtils.Nop();
        }
        public override void WriteLine(string message)
        {
            CommonUtils.Nop();
        }
#pragma warning restore CS8765
#pragma warning restore IDE0079
        public DebugTraceListener()
        {
        }
    }

}
