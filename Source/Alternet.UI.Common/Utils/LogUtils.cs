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
            var bitsOS = App.Is64BitOS ? "x64" : "x86";
            var bitsApp = App.Is64BitProcess ? "x64" : "x86";
            var net = $"Net: {Environment.Version}, OS: {bitsOS}, App: {bitsApp}";
            var dpi = $"DPI: {App.FirstWindow()?.GetDPI().Width}";
            var ui = $"UI: {SystemSettings.Handler.GetUIVersion()}";
            var counterStr = $"Counter: {App.BuildCounter}";
            var s = $"{ui}, {net}, {wxWidgets}, {dpi}, {counterStr}";
            App.Log(s);
            if (App.LogFileIsEnabled)
                App.DebugLog($"Log File = {App.LogFilePath}");
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
            Log(items, kind);
            App.LogEndSection();
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
                App.Log(value, kind);
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
                App.Log(SectionSeparator, LogItemKind.Error);
                App.Log($"Exception: {info}", LogItemKind.Error);
                App.Log(e.ToString(), LogItemKind.Error);
                App.Log(SectionSeparator, LogItemKind.Error);
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
        /// Deletes application log file (specified in <see cref="Application.LogFilePath"/>).
        /// </summary>
        public static void DeleteLog()
        {
            if (File.Exists(App.LogFilePath))
                File.Delete(App.LogFilePath);
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
            filename ??= App.LogFilePath;

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
            App.LogBeginSection();
            App.Log("Skia Mono Fonts:");
            App.LogEmptyLine();

            var names = SKFontManager.Default.GetFontFamilies();

            foreach(var name in names)
            {
                var family = SKFontManager.Default.MatchFamily(name);
                if (family.IsFixedPitch)
                    App.Log(family.FamilyName);
            }

            App.LogEndSection();
        }

        public static void LogSkiaFont(SKFont font)
        {
            App.LogBeginSection("SKFont");
            App.LogNameValue("FamilyName", font.Typeface.FamilyName);

            App.LogNameValue("Typeface.UnitsPerEm", font.Typeface.UnitsPerEm);
            App.LogNameValue("ForceAutoHinting", font.ForceAutoHinting);
            App.LogNameValue("Subpixel",font.Subpixel);
            App.LogNameValue("EmbeddedBitmaps", font.EmbeddedBitmaps);
            App.LogNameValue("LinearMetrics",font.LinearMetrics);
            App.LogNameValue("Embolden", font.Embolden);
            App.LogNameValue("BaselineSnap", font.BaselineSnap);
            App.LogNameValue("Size (float)", font.Size);
            App.LogNameValue("Spacing", font.Spacing);
            App.LogNameValue("float Metrics.Top", font.Metrics.Top);
            App.LogNameValue("float Metrics.Ascent", font.Metrics.Ascent);
            App.LogNameValue("float Metrics.Descent", font.Metrics.Descent);
            App.LogNameValue("float Metrics.Bottom", font.Metrics.Bottom);
            App.LogNameValue("float Metrics.Leading", font.Metrics.Leading);
            App.LogNameValue("float Metrics.AverageCharacterWidth", font.Metrics.AverageCharacterWidth);
            App.LogNameValue("float Metrics.MaxCharacterWidth", font.Metrics.MaxCharacterWidth);
            App.LogNameValue("float Metrics.XMin", font.Metrics.XMin);
            App.LogNameValue("float Metrics.XMax", font.Metrics.XMax);
            App.LogNameValue("float Metrics.XHeight", font.Metrics.XHeight);
            App.LogNameValue("float Metrics.CapHeight", font.Metrics.CapHeight);
            App.LogNameValue("float? Metrics.UnderlineThickness", font.Metrics.UnderlineThickness);
            App.LogNameValue("float? Metrics.UnderlinePosition", font.Metrics.UnderlinePosition);
            App.LogNameValue("float? Metrics.StrikeoutThickness", font.Metrics.StrikeoutThickness);
            App.LogNameValue("float? Metrics.StrikeoutPosition", font.Metrics.StrikeoutPosition);

            App.LogEmptyLine();
            LogMeasureSkiaFont("Hello", font);
            LogMeasureSkiaFont("xy;", SkiaUtils.DefaultFont);

            App.LogEndSection();
        }

        public static void LogMeasureSkiaFont(string text, SKFont font)
        {
            SKRect bounds = SKRect.Empty;
            SKPaint paint = new(font);
            var result = paint.MeasureText(text, ref bounds);
            App.Log($"Font.MeasureText: \"{text}\", {result}, {bounds}");
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

            App.Log($"{count} Skia font families logged to file.");
        }

        /// <summary>
        /// Logs control related global information (metrics, fonts, etc.).
        /// </summary>
        public static void LogControlInfo(Control control)
        {
            App.Log($"Toolbar images: {ToolBarUtils.GetDefaultImageSize(control)}");
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
            App.Log("Current OS Information:\n");
            App.Log($"Platform: {os.Platform:G}");
            App.Log($"Version String: {os.VersionString}");
            App.Log($"Major version: {os.Version.Major}");
            App.Log($"Minor version: {os.Version.Minor}");
            App.Log($"Service Pack: '{os.ServicePack}'");
        }

        /// <summary>
        /// Logs <see cref="SystemSettings"/>.
        /// </summary>
        public static void LogSystemSettings()
        {
            App.LogBeginSection();
            App.Log($"IsDark = {SystemSettings.AppearanceIsDark}");
            App.Log($"IsUsingDarkBackground = {SystemSettings.IsUsingDarkBackground}");
            App.Log($"AppearanceName = {SystemSettings.AppearanceName}");

            var defaultColors = Control.GetStaticDefaultFontAndColor(ControlTypeId.TextBox);
            LogUtils.LogColor("TextBox.ForegroundColor (defaults)", defaultColors.ForegroundColor);
            LogUtils.LogColor("TextBox.BackgroundColor (defaults)", defaultColors.BackgroundColor);

            App.Log($"CPP.SizeOfLong = {WebBrowser.DoCommandGlobal("SizeOfLong")}");
            App.Log($"CPP.IsDebug = {WebBrowser.DoCommandGlobal("IsDebug")}");

            App.LogSeparator();

            foreach (SystemSettingsFeature item in Enum.GetValues(typeof(SystemSettingsFeature)))
            {
                App.Log($"HasFeature({item}) = {SystemSettings.HasFeature(item)}");
            }

            App.LogSeparator();

            foreach (SystemSettingsMetric item in Enum.GetValues(typeof(SystemSettingsMetric)))
            {
                App.Log($"GetMetric({item}) = {SystemSettings.GetMetric(item)}");
            }

            App.LogSeparator();

            foreach (SystemSettingsFont item in Enum.GetValues(typeof(SystemSettingsFont)))
            {
                App.Log($"GetFont({item}) = {SystemSettings.GetFont(item)}");
            }

            App.LogEndSection();
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

            App.Log($"{names.Count()} FontFamilies logged to file.");
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
            App.Log(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);

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

                App.Log("Embedded Resource Names added to log file");

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
                App.Log("Resource Names added to log file");
            });

            addLogAction("Log test error and warning items", () =>
            {
                App.Log("Sample error", LogItemKind.Error);
                App.Log("Sample warning", LogItemKind.Warning);
                App.Log("Sample info", LogItemKind.Information);
            });

            addLogAction("Log SKFontManager", LogUtils.LogSkiaFontManager);
            addLogAction("Log SKFont", LogUtils.LogSkiaFont);
            addLogAction("Log Skia mono fonts", LogUtils.LogSkiaMonoFonts);
        }
    }
}
