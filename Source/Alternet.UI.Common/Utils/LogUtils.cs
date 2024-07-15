using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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

        /// <summary>
        /// Log related flags.
        /// </summary>
        public static LogFlags Flags;

        private static readonly ICustomFlags EventLoggedFlags = Factory.CreateFlags();

        private static List<(string Name, Action Action)>? registeredLogActions;
        private static int id;
        private static int logUseMaxLength;

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
        /// Logs environment versions.
        /// </summary>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogVersion(bool showAnyway = false)
        {
            if (!showAnyway)
            {
                if (!LogUtils.ShowDebugWelcomeMessage)
                    return;
            }

            if (LogUtils.Flags.HasFlag(LogUtils.LogFlags.VersionLogged))
                return;
            LogUtils.Flags |= LogUtils.LogFlags.VersionLogged;
            var wxWidgets = SystemSettings.Handler.GetLibraryVersionString();
            var bitsOS = App.Is64BitOS ? "x64" : "x86";
            var bitsApp = App.Is64BitProcess ? "x64" : "x86";
            var net = $"Net: {Environment.Version}, OS: {bitsOS}, App: {bitsApp}";

            var minDPI = Display.MinDPI;
            var maxDPI = Display.MaxDPI;
            string dpiValue;

            if (minDPI == maxDPI)
                dpiValue = $"{minDPI}";
            else
                dpiValue = $"{minDPI}..{maxDPI}";

            var dpi = $"DPI: {dpiValue}";
            var ui = $"UI: {SystemSettings.Handler.GetUIVersion()}";
            var counterStr = $"Counter: {App.BuildCounter}";
            var s = $"{ui}, {net}, {wxWidgets}, {dpi}, {counterStr}";
            App.Log(s);
            if (App.LogFileIsEnabled)
                App.DebugLog($"Log File = {App.LogFilePath}");
            if (Display.MinScaleFactor != Display.MaxScaleFactor)
                App.LogWarning("Displays have different ScaleFactor");
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
        /// Deletes application log file (specified in <see cref="App.LogFilePath"/>).
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
        /// <param name="filename">Log file path. <see cref="App.LogFilePath"/> is used
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

        /// <summary>
        /// Logs to file all resource names.
        /// </summary>
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
        /// Logs <see cref="SKBitmap"/> metrics.
        /// </summary>
        public static void LogSkiaBitmap()
        {
            App.LogNameValue("SKImageInfo.PlatformColorType", SKImageInfo.PlatformColorType);

            App.LogNameValue("SKImageInfo.PlatformColorAlphaShift", SKImageInfo.PlatformColorAlphaShift);
            App.LogNameValue("SKImageInfo.PlatformColorRedShift", SKImageInfo.PlatformColorRedShift);
            App.LogNameValue("SKImageInfo.PlatformColorGreenShift", SKImageInfo.PlatformColorGreenShift);
            App.LogNameValue("SKImageInfo.PlatformColorBlueShift", SKImageInfo.PlatformColorBlueShift);

            LogSkiaBitmap(new SKBitmap(), "new SKBitmap():");
            LogSkiaBitmap(new SKBitmap(50, 50, isOpaque: true), "new SKBitmap(50, 50, isOpaque: true):");
            LogSkiaBitmap(new SKBitmap(50, 50, isOpaque: false), "new SKBitmap(50, 50, isOpaque: false):");
        }

        /// <summary>
        /// Logs <see cref="SKBitmap"/> metrics.
        /// </summary>
        public static void LogSkiaBitmap(SKBitmap bitmap, string? title = null)
        {
            App.LogSection(
                () =>
                {
                    App.LogNameValue("Size", (bitmap.Width, bitmap.Height));
                    App.LogNameValue("ReadyToDraw", bitmap.ReadyToDraw);
                    App.LogNameValue("DrawsNothing", bitmap.DrawsNothing);
                    App.LogNameValue("IsEmpty", bitmap.IsEmpty);
                    App.LogNameValue("IsNull", bitmap.IsNull);
                    App.LogNameValue("AlphaType", bitmap.AlphaType);
                    App.LogNameValue("BytesPerPixel", bitmap.BytesPerPixel);
                    App.LogNameValue("ColorType", bitmap.ColorType);
                },
                title);
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

            foreach (var name in names)
            {
                var family = SKFontManager.Default.MatchFamily(name);
                if (family.IsFixedPitch)
                    App.Log(family.FamilyName);
            }

            App.LogEndSection();
        }

        /// <summary>
        /// Logs <see cref="SKFont"/>.
        /// </summary>
        /// <param name="font">Font to log.</param>
        public static void LogSkiaFont(SKFont font)
        {
            App.LogBeginSection("SKFont");
            App.LogNameValue("FamilyName", font.Typeface.FamilyName);

            App.LogNameValue("Typeface.UnitsPerEm", font.Typeface.UnitsPerEm);
            App.LogNameValue("ForceAutoHinting", font.ForceAutoHinting);
            App.LogNameValue("Subpixel", font.Subpixel);
            App.LogNameValue("EmbeddedBitmaps", font.EmbeddedBitmaps);
            App.LogNameValue("LinearMetrics", font.LinearMetrics);
            App.LogNameValue("Embolden", font.Embolden);
            App.LogNameValue("BaselineSnap", font.BaselineSnap);
            App.LogNameValue("Size", font.Size);
            App.LogNameValue("Spacing", font.Spacing);
            App.LogNameValue("Metrics.Top", font.Metrics.Top, null, "Greatest distance above the baseline for any glyph. (<= 0).");
            App.LogNameValue("Metrics.Ascent", font.Metrics.Ascent, null, "Recommended distance above the baseline. (<= 0).");
            App.LogNameValue("Metrics.Descent", font.Metrics.Descent, null, "Recommended distance below the baseline. (>= 0).");
            App.LogNameValue("Metrics.Bottom", font.Metrics.Bottom, null, "Greatest distance below the baseline for any glyph. (>= 0).");
            App.LogNameValue("Metrics.Leading", font.Metrics.Leading, null, "Recommended distance to add between lines of text. (>= 0).");
            App.LogNameValue("Metrics.AverageCharacterWidth", font.Metrics.AverageCharacterWidth, null, "Average character width. (>= 0).");
            App.LogNameValue("Metrics.MaxCharacterWidth", font.Metrics.MaxCharacterWidth, null, "Max character width. (>= 0).");
            App.LogNameValue("Metrics.XMin", font.Metrics.XMin, null, "Minimum bounding box x value for all glyphs.");
            App.LogNameValue("Metrics.XMax", font.Metrics.XMax, null, "Maximum bounding box x value for all glyphs.");
            App.LogNameValue("Metrics.XHeight", font.Metrics.XHeight, null, "Height of an 'x' in px. 0 if no 'x' in face.");
            App.LogNameValue("Metrics.CapHeight", font.Metrics.CapHeight, null, "Cap height. Will be > 0, or 0 if cannot be determined.");
            App.LogNameValue("Metrics.UnderlineThickness?", font.Metrics.UnderlineThickness, null, "Thickness of underline. 0 - not determined. null - not set.");
            App.LogNameValue("Metrics.UnderlinePosition?", font.Metrics.UnderlinePosition, null, "Position of top of underline relative to baseline. <0 - above. >0 - below. 0 - on baseline");
            App.LogNameValue("Metrics.StrikeoutThickness?", font.Metrics.StrikeoutThickness, null, "Thickness of strikeout.");
            App.LogNameValue("Metrics.StrikeoutPosition?", font.Metrics.StrikeoutPosition, null, "Position of the bottom of the strikeout stroke relative to the baseline. Is negative when valid.");

            App.LogEmptyLine();
            LogMeasureSkiaFont("Hello", font);
            LogMeasureSkiaFont("xy;", SkiaUtils.DefaultFont);

            App.LogEndSection();
        }

        /// <summary>
        /// Logs measurements of the specified string for the given font.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <param name="font">Font.</param>
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

            App.LogSeparator();
            LogFamily("serif");
            LogFamily("sans-serif");
            LogFamily("cursive");
            LogFamily("fantasy");
            LogFamily("monospace");
            App.LogSeparator();

            void LogFamily(string name)
            {
                var family = SKFontManager.Default.MatchFamily(name);
                App.Log($"{name} -> {family?.FamilyName}");
            }
        }

        /// <summary>
        /// Logs control related global information (metrics, fonts, etc.).
        /// </summary>
        public static void LogControlInfo(Control control)
        {
            App.LogNameValue("Toolbar images", ToolBarUtils.GetDefaultImageSize(control));
            App.LogNameValue("Control.DefaultFont", Control.DefaultFont.ToInfoString());
            App.LogNameValue("Font.Default", Font.Default.ToInfoString());
            App.LogNameValue("Splitter.MinSashSize", AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize);
            App.LogNameValue("Control.DPI", control.GetDPI());
        }

        /// <summary>
        /// Logs OS related information (platform, version, etc.).
        /// </summary>
        public static void LogOSInformation()
        {
            var os = Environment.OSVersion;
            App.Log("Current OS Information:\n");
            App.Log($"Environment.OSVersion.Platform: {os.Platform:G}");
            App.Log($"Environment.OSVersion.VersionString: {os.VersionString}");
            App.Log($"Environment.OSVersion.Version.Major: {os.Version.Major}");
            App.Log($"Environment.OSVersion.Version.Minor: {os.Version.Minor}");
            App.Log($"Environment.OSVersion.ServicePack: '{os.ServicePack}'");
            App.LogNameValue("App.IsWindowsOS", App.IsWindowsOS);

            App.LogNameValue("App.IsLinuxOS", App.IsLinuxOS);
            App.LogNameValue("App.IsMacOS", App.IsMacOS);
            App.LogNameValue("App.IsAndroidOS", App.IsAndroidOS);
            App.LogNameValue("App.IsUnknownOS", App.IsUnknownOS);
            App.LogNameValue("App.IsIOS", App.IsIOS);
            App.LogNameValue("App.Is64BitProcess", App.Is64BitProcess);
            App.LogNameValue("App.Is64BitOS", App.Is64BitOS);

            App.LogNameValue("AppUtils.FrameworkIdentifier", AppUtils.FrameworkIdentifier);
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

            App.LogSeparator();

            App.LogNameValue("Caret.BlinkTime", new Caret().BlinkTime);

            App.LogNameValue("Vector.IsHardwareAccelerated", Vector.IsHardwareAccelerated);

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

        /// <summary>
        /// Tests different methods of getting Argb of the system color.
        /// </summary>
        public static void TestSystemColors(
            Func<KnownSystemColor, int> method1,
            Func<KnownSystemColor, int> method2)
        {
            void Test(KnownSystemColor color)
            {
                var argb1 = method1(color);
                var argb2 = method2(color);
                var equal = argb1 == argb2;

                var oldArgbStr = argb1.ToString("X");
                var newArgbStr = argb2.ToString("X");

                App.Log($"{equal} 1: {oldArgbStr} 2: {newArgbStr}");
            }

            Test(KnownSystemColor.ActiveBorder);
            Test(KnownSystemColor.ActiveCaption);
            Test(KnownSystemColor.ActiveCaptionText);
            Test(KnownSystemColor.AppWorkspace);
            Test(KnownSystemColor.Control);
            Test(KnownSystemColor.ControlDark);
            Test(KnownSystemColor.ControlDarkDark);
            Test(KnownSystemColor.ControlLight);
            Test(KnownSystemColor.ControlLightLight);
            Test(KnownSystemColor.ControlText);
            Test(KnownSystemColor.Desktop);
            Test(KnownSystemColor.GrayText);
            Test(KnownSystemColor.Highlight);
            Test(KnownSystemColor.HighlightText);
            Test(KnownSystemColor.HotTrack);
            Test(KnownSystemColor.InactiveBorder);
            Test(KnownSystemColor.InactiveCaption);
            Test(KnownSystemColor.InactiveCaptionText);
            Test(KnownSystemColor.Info);
            Test(KnownSystemColor.InfoText);
            Test(KnownSystemColor.Menu);
            Test(KnownSystemColor.MenuText);
            Test(KnownSystemColor.ScrollBar);
            Test(KnownSystemColor.Window);
            Test(KnownSystemColor.WindowFrame);
            Test(KnownSystemColor.WindowText);
            Test(KnownSystemColor.ButtonFace);
            Test(KnownSystemColor.ButtonHighlight);
            Test(KnownSystemColor.ButtonShadow);
            Test(KnownSystemColor.GradientActiveCaption);
            Test(KnownSystemColor.GradientInactiveCaption);
            Test(KnownSystemColor.MenuBar);
            Test(KnownSystemColor.MenuHighlight);
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
        /// Enumerates registered log actions.
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
            addLogAction("Log SKBitmap", LogUtils.LogSkiaBitmap);
            addLogAction("Log Skia mono fonts", LogUtils.LogSkiaMonoFonts);
            addLogAction("Log image bits formats", LogImageBitsFormats);

            if (registeredLogActions is not null)
            {
                foreach (var item in registeredLogActions)
                {
                    addLogAction(item.Name, item.Action);
                }
            }

            addLogAction("Test RegistryUtils", () =>
            {
                var previewerPath = RegistryUtils.ReadUIXmlPreviewPath();
                RegistryUtils.WriteUIXmlPreviewPath(@"C:\AlternetUI\UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.exe");
                previewerPath = RegistryUtils.ReadUIXmlPreviewPath();
                App.LogNameValue("UIXmlPreviewPath", previewerPath);
            });
        }

        /// <summary>
        /// Logs image pixel formats.
        /// </summary>
        public static void LogImageBitsFormats()
        {
            GraphicsFactory.NativeBitsFormat.Log("NativeBitsFormat");
            GraphicsFactory.AlphaBitsFormat.Log("AlphaBitsFormat");
            GraphicsFactory.GenericBitsFormat.Log("GenericBitsFormat");

            App.LogSeparator();
            App.LogNameValue("NativeBitsFormat.ColorType", GraphicsFactory.NativeBitsFormat.ColorType);
            App.LogNameValue("AlphaBitsFormat.ColorType", GraphicsFactory.AlphaBitsFormat.ColorType);
            App.LogNameValue("GenericBitsFormat.ColorType", GraphicsFactory.GenericBitsFormat.ColorType);
            App.LogSeparator();
        }
    }
}