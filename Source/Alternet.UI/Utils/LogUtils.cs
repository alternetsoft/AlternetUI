﻿using System;
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

            Application.Log(s + propName + " = " + propValue);
        }

        /// <summary>
        /// Writes to log file "Application started" header text.
        /// </summary>
        public static void LogToFileAppStarted()
        {
            LogUtils.LogToFile(string.Empty);
            LogUtils.LogToFile(string.Empty);
            LogToFile("======================================");
            LogToFile("Application log started");
            LogToFile("======================================");
            LogUtils.LogToFile(string.Empty);
        }

        /// <summary>
        /// Writes to log file "Application finished" header text.
        /// </summary>
        public static void LogToFileAppFinished()
        {
            LogUtils.LogToFile(string.Empty);
            LogUtils.LogToFile(string.Empty);
            LogToFile("======================================");
            LogToFile("Application log finished");
            LogToFile("======================================");
            LogUtils.LogToFile(string.Empty);
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
        /// <param name="msg">Log message</param>
        /// <param name="filename">Log file path. <see cref="Application.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        [Conditional("DEBUG")]
        public static void DebugLogToFile(string msg, string? filename = null)
        {
            LogToFile(msg, filename);
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
    }
}
