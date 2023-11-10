using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private static int logUseMaxLength;
        private static Flags flags;

        [Flags]
        internal enum Flags
        {
            AppStartLogged = 1,
            AppFinishLogged = 2,
        }

        /// <summary>
        /// Gets or sets <see cref="string"/> which is logged before and after log section.
        /// </summary>
        public static string SectionSeparator { get; set; } = "=========";

        /// <summary>
        /// Gets or sets max property value length for the <see cref="LogPropLimitLength"/>
        /// </summary>
        public static int LogPropMaxLength { get; set; } = 80;

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

            LogToFile(SectionSeparator);
            LogToFile("Font Families:");
            LogToFile(s);
            LogToFile(SectionSeparator);
        }

        /// <summary>
        /// Gets logging method based on the <paramref name="toFile"/> parameter value.
        /// </summary>
        /// <param name="toFile">If <c>true</c>, <see cref="LogToFile"/> is returned;
        /// otherwise <see cref="Application.Log"/> is returned.</param>
        /// <returns></returns>
        public static Action<object?> GetLogMethod(bool toFile)
        {
            static void ToFile(object? value)
            {
                LogToFile(value?.ToString());
            }

            if (toFile)
                return ToFile;
            else
                return Application.Log;
        }

        /// <summary>
        /// Logs environment versions.
        /// </summary>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogVersion()
        {
            var wxWidgets = WebBrowser.GetLibraryVersionString();
            var net = $"Net: {Environment.Version}";
            var dpi = $"DPI: {Application.FirstWindow()?.GetDPI().Width}";
            Application.Log($"{net}, {wxWidgets}, {dpi}");

            Application.Log($"Default font: {Font.Default.ToInfoString()}");
            Application.Log($"Bold font: {Font.Default.AsBold.ToInfoString()}");
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        public static void LogException(Exception e)
        {
            Application.Log(SectionSeparator);
            Application.Log("Exception:");
            Application.Log(e.ToString());
            Application.Log(SectionSeparator);
        }

        /// <summary>
        /// Writes to log property value of the specified object.
        /// </summary>
        /// <param name="obj">Object instance.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="prefix">Object name.</param>
        public static void LogProp(object? obj, string propName, string? prefix = null)
        {
            var s = prefix;
            if (s != null)
                s += ".";

            PropertyInfo? propInfo = obj?.GetType().GetProperty(propName);
            if (propInfo == null)
                return;
            string? propValue = propInfo?.GetValue(obj)?.ToString();

            if (logUseMaxLength > 0)
                propValue = StringUtils.LimitLength(propValue, LogPropMaxLength);

            Application.Log(s + propName + " = " + propValue);
        }

        /// <summary>
        /// Writes to log property value of the specified object.
        /// </summary>
        /// <param name="obj">Object instance.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="prefix">Object name.</param>
        /// <remarks>
        /// Uses <see cref="LogPropMaxLength"/> to limit max length of the property value.
        /// </remarks>
        public static void LogPropLimitLength(object? obj, string propName, string? prefix = null)
        {
            logUseMaxLength++;
            try
            {
                LogProp(obj, propName, prefix);
            }
            finally
            {
                logUseMaxLength--;
            }
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
        /// <param name="obj">Log message or object.</param>
        /// <param name="filename">Log file path. <see cref="Application.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogToFile(object? obj = null, string? filename = null)
        {
            var msg = obj?.ToString() ?? string.Empty;
            filename ??= Application.LogFilePath;

            string dt = System.DateTime.Now.ToString("HH:mm:ss");
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
        /// <param name="obj">Log message or object.</param>
        /// <param name="filename">Log file path. <see cref="Application.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        [Conditional("DEBUG")]
        public static void DebugLogToFile(object? obj = null, string? filename = null)
        {
            LogToFile(obj, filename);
        }

        /// <summary>
        /// Logs <see cref="Color"/> value.
        /// </summary>
        /// <param name="title">Color label.</param>
        /// <param name="value">Color value.</param>
        public static void LogColor(string title, Color? value)
        {
            if (value is not null)
                value = ColorUtils.FindKnownColor(value.Value);
            Application.Log($"{title} = {value?.ToDebugString()}");
        }

        /// <summary>
        /// Opens log file <see cref="Application.LogFilePath"/> in the default editor
        /// of the operating system.
        /// </summary>
        public static void OpenLogFile()
        {
            if (!File.Exists(Application.LogFilePath))
                LogToFileAppStarted();

            AppUtils.ShellExecute(Application.LogFilePath);
        }

        /// <summary>
        /// Writes to log file "Application started" header text.
        /// </summary>
        internal static void LogToFileAppStarted()
        {
            if (flags.HasFlag(Flags.AppStartLogged))
                return;
            try
            {
                flags |= Flags.AppStartLogged;
                LogToFile();
                LogToFile();
                LogToFile(SectionSeparator);
                LogToFile("Application log started");
                LogToFile(SectionSeparator);
                LogToFile();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Writes to log file "Application finished" header text.
        /// </summary>
        internal static void LogToFileAppFinished()
        {
            if (flags.HasFlag(Flags.AppFinishLogged))
                return;
            try
            {
                flags |= Flags.AppFinishLogged;
                LogToFile();
                LogToFile();
                LogToFile(SectionSeparator);
                LogToFile("Application log finished");
                LogToFile(SectionSeparator);
                LogToFile();
            }
            catch
            {
            }
        }
    }
}
