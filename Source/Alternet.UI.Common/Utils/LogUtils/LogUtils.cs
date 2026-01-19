using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods for log handling.
    /// </summary>
    public static partial class LogUtils
    {
        /// <summary>
        /// Represents the time format used for logging timestamps to a file.
        /// </summary>
        /// <remarks>By default this format ensures that log entries include precise timestamps down to the
        /// millisecond level, which can be useful for debugging and performance analysis.</remarks>
        public static string LogToFileTimeFormat = "HH:mm:ss.fff";

        /// <summary>
        /// Gets or sets whether to redirect all log to file operations to <see cref="App.Log"/>.
        /// </summary>
        public static bool RedirectLogFromFileToScreen = false;

        /// <summary>
        /// Gets or sets whether to show debug welcome message
        /// with version number and other information.
        /// </summary>
        public static bool ShowDebugWelcomeMessage = false;

        /// <summary>
        /// Log related flags.
        /// </summary>
        public static LogFlags Flags;

        private static readonly object locker = new();
        private static readonly ICustomFlags EventLoggedFlags = FlagsFactory.Create();

        private static List<(string Name, Action Action)>? registeredLogActions;
        private static int id;
        private static int logUseMaxLength;
        private static string sectionSeparator = "=========";

        /// <summary>
        /// Enumerates log related flags.
        /// </summary>
        [Flags]
        public enum LogFlags
        {
            /// <summary>
            /// Message on application start were logged.
            /// </summary>
            AppStartLogged = 1,

            /// <summary>
            /// Message on application finish were logged.
            /// </summary>
            AppFinishLogged = 2,

            /// <summary>
            /// Application welcome message were logged.
            /// </summary>
            VersionLogged = 4,
        }

        /// <summary>
        /// Gets or sets <see cref="string"/> which is logged before and after log section.
        /// </summary>
        public static string SectionSeparator
        {
            get
            {
                return sectionSeparator;
            }

            set
            {
                sectionSeparator = value;
            }
        }

        /// <summary>
        /// Gets the number of log sections which were opened using <see cref="App.LogBeginSection(string?, LogItemKind)"/>
        /// and not yet closed with <see cref="App.LogEndSection(LogItemKind)"/>.
        /// </summary>
        public static int OpenedSectionsCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets max property value length for the <see cref="LogPropLimitLength"/>
        /// </summary>
        public static int LogPropMaxLength { get; set; } = 80;

        /// <summary>
        /// Gets unique id for debug purposes.
        /// </summary>
        public static int GenNewId()
        {
            lock (locker)
            {
                return id++;
            }
        }

        /// <summary>
        /// Returns whether specified event is logged.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        /// <returns></returns>
        public static bool IsEventLogged(string eventId) => EventLoggedFlags.HasFlag(eventId);

        /// <summary>
        /// Gets event key.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="evt">Event information.</param>
        /// <returns>Event id.</returns>
        public static string? GetEventKey(Type? type, EventInfo? evt)
        {
            if (type is null || evt is null)
                return null;
            var key = type.Name + "." + evt.Name;
            return key;
        }

        /// <summary>
        /// Returns whether specified event is logged.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="evt">Event information.</param>
        /// <returns></returns>
        public static bool IsEventLogged(Type? type, EventInfo? evt)
        {
            var key = GetEventKey(type, evt);
            if (key is null)
                return false;
            var result = EventLoggedFlags.HasFlag(key);
            return result;
        }

        /// <summary>
        /// Temporary adds <paramref name="logAction"/> to <see cref="App.LogMessage"/> event
        /// and calls <paramref name="action"/>.
        /// </summary>
        /// <param name="action">Action to call.</param>
        /// <param name="logAction">Log action to use for log messages output.</param>
        public static void LogActionToAction(Action action, Action<string> logAction)
        {
            App.LogMessage += HandleLogMessage;
            try
            {
                action();
            }
            finally
            {
                App.LogMessage -= HandleLogMessage;
            }

            void HandleLogMessage(object? sender, LogMessageEventArgs e)
            {
                logAction(e.Message ?? string.Empty);
            }
        }

        /// <summary>
        /// Sets whether to log specified event.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="evt">Event information.</param>
        /// <param name="logged">Flag which enables/disables event logging.</param>
        public static void SetEventLogged(Type? type, EventInfo? evt, bool logged)
        {
            var key = GetEventKey(type, evt);
            if (key is null)
                return;
            EventLoggedFlags.SetFlag(key, logged);
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/>.
        /// </summary>
        public static void LogEnumerable(IEnumerable? items, LogItemKind kind = LogItemKind.Information)
        {
            if (items is null)
                return;
            foreach (var item in items)
                App.Log(item, kind);
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/> as section.
        /// </summary>
        public static void LogAsSection(IEnumerable? items, LogItemKind kind = LogItemKind.Information)
        {
            if (items is null)
                return;
            App.LogBeginSection();
            LogEnumerable(items, kind);
            App.LogEndSection();
        }

        /// <summary>
        /// Gets logging method based on the <paramref name="toFile"/> parameter value.
        /// </summary>
        /// <param name="toFile">If <c>true</c>, <see cref="LogToFile"/> is returned;
        /// otherwise <see cref="App.Log"/> is returned.</param>
        /// <returns></returns>
        /// <param name="kind">Log item kind.</param>
        public static Action<object?> GetLogMethod(
            bool toFile,
            LogItemKind kind = LogItemKind.Information)
        {
            static void ToFile(object? value)
            {
                LogToFile(value?.ToString());
            }

            void ToLog(object? value)
            {
                App.Log(value, kind);
            }

            if (toFile)
                return ToFile;
            else
                return ToLog;
        }

        /// <summary>
        /// Logs message to file, debug output and console if it is allowed.
        /// </summary>
        /// <param name="msg">Message to log.</param>
        /// <param name="kind">Message kind.</param>
        public static string[] LogToExternalIfAllowed(string? msg, LogItemKind kind)
        {
            if (msg is null)
                return [];

            App.WriteToLogFileIfAllowed(msg);

            string[] result = msg.Split(
                StringUtils.StringSplitToArrayChars,
                StringSplitOptions.RemoveEmptyEntries);

            if (App.DebugWriteLine.HasKind(kind) || !App.HasLogMessageHandler)
            {
                foreach (string s2 in result)
                {
                    Debug.WriteLine(s2);

                    try
                    {
                        Console.WriteLine(s2);
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Logs values and their percentage relative to the first value.
        /// The first value is treated as 100%.
        /// </summary>
        /// <param name="values">Array of numeric values (e.g. times in ms).
        /// Must be non-null and length > 0.</param>
        /// <param name="titles">Array of titles corresponding to values.
        /// Must be same length as values.</param>
        /// <param name="unit">The unit of measurement (e.g. "ms").</param>
        /// <param name="logger">The logger to use for logging.</param>
        public static void LogPercentRelativeToFirst(
            double[] values,
            string[] titles,
            ILogWriter? logger = null,
            string? unit = null)
        {
            if (unit is not null)
                unit += " ";

            if (values == null) throw new ArgumentNullException(nameof(values));
            if (titles == null) throw new ArgumentNullException(nameof(titles));
            if (values.Length != titles.Length)
                throw new ArgumentException("values and titles must have the same length");
            if (values.Length == 0) return;

            double baseline = values[0];
            bool baselineIsZero = Math.Abs(baseline) < double.Epsilon;

            // compute column widths for neat alignment
            int maxTitleLen = titles.Max(t => (t ?? string.Empty).Length);

            var sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                string title = titles[i] ?? $"#{i}";
                double val = values[i];

                // format numeric value with 3 fractional digits
                string valText = val.ToString("F3", CultureInfo.InvariantCulture);

                string pctText;
                if (i == 0)
                {
                    pctText = "100.0%";
                }
                else if (baselineIsZero)
                {
                    // cannot compute percentage when baseline is 0
                    pctText = "N/A";
                }
                else
                {
                    double pct = (val / baseline) * 100.0;
                    pctText = pct.ToString("F1", CultureInfo.InvariantCulture) + "%";
                }

                // example line: "Default    :   10.123 ms (100.0%)"
                sb.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0,-" + maxTitleLen + "} : {1,9} {2}({3})",
                    title,
                    valText,
                    unit,
                    pctText);
                sb.AppendLine();
            }

            LogWriter.Safe(logger).Write(sb);
        }

        /// <summary>
        /// Logs the contents of the specified dictionary as a named section, writing each key-value pair using the
        /// application's logging mechanism.
        /// </summary>
        /// <remarks>The method writes a begin section log entry with the specified section name, logs
        /// each key-value pair in the dictionary, and then writes an end section log entry. If a key is null, the
        /// string "null" is used as the key name in the log output.</remarks>
        /// <typeparam name="T1">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="T2">The type of the values in the dictionary.</typeparam>
        /// <param name="sectionName">The name of the section under which the dictionary entries will be logged.</param>
        /// <param name="info">The dictionary containing the key-value pairs to log. Keys may be null.</param>
        public static void LogDictionary<T1, T2>(string sectionName, IDictionary<T1, T2> info)
        {
            App.LogBeginSection(sectionName);

            try
            {
                foreach (var kvp in info)
                {
                    var value = kvp.Value;
                    var key = kvp.Key;
                    if (key == null)
                        App.LogNameValue("null", value);
                    else
                        App.LogNameValue(key, value);
                }
            }
            finally
            {
                App.LogEndSection();
            }
        }

        /// <summary>
        /// Creates a <see cref="TreeViewItem"/> for logging an exception.
        /// </summary>
        /// <param name="e">The exception to log.</param>
        /// <param name="kind">The kind of log item. Default is <see cref="LogItemKind.Error"/>.</param>
        /// <param name="allowReplace">Indicates whether replacing the last log item with the
        /// same text is allowed. Currently not implemented.</param>
        /// <returns>A <see cref="TreeViewItem"/> representing the logged exception.</returns>
        public static TreeViewItem CreateLogItemForException(
            Exception e,
            LogItemKind kind = LogItemKind.Error,
            bool allowReplace = false)
        {
            var asString = e.ToString();

            if (e is BaseException baseException)
            {
                if (baseException.AdditionalInformation is not null)
                    asString += Environment.NewLine + baseException.AdditionalInformation;
            }

            var separator = $"{SectionSeparator}";
            LogToExternalIfAllowed(
                $"{separator}{Environment.NewLine}{asString}{Environment.NewLine}{separator}",
                kind);

            var prefix = "Error";
            if (kind != LogItemKind.Error)
                prefix = "Warning";

            var s = $"{prefix} '{e.GetType().Name}': <b>{e.Message}</b>. [Double click...]";

            TreeViewItem item = new(s);
            item.TextHasBold = true;
            item.Tag = asString;
            item.DoubleClickAction = () =>
            {
                App.AddIdleTask(() =>
                {
                    App.ShowExceptionWindow(e, null, true, false);
                });
            };

            return item;
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information.
        /// </summary>
        /// <param name="kind">Message kind. Optional.
        /// Default is <see cref="LogItemKind.Error"/>.</param>
        /// <param name="e">Exception to log.</param>
        /// <param name="allowReplace">Whether to allow replace last log item
        /// if last item has the same text.</param>
        public static void LogException(
            Exception e,
            LogItemKind kind = LogItemKind.Error,
            bool allowReplace = false)
        {
            if (e == App.LastUnhandledException)
                return;

            try
            {
                var item = CreateLogItemForException(e, kind, allowReplace);
                if(e.InnerException is not null)
                {
                    var innerItem = CreateLogItemForException(e.InnerException, kind);
                    item.Add(innerItem);
                }

                App.AddLogItem(item, kind);
            }
            catch (Exception exception)
            {
                BaseObject.Nop(exception);
            }
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information to file.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogExceptionToFile(Exception e, string? filename = null)
        {
            try
            {
                LogToFile(SectionSeparator, filename);
                LogToFile($"Exception:", filename);
                LogToFile(e.ToString(), filename);
                LogToFile(SectionSeparator, filename);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information if DEBUG is defined.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        [Conditional("DEBUG")]
        public static void LogExceptionIfDebug(Exception e)
        {
            LogException(e);
        }

        /// <summary>
        /// Writes to log property value of the specified object.
        /// </summary>
        /// <param name="obj">Object instance.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="prefix">Object name.</param>
        /// <param name="kind">Log item kind.</param>
        public static void LogProp(
            object? obj,
            string propName,
            string? prefix = null,
            LogItemKind kind = LogItemKind.Information)
        {
            PropertyInfo? propInfo = AssemblyUtils.GetPropertySafe(obj?.GetType(), propName);
            if (propInfo == null)
                return;

            var s = prefix;
            if (s != null)
                s += ".";

            string? propValue = propInfo?.GetValue(obj)?.ToString();

            if (logUseMaxLength > 0)
                propValue = StringUtils.LimitLength(propValue, LogPropMaxLength);

            App.Log(s + propName + " = " + propValue, kind);
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
        /// Deletes application log file (specified in <see cref="App.LogFilePath"/>).
        /// </summary>
        public static void DeleteLog()
        {
            try
            {
                if (File.Exists(App.LogFilePath))
                    File.Delete(App.LogFilePath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Logs to file pair of name and value as "{name} = {value}".
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogNameValueToFile(
            object name,
            object? value,
            string? filename = null)
        {
            LogToFile($"{name} = {value}", filename);
        }

        /// <summary>
        /// Begins log to file section.
        /// </summary>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        /// <param name="title">Section title. Optional.</param>
        public static void LogBeginSectionToFile(string? title = null, string? filename = null)
        {
            LogToFile(SectionSeparator, filename);

            if (title is not null)
            {
                LogToFile(title, filename);
                LogToFile(SectionSeparator, filename);
            }
        }

        /// <summary>
        /// Ends log to file section.
        /// </summary>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogEndSectionToFile(string? filename = null)
        {
            LogToFile(LogUtils.SectionSeparator, filename);
        }

        /// <summary>
        /// Logs section using <see cref="LogBeginSectionToFile"/>, <see cref="LogEndSectionToFile"/>
        /// and logging <paramref name="obj"/> between these calls.
        /// </summary>
        /// <param name="obj">Object to log.</param>
        /// <param name="title">Section title (optional).</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogSectionToFile(
            object? obj,
            string? title = null,
            string? filename = null)
        {
            LogBeginSectionToFile(title, filename);
            try
            {
                LogToFile(obj, filename);
            }
            finally
            {
                LogEndSectionToFile(filename);
            }
        }

        /// <summary>
        /// Logs section using <see cref="LogBeginSectionToFile"/>, <see cref="LogEndSectionToFile"/>
        /// and calling <paramref name="action"/> between these calls.
        /// </summary>
        /// <param name="action">Action which is called inside begin/end tags of the section.</param>
        /// <param name="title">Section title (optional).</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogActionToFile(
            Action action,
            string? title = null,
            string? filename = null)
        {
            LogBeginSectionToFile(title, filename);
            try
            {
                action();
            }
            finally
            {
                LogEndSectionToFile(filename);
            }
        }

        /// <summary>
        /// Logs message using <see cref="App.Log"/> and after that calls <see cref="LogToFile"/>.
        /// </summary>
        /// <param name="obj">Object to log.</param>
        public static void LogAndToFile(object? obj = null)
        {
            App.Log(obj);
            if (!App.LogFileIsEnabled)
                LogUtils.LogToFile(obj);
        }

        /// <summary>
        /// Logs the specified object to a file and writes its string representation to the debug output.
        /// </summary>
        /// <remarks>This method combines file logging and debug output for the provided object.
        /// The object's string representation is used for both operations.</remarks>
        /// <param name="obj">The object to log. If <see langword="null"/>,
        /// an empty string is logged and written to the debug output.</param>
        public static void LogToFileAndDebug(object? obj = null)
        {
            LogToFile(obj);
            Debug.WriteLine(obj?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Logs message to the specified file or to default application log file.
        /// </summary>
        /// <param name="obj">Object to log.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        public static void LogToFile(object? obj = null, string? filename = null)
        {
            try
            {
                if (RedirectLogFromFileToScreen)
                {
                    App.LogFileIsEnabled = false;
                    App.Log(obj);
                    return;
                }

                var msg = obj?.ToString() ?? string.Empty;
                filename ??= App.LogFilePath;

                string dt = System.DateTime.Now.ToString(LogToFileTimeFormat);
                string[] result = msg.Split(StringUtils.StringSplitToArrayChars, StringSplitOptions.None);

                string contents = string.Empty;

                foreach (string s2 in result)
                    contents += $"{dt} :: {s2}{Environment.NewLine}";

                File.AppendAllText(filename, contents);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Same as <see cref="LogToFile"/> but writes message to file only under debug environment
        /// (DEBUG conditional is defined) and if <paramref name="conditional"/> is <c>true</c>.
        /// </summary>
        /// <param name="obj">Log message or object.</param>
        /// <param name="conditional">Whether to log message to file.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        [Conditional("DEBUG")]
        public static void DebugLogToFileIf(object? obj, bool conditional, string? filename = null)
        {
            if (conditional)
                LogToFile(obj, filename);
        }

        /// <summary>
        /// Same as <see cref="LogToFile"/> but writes message to file only under debug environment
        /// (DEBUG conditional is defined).
        /// </summary>
        /// <param name="obj">Log message or object.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
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
        public static void LogColor(string? title, Color? value)
        {
            if (value is not null)
                value = ColorUtils.FindKnownColor(value);
            title ??= "Color";
            App.Log($"{title} = {value?.ToDebugString()}");
        }

        /// <summary>
        /// Logs <see cref="Color"/> and <see cref="RectD"/> values.
        /// </summary>
        /// <param name="title">Log label.</param>
        /// <param name="value">Color value.</param>
        /// <param name="rect">Rectangle value.</param>
        public static void LogColorAndRect(Color? value, RectD rect, string? title = null)
        {
            if (value is not null)
                value = ColorUtils.FindKnownColor(value);
            title ??= "ColorAndRect";
            App.Log($"{title} = {value?.ToDebugString()}, {rect}");
        }

        /// <summary>
        /// Logs file names found in the specified folder.
        /// </summary>
        /// <param name="path">Path to folder.</param>
        /// <param name="title">Title to log. Optional.
        /// If not specified <paramref name="path"/> is logged.</param>
        public static void LogFilenamesOfFolder(string path, string? title = null)
        {
            if(title is not null)
                App.Log(title);
            else
                App.Log(title);

            if (path is null)
            {
                App.Log("Empty");
                return;
            }
            else
            {
                App.Log(path);
            }

            var files = Directory.GetFiles(path).Select(System.IO.Path.GetFileName);
            Alternet.UI.LogUtils.LogRangeAsSection(files);
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/> as section (enclosed in separators).
        /// </summary>
        /// <param name="kind">Log item kind.</param>
        /// <param name="items">Items to log.</param>
        public static void LogRangeAsSection(
            IEnumerable items,
            LogItemKind kind = LogItemKind.Information)
        {
            App.LogBeginSection();
            LogRange(items, kind);
            App.LogEndSection();
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="kind">Log item kind.</param>
        /// <param name="items">Items to log.</param>
        public static void LogRange(IEnumerable items, LogItemKind kind = LogItemKind.Information)
        {
            foreach (var item in items)
                App.Log(item, kind);
        }

        /// <summary>
        /// Logs error 'InvalidBoundArgument'.
        /// </summary>
        /// <param name="name">Name of the field or property.</param>
        /// <param name="value">Value.</param>
        /// <param name="minBound">Minimum value.</param>
        /// <param name="maxBound">Maximum value.</param>
        public static void LogInvalidBoundArgument(
            string name,
            object value,
            object minBound,
            object maxBound)
        {
            var s = string.Format(
                ErrorMessages.Default.InvalidBoundArgument,
                name,
                value.ToString(),
                minBound.ToString(),
                maxBound.ToString());
            App.LogError(s);
        }

        /// <summary>
        /// Logs the specified file name (or a folder) with a prefix.
        /// When log item is double clicked, application
        /// opens a preview window for the file.
        /// </summary>
        /// <param name="prefix">The prefix to include in the log message.</param>
        /// <param name="fileName">The path to the file or folder to log and preview.</param>
        /// <param name="kind">The kind of log item. Default is <see cref="LogItemKind.Information"/>.</param>
        public static void LogFileName(
            string prefix,
            string fileName,
            LogItemKind kind = LogItemKind.Information)
        {
            App.LogAction($"{prefix}: {fileName}", () =>
            {
                WindowFilePreview.ShowPreviewWindow(fileName);
            });
        }

        /// <summary>
        /// Logs error 'InvalidBoundArgument' for unsigned <see cref="int"/> values.
        /// </summary>
        /// <param name="name">Name of the field or property.</param>
        /// <param name="value">Value.</param>
        public static void LogInvalidBoundArgumentUInt(string name, int value)
        {
            LogInvalidBoundArgument(name, value, 0, int.MaxValue);
        }

        /// <summary>
        /// Writes to log file "Application started" header text.
        /// </summary>
        public static void LogAppStartedToFile()
        {
            if (Flags.HasFlag(LogFlags.AppStartLogged))
                return;
            try
            {
                Flags |= LogFlags.AppStartLogged;
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
        public static void LogAppFinishedToFile()
        {
            if (Flags.HasFlag(LogFlags.AppFinishLogged))
                return;
            try
            {
                Flags |= LogFlags.AppFinishLogged;
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

        /// <summary>
        /// Registers log action which will be shown in the Developer Tools window.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <param name="action">Action.</param>
        public static void RegisterLogAction(string name, Action action)
        {
            registeredLogActions ??= new();
            registeredLogActions.Add((name, action));
        }

        /// <summary>
        /// Logs exception to console.
        /// </summary>
        /// <param name="e">Exception.</param>
        public static void LogExceptionToConsole(Exception e)
        {
            Console.WriteLine("====== EXCEPTION:");
            Console.WriteLine(e.ToString());
            Console.WriteLine("======");
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/> to file.
        /// </summary>
        /// <param name="items">Range of items.</param>
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
        /// when this parameter is <c>null</c>.</param>
        /// <param name="title">Section title. Optional.</param>
        public static void LogRangeToFile(IEnumerable items, string? title = null, string? filename = null)
        {
            LogBeginSectionToFile(title, filename);
            try
            {
                foreach (var item in items)
                {
                    LogUtils.LogToFile(item, filename);
                }
            }
            finally
            {
                LogEndSectionToFile(filename);
            }
        }

        /// <summary>
        /// Gets the exception message text.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <param name="additionalInfo">Additional information to include in the message.</param>
        /// <returns>A string containing the exception message text
        /// and additional information.</returns>
        public static string GetExceptionMessageText(
            Exception? e,
            string? additionalInfo = null)
        {
            string text = string.Empty;

            if (e is not null)
            {
                text = "Type: " + e.GetType().FullName;

                if (e.Message != null)
                    text += "\n" + "Message: " + e.Message;

                if (e is BaseException baseException)
                {
                    var s = baseException.AdditionalInformation;
                    if (!string.IsNullOrEmpty(s))
                    {
                        text += "\n" + s;
                    }
                }

                var innerExceptionMessage = e.InnerException?.Message ?? string.Empty;

                var containsInnerText = e.Message?.Contains(innerExceptionMessage) ?? false;

                if (e.InnerException is not null && !containsInnerText)
                {
                    text += "\n" + LogUtils.SectionSeparator + "\n";
                    text += "Inner exception: \n";

                    text += GetExceptionMessageText(e.InnerException) + "\n";
                }
            }

            if (additionalInfo is not null)
            {
                text += "\n" + additionalInfo;
            }

            return text;
        }

        /// <summary>
        /// Gets detailed text information about the specified exception.
        /// </summary>
        /// <param name="e">The exception to get details for.</param>
        /// <returns>A string containing detailed information about the exception.</returns>
        public static string GetExceptionDetailsText(Exception? e)
        {
            if (e is null)
                return string.Empty;

            StringBuilder detailsTextBuilder = new();
            string newline = "\n";
            string separator = "----------------------------------------\n";
            string sectionSeparator = "\n************** {0} **************\n";

            detailsTextBuilder.Append(string.Format(
                CultureInfo.CurrentCulture,
                sectionSeparator,
                "Exception Text"));
            detailsTextBuilder.Append(e.ToString());
            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(
                string.Format(
                    CultureInfo.CurrentCulture,
                    sectionSeparator,
                    "Loaded Assemblies"));

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyName name = asm.GetName();
                string? location = AssemblyUtils.GetLocationSafe(asm);
                string? fileVer = "n/a";

                try
                {
                    if (location is not null &&
                        location.Length > 0)
                    {
                        fileVer =
                            FileVersionInfo.GetVersionInfo(location).FileVersion;
                    }
                }
                catch (FileNotFoundException)
                {
                }

                const string ExDlgMsgLoadedAssembliesEntry =
                    "{0}\n    Assembly Version: {1}\n" +
                    "    Win32 Version: {2}\n    CodeBase: {3}\n";

                detailsTextBuilder.Append(
                    string.Format(
                        ExDlgMsgLoadedAssembliesEntry,
                        name.Name,
                        name.Version,
                        fileVer,
                        location));
                detailsTextBuilder.Append(separator);
            }

            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(newline);

            return detailsTextBuilder.ToString();
        }

        /// <summary>
        /// Logs the contents of an object as child items of a <see cref="TreeViewItem"/>.
        /// If the object is an array or an enumerable collection, it logs each item up to a maximum of 100.
        /// Also records the total item count.
        /// </summary>
        /// <param name="parent">The parent tree item to which the logged elements will be added.</param>
        /// <param name="result">The object to log, which may be an array or an enumerable collection.</param>
        public static bool LogAsTreeItemChilds(TreeViewItem parent, object? result)
        {
            if (result is Array array)
            {
                parent.AddWithText($"Total Items: {array.Length}");

                for (int i = 0; i < Math.Min(array.Length, 100); i++)
                {
                    parent.AddWithText($"[{i}] - {array.GetValue(i)}");
                }

                if (array.Length > 100)
                {
                    parent.AddWithText("... (truncated after 100 items)");
                }

                return true;
            }
            else
            if (result is IEnumerable enumerable)
            {
                int count = 0;

                foreach (var item in enumerable)
                {
                    if (count >= 100)
                    {
                        parent.AddWithText("... (truncated after 100 items)");
                        break;
                    }

                    parent.AddWithText($"[{count}] - {item}");

                    count++;
                }

                parent.PrependWithText($"Total Items: {EnumerableUtils.GetCount(enumerable)}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Implements log item.
        /// </summary>
        public class LogItem : BaseObjectWithAttr
        {
            /// <summary>
            /// Gets or sets message of the log item.
            /// </summary>
            public string Msg;

            /// <summary>
            /// Gets or sets kind of the log item.
            /// </summary>
            public LogItemKind Kind;

            /// <summary>
            /// Gets or sets <see cref="ListControlItem"/> used to show log item.
            /// </summary>
            public TreeViewItem? Item;

            private int? id;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogItem"/> class with
            /// the specified parameters.
            /// </summary>
            public LogItem(string msg, LogItemKind kind)
            {
                Msg = msg;
                Kind = kind;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LogItem"/> class
            /// with the specified parameters.
            /// </summary>
            public LogItem(TreeViewItem? item, LogItemKind kind = LogItemKind.Information)
            {
                Msg = string.Empty;
                Kind = kind;
                Item = item;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LogItem"/> class.
            /// </summary>
            public LogItem()
            {
                Msg = string.Empty;
                Kind = LogItemKind.Information;
            }

            /// <summary>
            /// Gets id of the log message.
            /// </summary>
            public int Id => id ??= LogUtils.GenNewId();
        }
    }
}