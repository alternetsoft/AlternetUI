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
    internal static partial class ControlsSampleUtils
    {
        private static bool cmdLineNoMfcDedug = false;

        public static bool CmdLineNoMfcDedug => cmdLineNoMfcDedug;

        public static string PrepareUrl(string s)
        {
            s = s.Replace('\\', '/')
                .Replace(":", "%3A")
                .Replace(" ", "%20");
            return s;
        }

        public static string PathAddBackslash(string? path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
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
                int index = path.Length - 1;
                char c = path[index];
                if (c != Path.DirectorySeparatorChar)
                    return c == Path.AltDirectorySeparatorChar;
                return true;
            }
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

        public static void ParseCmdLine(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string text = args[i];

                Fn("-nomfcdebug", out cmdLineNoMfcDedug);

                void Fn(string strFlag, out bool boolFlag)
                {
                    boolFlag = text.ToLower() == strFlag;
                }
            }
        }
    }
}
