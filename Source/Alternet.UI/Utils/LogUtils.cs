using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods for log handling.
    /// </summary>
    public static class LogUtils
    {
        private static int id;

        /// <summary>
        /// Gets unique id for debug purposes.
        /// </summary>
        public static int GenNewId()
        {
            return id++;
        }

        /// <summary>
        /// Logs <see cref="FontFamily.FamiliesNames"/>.
        /// </summary>
        public static void LogFontFamilies()
        {
            var s = string.Empty;
            foreach (string s2 in FontFamily.FamiliesNames)
            {
                s += s2 + Environment.NewLine;
            }

            LogToFile("=========");
            LogToFile("Font Families:");
            LogToFile(s);
            LogToFile("=========");
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        public static void LogException(Exception e)
        {
            Application.Log("====== EXCEPTION:");
            Application.Log(e.ToString());
            Application.Log("======");
        }

        /// <summary>
        /// Deletes application log file (specified in <see cref="Application.LogFilePath"/>).
        /// </summary>
        public static void DeleteLog()
        {
            if (File.Exists(Application.LogFilePath))
                File.Delete(Application.LogFilePath);
        }

        /// <summary>
        /// Logs message to the specified file or to default application log file.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <param name="filename">Log file path. <see cref="Application.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogToFile(string msg, string? filename = null)
        {
            filename ??= Application.LogFilePath;

            string dt = System.DateTime.Now.ToString(DateUtils.DateFormatJs);
            string[] result = msg.Split(StringUtils.StringSplitToArrayChars, StringSplitOptions.None);

            string contents = string.Empty;

            foreach (string s2 in result)
                contents += $"{dt} :: {s2}{Environment.NewLine}";
            File.AppendAllText(filename, contents);
        }

        /// <summary>
        /// Same as <see cref="LogToFile"/> but writes message to file only under debug environment
        /// (DEBUG conditional is defined).
        /// </summary>
        /// <param name="msg">Log message</param>
        /// <param name="filename">Log file path. <see cref="Application.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        [Conditional("DEBUG")]
        public static void DebugLogToFile(string msg, string? filename = null)
        {
            LogToFile(msg, filename);
        }
    }
}
