﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods for log handling.
    /// </summary>
    public static class LogUtils
    {
        /// <summary>
        /// Gets or sets whether to show debug welcome message
        /// with version number and other information.
        /// </summary>
        public static bool ShowDebugWelcomeMessage = false;

        public static LogFlags Flags;

        private static readonly ICustomFlags EventLoggedFlags = Factory.CreateCustomFlags();
        private static int id;
        private static int logUseMaxLength;

        [Flags]
        public enum LogFlags
        {
            AppStartLogged = 1,
            AppFinishLogged = 2,
            VersionLogged = 4,
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

        public static bool IsEventLogged(string eventId) => EventLoggedFlags.HasFlag(eventId);

        public static string? GetEventKey(Type? type, EventInfo? evt)
        {
            if (type is null || evt is null)
                return null;
            var key = type.Name + "." + evt.Name;
            return key;
        }

        public static bool IsEventLogged(Type? type, EventInfo? evt)
        {
            var key = GetEventKey(type, evt);
            if (key is null)
                return false;
            var result = EventLoggedFlags.HasFlag(key);
            return result;
        }

        public static void SetEventLogged(Type? type, EventInfo? evt, bool logged)
        {
            var key = GetEventKey(type, evt);
            if (key is null)
                return;
            EventLoggedFlags.SetFlag(key, logged);
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
            if (!LogUtils.ShowDebugWelcomeMessage)
                return;
            if (LogUtils.Flags.HasFlag(LogUtils.LogFlags.VersionLogged))
                return;
            LogUtils.Flags |= LogUtils.LogFlags.VersionLogged;
            var wxWidgets = SystemSettings.Handler.GetLibraryVersionString();
            var bitsOS = BaseApplication.Is64BitOS ? "x64" : "x86";
            var bitsApp = BaseApplication.Is64BitProcess ? "x64" : "x86";
            var net = $"Net: {Environment.Version}, OS: {bitsOS}, App: {bitsApp}";
            var dpi = $"DPI: {BaseApplication.FirstWindow()?.GetDPI().Width}";
            var ui = $"UI: {SystemSettings.Handler.GetUIVersion()}";
            var counterStr = $"Counter: {BaseApplication.BuildCounter}";
            var s = $"{ui}, {net}, {wxWidgets}, {dpi}, {counterStr}";
            BaseApplication.Log(s);
            if (BaseApplication.LogFileIsEnabled)
                BaseApplication.DebugLog($"Log File = {BaseApplication.LogFilePath}");
        }

        /// <summary>
        /// Logs all system colors.
        /// </summary>
        public static void LogSystemColors()
        {
            var type = typeof(SystemColors);
            foreach (var prop in type.GetFields(
                BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.FieldType == typeof(Color))
                    LogUtils.LogColor(prop.Name, (Color)prop.GetValue(null)!);
            }
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/>.
        /// </summary>
        public static void Log(IEnumerable? items, LogItemKind kind = LogItemKind.Information)
        {
            if (items is null)
                return;
            foreach (var item in items)
                BaseApplication.Log(item, kind);
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/> as section.
        /// </summary>
        public static void LogAsSection(IEnumerable? items, LogItemKind kind = LogItemKind.Information)
        {
            if (items is null)
                return;
            BaseApplication.LogBeginSection();
            Log(items, kind);
            BaseApplication.LogEndSection();
        }

        /// <summary>
        /// Gets logging method based on the <paramref name="toFile"/> parameter value.
        /// </summary>
        /// <param name="toFile">If <c>true</c>, <see cref="LogToFile"/> is returned;
        /// otherwise <see cref="Application.Log"/> is returned.</param>
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
                BaseApplication.Log(value, kind);
            }

            if (toFile)
                return ToFile;
            else
                return ToLog;
        }

        /// <summary>
        /// Logs <see cref="Exception"/> information.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        /// <param name="info">Additional info.</param>
        public static void LogException(Exception e, string? info = default)
        {
            try
            {
                BaseApplication.Log(SectionSeparator, LogItemKind.Error);
                BaseApplication.Log($"Exception: {info}", LogItemKind.Error);
                BaseApplication.Log(e.ToString(), LogItemKind.Error);
                BaseApplication.Log(SectionSeparator, LogItemKind.Error);
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
            var s = prefix;
            if (s != null)
                s += ".";

            PropertyInfo? propInfo = obj?.GetType().GetProperty(propName);
            if (propInfo == null)
                return;
            string? propValue = propInfo?.GetValue(obj)?.ToString();

            if (logUseMaxLength > 0)
                propValue = StringUtils.LimitLength(propValue, LogPropMaxLength);

            BaseApplication.Log(s + propName + " = " + propValue, kind);
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
            if (File.Exists(BaseApplication.LogFilePath))
                File.Delete(BaseApplication.LogFilePath);
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
            filename ??= BaseApplication.LogFilePath;

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
        public static void LogColor(string? title, Color? value)
        {
            if (value is not null)
                value = ColorUtils.FindKnownColor(value);
            title ??= "Color";
            BaseApplication.Log($"{title} = {value?.ToDebugString()}");
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
            BaseApplication.Log($"{title} = {value?.ToDebugString()}, {rect}");
        }

        /// <summary>
        /// Logs <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="kind">Log item kind.</param>
        /// <param name="items">Items to log.</param>
        public static void LogRange(IEnumerable items, LogItemKind kind = LogItemKind.Information)
        {
            foreach (var item in items)
                BaseApplication.Log(item, kind);
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
            BaseApplication.LogError(s);
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
        public static void LogToFileAppStarted()
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

        public static void LogResourceNames()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var resources = assembly.GetManifestResourceNames();

                if (resources.Length == 0)
                    continue;

                LogUtils.LogToFile("========");
                LogUtils.LogToFile($"Name: {assembly.FullName}");
                LogUtils.LogToFile(" ");

                foreach (var resource in resources)
                {
                    LogUtils.LogToFile(resource);
                }

                LogUtils.LogToFile("========");
            }
        }

        /// <summary>
        /// Logs <see cref="SKFont"/> metrics.
        /// </summary>
        public static void LogSkiaFont()
        {
            LogSkiaFont(SkiaUtils.DefaultFont);
        }

        /// <summary>
        /// Logs useful c++ defines.
        /// </summary>
        public static void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        /// <summary>
        /// Logs monospaced Skia fonts.
        /// </summary>
        public static void LogSkiaMonoFonts()
        {
            BaseApplication.LogBeginSection();
            BaseApplication.Log("Skia Mono Fonts:");
            BaseApplication.LogEmptyLine();

            var names = SKFontManager.Default.GetFontFamilies();

            foreach(var name in names)
            {
                var family = SKFontManager.Default.MatchFamily(name);
                if (family.IsFixedPitch)
                    BaseApplication.Log(family.FamilyName);
            }

            BaseApplication.LogEndSection();
        }

        public static void LogSkiaFont(SKFont font)
        {
            BaseApplication.LogBeginSection();
            BaseApplication.LogNameValue("Font", font.Typeface.FamilyName);

            LogMeasureSkiaFont("Hello", font);
            LogMeasureSkiaFont("xy;", SkiaUtils.DefaultFont);

            BaseApplication.LogEndSection();
        }

        public static void LogMeasureSkiaFont(string text, SKFont font)
        {
            SKRect bounds = SKRect.Empty;
            SKPaint paint = new(font);
            var result = paint.MeasureText(text, ref bounds);
            BaseApplication.Log($"Font.MeasureText: \"{text}\", {result}, {bounds}");
        }

        /// <summary>
        /// Logs <see cref="SKFontManager"/> related information.
        /// </summary>
        public static void LogSkiaFontManager()
        {
            List<string> wxNames = new(FontFamily.FamiliesNames);
            wxNames.Sort();

            var s = string.Empty;
            var count = SKFontManager.Default.FontFamilyCount;
            for (int i = 0; i < count; i++)
            {
                var s2 = SKFontManager.Default.GetFamilyName(i);

                var wx = wxNames.IndexOf(s2) >= 0 ? "(wx)" : string.Empty;

                s += $"{s2} {wx}{Environment.NewLine}";

                var styles = SKFontManager.Default.GetFontStyles(i);

                for (int k = 0; k < styles.Count; k++)
                {
                    var style = styles[k];
                    var fontWeight = Font.GetWeightClosestToNumericValue(style.Weight);
                    var fontWidth = Font.GetSkiaWidthClosestToNumericValue(style.Width).ToString()
                        ?? style.Width.ToString();
                    var fontSlant = style.Slant;

                    s += $"{StringUtils.FourSpaces}{fontSlant}, {fontWeight}, {fontWidth}{Environment.NewLine}";
                }
            }

            LogUtils.LogToFile(LogUtils.SectionSeparator);
            LogUtils.LogToFile("Skia Font Families:");
            LogUtils.LogToFile(s);
            LogUtils.LogToFile(LogUtils.SectionSeparator);

            BaseApplication.Log($"{count} Skia font families logged to file.");
        }

        /// <summary>
        /// Logs control related global information (metrics, fonts, etc.).
        /// </summary>
        public static void LogControlInfo(Control control)
        {
            BaseApplication.Log($"Toolbar images: {ToolBarUtils.GetDefaultImageSize(control)}");
            Log($"Control.DefaultFont: {Control.DefaultFont.ToInfoString()}");
            Log($"Font.Default: {Font.Default.ToInfoString()}");
            Log($"Splitter.MinSashSize: {AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize}");
        }

        /// <summary>
        /// Logs OS related information (platform, version, etc.).
        /// </summary>
        public static void LogOSInformation()
        {
            var os = Environment.OSVersion;
            BaseApplication.Log("Current OS Information:\n");
            BaseApplication.Log($"Platform: {os.Platform:G}");
            BaseApplication.Log($"Version String: {os.VersionString}");
            BaseApplication.Log($"Major version: {os.Version.Major}");
            BaseApplication.Log($"Minor version: {os.Version.Minor}");
            BaseApplication.Log($"Service Pack: '{os.ServicePack}'");
        }

        /// <summary>
        /// Logs <see cref="SystemSettings"/>.
        /// </summary>
        public static void LogSystemSettings()
        {
            BaseApplication.LogBeginSection();
            BaseApplication.Log($"IsDark = {SystemSettings.AppearanceIsDark}");
            BaseApplication.Log($"IsUsingDarkBackground = {SystemSettings.IsUsingDarkBackground}");
            BaseApplication.Log($"AppearanceName = {SystemSettings.AppearanceName}");

            var defaultColors = Control.GetStaticDefaultFontAndColor(ControlTypeId.TextBox);
            LogUtils.LogColor("TextBox.ForegroundColor (defaults)", defaultColors.ForegroundColor);
            LogUtils.LogColor("TextBox.BackgroundColor (defaults)", defaultColors.BackgroundColor);

            BaseApplication.Log($"CPP.SizeOfLong = {WebBrowser.DoCommandGlobal("SizeOfLong")}");
            BaseApplication.Log($"CPP.IsDebug = {WebBrowser.DoCommandGlobal("IsDebug")}");

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFeature item in Enum.GetValues(typeof(SystemSettingsFeature)))
            {
                BaseApplication.Log($"HasFeature({item}) = {SystemSettings.HasFeature(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsMetric item in Enum.GetValues(typeof(SystemSettingsMetric)))
            {
                BaseApplication.Log($"GetMetric({item}) = {SystemSettings.GetMetric(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFont item in Enum.GetValues(typeof(SystemSettingsFont)))
            {
                BaseApplication.Log($"GetFont({item}) = {SystemSettings.GetFont(item)}");
            }

            BaseApplication.LogEndSection();
        }

        /// <summary>
        /// Logs <see cref="FontFamily.FamiliesNames"/>.
        /// </summary>
        public static void LogFontFamilies()
        {
            List<string> skiaNames = new(SKFontManager.Default.FontFamilies);
            skiaNames.Sort();

            var s = string.Empty;
            var names = FontFamily.FamiliesNames;
            foreach (string s2 in names)
            {
                var skia = skiaNames.IndexOf(s2) >= 0 ? "(skia)" : string.Empty;

                s += $"{s2} {skia}{Environment.NewLine}";
            }

            LogUtils.LogToFile(LogUtils.SectionSeparator);
            LogUtils.LogToFile("Font Families:");
            LogUtils.LogToFile(s);
            LogUtils.LogToFile(LogUtils.SectionSeparator);

            BaseApplication.Log($"{names.Count()} FontFamilies logged to file.");
        }

        /// <summary>
        /// Writes to log file "Application finished" header text.
        /// </summary>
        public static void LogToFileAppFinished()
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

        internal static void LogAppDomainTargetFrameworkName()
        {
            BaseApplication.Log(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);

            var frameworkName = new System.Runtime.Versioning.FrameworkName(
                AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName!);

            if (frameworkName.Version >= new Version(4, 5))
            {
                // run code
            }
        }

        /*/// <summary>
        /// Tests different methods of getting Argb of the system color.
        /// </summary>
        internal static void TestSystemColors()
        {
            static void Test(KnownColor color)
            {
                var oldArgb = KnownColorTable.GetSystemColorArgb(color);
                var newArgb = KnownColorTable.GetSystemColorArgbUseSystemSettings(color);
                var equal = oldArgb == newArgb;

                var oldArgbStr = oldArgb.ToString("X");
                var newArgbStr = newArgb.ToString("X");

                BaseApplication.Log($"{equal} old: {oldArgbStr} new: {newArgbStr}");
            }

            Test(KnownColor.ActiveBorder);
            Test(KnownColor.ActiveCaption);
            Test(KnownColor.ActiveCaptionText);
            Test(KnownColor.AppWorkspace);
            Test(KnownColor.Control);
            Test(KnownColor.ControlDark);
            Test(KnownColor.ControlDarkDark);
            Test(KnownColor.ControlLight);
            Test(KnownColor.ControlLightLight);
            Test(KnownColor.ControlText);
            Test(KnownColor.Desktop);
            Test(KnownColor.GrayText);
            Test(KnownColor.Highlight);
            Test(KnownColor.HighlightText);
            Test(KnownColor.HotTrack);
            Test(KnownColor.InactiveBorder);
            Test(KnownColor.InactiveCaption);
            Test(KnownColor.InactiveCaptionText);
            Test(KnownColor.Info);
            Test(KnownColor.InfoText);
            Test(KnownColor.Menu);
            Test(KnownColor.MenuText);
            Test(KnownColor.ScrollBar);
            Test(KnownColor.Window);
            Test(KnownColor.WindowFrame);
            Test(KnownColor.WindowText);
            Test(KnownColor.ButtonFace);
            Test(KnownColor.ButtonHighlight);
            Test(KnownColor.ButtonShadow);
            Test(KnownColor.GradientActiveCaption);
            Test(KnownColor.GradientInactiveCaption);
            Test(KnownColor.MenuBar);
            Test(KnownColor.MenuHighlight);
        }*/

        /// <summary>
        /// Enumerates log actions.
        /// </summary>
        public static void EnumLogActions(Action<string, Action> addLogAction)
        {
            addLogAction("Log system settings", LogUtils.LogSystemSettings);
            addLogAction("Log font families", LogUtils.LogFontFamilies);
            addLogAction("Log system fonts", SystemSettings.LogSystemFonts);
            addLogAction("Log fixed width fonts", SystemSettings.LogFixedWidthFonts);
            addLogAction("Log display info", Display.Log);
            addLogAction("Log useful defines", LogUtils.LogUsefulDefines);
            addLogAction("Log OS information", LogUtils.LogOSInformation);
            addLogAction("Log system colors", LogUtils.LogSystemColors);

            addLogAction("Log Embedded Resources in Alternet.UI", () =>
            {
                const string s = "embres:Alternet.UI?assembly=Alternet.UI";

                BaseApplication.Log("Embedded Resource Names added to log file");

                var items = ResourceLoader.GetAssets(new Uri(s), null);
                LogUtils.LogToFile(LogUtils.SectionSeparator);
                foreach (var item in items)
                {
                    LogUtils.LogToFile(item);
                }

                LogUtils.LogToFile(LogUtils.SectionSeparator);
            });

            addLogAction("Log Embedded Resources", () =>
            {
                LogUtils.LogResourceNames();
                BaseApplication.Log("Resource Names added to log file");
            });

            addLogAction("Log test error and warning items", () =>
            {
                BaseApplication.Log("Sample error", LogItemKind.Error);
                BaseApplication.Log("Sample warning", LogItemKind.Warning);
                BaseApplication.Log("Sample info", LogItemKind.Information);
            });

            addLogAction("Log SKFontManager", LogUtils.LogSkiaFontManager);
            addLogAction("Log SKFont", LogUtils.LogSkiaFont);
            addLogAction("Log Skia mono fonts", LogUtils.LogSkiaMonoFonts);
        }
    }
}
