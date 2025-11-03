using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;

using Alternet.Drawing;
using Alternet.Skia;

using SkiaSharp;

namespace Alternet.UI
{
    public static partial class LogUtils
    {
        private static TestActionsWindow? testActionsWindow;

        /// <summary>
        /// Enumerates and logs debug actions from the specified type, invoking a callback for each eligible method.
        /// </summary>
        /// <remarks>This method is only executed in debug builds. It identifies static, public methods in
        /// the specified type whose names start with "Test" and have no parameters.
        /// For each such method, the callback
        /// is invoked with the formatted method name (inserting a space after "Test") and an action that
        /// can be used to
        /// invoke the method.</remarks>
        /// <param name="fn">A callback that receives the formatted name of each eligible
        /// method and an <see cref="Action"/> to invoke the method.</param>
        /// <param name="type">The type to inspect for static, public methods that start with
        /// "Test" and have no parameters.</param>
        [Conditional("DEBUG")]
        public static void EnumDebugLogActionsFromType(Action<string, Action> fn, Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (var method in methods)
            {
                var name = method.Name;
                if (!method.Name.StartsWith("Test"))
                    continue;
                var prm = method.GetParameters();
                if (prm is not null && prm.Length > 0)
                    continue;
                name = name.Insert(4, StringUtils.OneSpace);

                fn(name, () =>
                {
                    method.Invoke(null, null);
                });
            }
        }

        /// <summary>
        /// Logs image created from an array with <see cref="TextAndFontStyle"/> elements.
        /// </summary>
        /// <param name="text">Array of strings with font styles.</param>
        public static void LogTextAndFontStyle(TextAndFontStyle[] text)
        {
            var image = DrawingUtils.DebugImageFromTextWithFontStyle(text);
            LogImage(image);
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            PanelDevTools.ShowDeveloperTools();
        }

        /// <summary>
        /// Creates developer tools window.
        /// </summary>
        public static void CreateDeveloperTools()
        {
            PanelDevTools.GetOrCreateDeveloperTools();
        }

        /// <summary>
        /// Logs environment versions.
        /// </summary>
        /// <remarks>
        /// Same as <see cref="LogVersion"/>, but works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogVersion(bool showAnyway = false)
        {
            LogVersion(showAnyway);
        }

        /// <summary>
        /// Gets text for <see cref="LogVersion"/>.
        /// </summary>
        /// <returns></returns>
        public static string GetLogVersionText()
        {
            var wxWidgets = SystemSettings.Handler.GetLibraryVersionString();
            var bitsOS = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture;
            var bitsApp = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

            var mauiPlatform = AssemblyUtils.InvokeMauiUtilsGetDevicePlatform();

            if (mauiPlatform is not null)
            {
                mauiPlatform = $" ({mauiPlatform})";
            }

            var backendOs = $"{App.BackendOS}{mauiPlatform}";

            var net = $"Net: {Environment.Version}, OS: {bitsOS} {backendOs}, App: {bitsApp}";

            var minDPI = Display.MinDPI;
            var maxDPI = Display.MaxDPI;
            string dpiValue;

            if (minDPI == maxDPI)
                dpiValue = $"{minDPI}";
            else
                dpiValue = $"{minDPI}..{maxDPI}";

            var dpi = $"DPI: {dpiValue}";
            var ui = $"UI: {SystemSettings.Handler.GetUIVersion()}";

            var debugger = Debugger.IsAttached ? ", DebuggerAttached"
                : DebugUtils.IsDebugDefined ? " , DebugDefined" : null;

            var s = $"{ui}, {net}, {wxWidgets}, {dpi}{debugger}";

            return s;
        }

        /// <summary>
        /// Adds <see cref="Image"/> to log.
        /// </summary>
        /// <param name="image">Image to log.</param>
        public static void LogImage(Image image)
        {
            TreeViewItem logItem = new();
            logItem.Image = image;
            logItem.HideSelection = true;

            App.AddLogItem(logItem);
        }

        /// <summary>
        /// Logs draw action to log.
        /// </summary>
        /// <param name="scaleFactor">Scale factor to use when draw action is called.</param>
        /// <param name="sizeAndDrawFunc">Function which calculates drawable element.
        /// Called with measure
        /// <see cref="Graphics"/> which can measure text size.</param>
        /// <param name="drawAction">Action which draws an element. If Null,
        /// <paramref name="sizeAndDrawFunc"/>
        /// is used for drawing with <see cref="Graphics"/> created on the <see cref="Bitmap"/>.
        /// <paramref name="sizeAndDrawFunc"/>Optional. </param>
        public static void LogDrawAction(
            Coord scaleFactor,
            Func<Graphics, SizeD> sizeAndDrawFunc,
            Action<Graphics>? drawAction = null)
        {
            var image = DrawingUtils.ImageFromAction(scaleFactor, sizeAndDrawFunc, drawAction);
            LogImage(image);
        }

        /// <summary>
        /// Logs environment versions.
        /// </summary>
        public static void LogVersion(bool showAnyway = false)
        {
            if (!showAnyway)
            {
                if (!LogUtils.ShowDebugWelcomeMessage)
                    return;
            }

            if (LogUtils.Flags.HasFlag(LogUtils.LogFlags.VersionLogged))
                return;
            LogUtils.Flags |= LogUtils.LogFlags.VersionLogged;

            var s = GetLogVersionText();
            App.Log(s);
            if (App.LogFileIsEnabled)
                App.DebugLog($"Log File = {App.LogFilePath}");
            if (Display.MinScaleFactor != Display.MaxScaleFactor)
                App.DebugLog("Displays have different scale factors");

            if(App.PlatformKind == UIPlatformKind.WxWidgets)
            {
                if(VirtualListBox.DefaultRenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp))
                {
                    App.DebugLog("SkiaSharp rendering is forcibly enabled for VirtualListBox.");
                }

                if(VirtualListBox.DefaultRenderingFlags.HasFlag(ControlRenderingFlags.UseOpenGL))
                {
                    App.DebugLog("OpenGL rendering is forcibly enabled for VirtualListBox.");
                }
            }
        }

        /// <summary>
        /// Shows a dialog with the list of test actions.
        /// </summary>
        public static void ShowTestActionsDialog()
        {
            if(testActionsWindow is null)
            {
                testActionsWindow = new();
                testActionsWindow.Disposed += (s, e) =>
                {
                    testActionsWindow = null;
                };
            }

            testActionsWindow.ShowAndFocus();
        }

        /// <summary>
        /// Logs all fonts related information.
        /// </summary>
        public static void LogFontsInformation()
        {
            SystemSettings.LogSystemFonts();
            SystemSettings.LogFixedWidthFonts();

            LogSkiaFontManager();
            LogSkiaFont();
            LogSkiaMonoFonts();
        }

        /// <summary>
        /// Enumerates registered log actions.
        /// </summary>
        public static void EnumLogActions(Action<string, Action> addLogAction)
        {
            List<(string Text, Action Action)> items = new();

            void Fn(string title, Action a)
            {
                items.Add(new(title, a));
            }

            if (!App.IsMaui)
            {
                Fn("> Show Test Actions Dialog", ShowTestActionsDialog);
            }

            Fn("File Preview Window", WindowFilePreview.ShowPreviewWindow);

            Fn("Log system settings", LogUtils.LogSystemSettings);
            Fn("Log font families", LogUtils.LogFontFamilies);
            Fn("Log fonts system", SystemSettings.LogSystemFonts);
            Fn("Log fonts fixed width", SystemSettings.LogFixedWidthFonts);
            Fn("Log display info", Display.Log);

            Fn("Log SIMD support", () =>
            {
                SimdSupportLogger.LogSimdSupport((s) =>
                {
                    App.Log(s);
                });
            });

            Fn("Log useful defines", LogUtils.LogUsefulDefines);
            Fn("Log system information", LogUtils.LogOSInformation);
            Fn("Log system colors", LogUtils.LogSystemColors);
            Fn("Show Alternet.UI commits", GitHubUtils.ShowCommitsForAlternetUI);
            Fn("Log constraint checks", LogUtils.LogCheckConstraints);
            Fn("Log used Alternet assemblies", LogUtils.LogUsedAlternetAssemblies);
            Fn("Log public members of the assembly", LogAssemblyPublicMembers);
            Fn("Log dotnet information", () =>
            {
                App.LogNameValue(
                    "RuntimeEnvironment.GetRuntimeDirectory",
                    System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());

                App.LogNameValue("GetSystemDllPath", CodeGeneratorUtils.GetSystemDllPath());

                App.LogNameValue(
                    "GetDotNetPathFromSystemDllPath()",
                    CodeGeneratorUtils.GetDotNetPathFromSystemDllPath());

                DotNetUtils.LogDefaultLocation();

                App.LogNameValue(
                    "Is Net 9 Runtime installed",
                    DotNetUtils.IsNetCoreRuntimeInstalled(9));

                DotNetUtils.LogEnvironmentVars();

                App.LogEmptyLine();
                App.Log("Installed net runtimes:");
                DotNetUtils.LogInstalledRuntimes();

                App.LogEmptyLine();
                App.Log("Installed net sdk:");
                DotNetUtils.LogInstalledSdk();
                App.LogEmptyLine();

                App.LogNameValue(
                    "DotNetUtils.GetDefaultLocationUsingSpecialFolder()",
                    DotNetUtils.GetDefaultLocationUsingSpecialFolder());
            });

            Fn("Set Height = (Width / 3) * 2", () =>
            {
                foreach (var window in App.Current.Windows)
                {
                    window.Height = (window.Width / 3) * 2;
                }
            });

            void AddToggle(string title, Type type, string propName)
            {
                Fn($"Toggle: {title}", () =>
                {
                    var newValue = AssemblyUtils.ToggleBoolMember(type, null, propName);
                    App.LogNameValue(title, newValue);
                });
            }

            AddToggle(
                "Show Focused Properties",
                typeof(WindowDevTools),
                nameof(WindowDevTools.ShowFocusedProperties));
            AddToggle(
                "Show Debug Focus Rect",
                typeof(WindowDevTools),
                nameof(WindowDevTools.LogGotFocus));
            AddToggle(
                "Focused control info",
                typeof(WindowDevTools),
                nameof(WindowDevTools.LogFocusedControlInfo));
            AddToggle(
                "Use generic caret",
                typeof(Caret),
                nameof(Caret.UseGeneric));

            Fn("Test Draw Bold Text", () =>
            {
                var image = DrawingUtils.ImageFromTextWithBoldTag(
                    "This is text with <b>bold</b> tag.",
                    Display.MaxScaleFactor,
                    Control.DefaultFont,
                    Color.Black);
                LogImage(image);
            });

            Fn("Test wxCHECK_MSG", () => { WebBrowser.DoCommandGlobal("wxCHECK_MSG"); });

            Fn("Log Embedded Resources in Alternet.UI.Common", () =>
            {
                const string s = "embres:Alternet.UI?assembly=Alternet.UI.Common";

                App.Log("Embedded Resource Names added to log file");

                var items = ResourceLoader.GetAssets(new Uri(s), null);
                LogUtils.LogToFile(LogUtils.SectionSeparator);
                foreach (var item in items)
                {
                    LogUtils.LogToFile(item);
                }

                LogUtils.LogToFile(LogUtils.SectionSeparator);
            });

            Fn("Log Embedded Resources", () =>
            {
                LogUtils.LogResourceNames();
                App.Log("Resource Names added to log file");
            });

            Fn("Log test error and warning items", () =>
            {
                App.Log("Sample error", LogItemKind.Error);
                App.Log("Sample warning", LogItemKind.Warning);
                App.Log("Sample info", LogItemKind.Information);
            });

            Fn("Log SkiaSharp: SKFontManager", LogUtils.LogSkiaFontManager);
            Fn("Log SkiaSharp: SKFont", LogUtils.LogSkiaFont);
            Fn("Log SkiaSharp: SKBitmap", LogUtils.LogSkiaBitmap);
            Fn("Log SkiaSharp: Mono fonts", LogUtils.LogSkiaMonoFonts);

            Fn("Log image bits formats", LogImageBitsFormats);
            Fn("Log Control descendants events", LogControlDescendantsEvents);
            Fn("Log Control descendants", LogControlDescendants);

            Fn(
                "Test ShowCriticalMessage",
                () => DialogFactory.ShowCriticalMessage("Critical message."));

            Fn("Log control info", () => LogUtils.LogControlInfo(AppUtils.FirstWindowChildOrEmpty));
            Fn("Test Exception: HookExceptionEvents()", DebugUtils.HookExceptionEvents);
            Fn("Run terminal command", () => DialogFactory.ShowRunTerminalCommandDlg());
            Fn("Show Second MainForm", () => AppUtils.CreateMainWindowClone());
            Fn("Log mapping: Key <-> Keys", KeysExtensions.KeyAndKeysMapping.LogToFile);
            Fn("Log metrics: ScrollBar", ScrollBar.DefaultMetrics(Window.Default).Log);
            Fn("Log CultureInfo.CurrentCulture", LogCurrentCulture);

            EnumDebugLogActions(Fn);

            if (registeredLogActions is not null)
            {
                foreach (var item in registeredLogActions)
                {
                    Fn(item.Name, item.Action);
                }
            }

            Fn("Test RegistryUtils", () =>
            {
                var previewerPath = RegistryUtils.ReadUIXmlPreviewPath();
                RegistryUtils.WriteUIXmlPreviewPath(
                    @"C:\AlternetUI\UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.exe");
                previewerPath = RegistryUtils.ReadUIXmlPreviewPath();
                App.LogNameValue("UIXmlPreviewPath", previewerPath);
            });

            Fn("Log public objects from Alternet.UI.Port", () =>
            {
                var items = AssemblyUtils.EnumPublicObjectsForNamespace(
                    typeof(AbstractControl).Assembly,
                    "Alternet.UI.Port");
                App.Log("Public objects from Alternet.UI.Port added to log file");
                LogRangeToFile(items);
            });

            Fn("Test Exception: Throw C++", () =>
            {
                ExceptionUtils.ForceUnhandledExceptionToUseDialog();
                WebBrowser.DoCommandGlobal("CppThrow");
            });

            Fn("Test Exception: Throw C#", () =>
            {
                ExceptionUtils.ForceUnhandledExceptionToUseDialog();
                throw new FileNotFoundException("Test message", "MyFileName.dat");
            });

            Fn("Test Exception: Show ThreadExceptionWindow", () =>
            {
                try
                {
                    throw new ApplicationException("This is exception message");
                }
                catch (Exception e)
                {
                    App.ShowExceptionWindow(e, "This is an additional info", true);
                }
            });

            Fn("Log metrics: ControlPaint", () =>
            {
                ControlPaint.LogPartSize(AppUtils.FirstWindowChildOrEmpty);
            });

            /* Only available on Windows */

            if (App.IsWindowsOS)
            {
                Fn("Test custom console: Clear", () =>
                {
                    CustomWindowsConsole.Default.Clear();
                });

                Fn("Test ExecuteTerminalEchoCmd", () =>
                {
                    AppUtils.OpenTerminalAndRunEcho("This is echo message");
                });

                Fn("Test custom console: WriteLine", () =>
                {
                    var console = CustomWindowsConsole.Default;

                    console.WriteLine("This is sample text string.");
                    console.WriteLine();

                    console.BackColor = ConsoleColor.White;
                    console.TextColor = ConsoleColor.Black;
                    console.WriteLine("Hello from custom console.");

                    console.BackColor = ConsoleColor.Black;
                    console.TextColor = ConsoleColor.White;
                });
            }

            /* Output log actions */

            int Compare((string Text, Action Action) x, (string Text, Action Action) y)
            {
                return string.Compare(x.Text, y.Text);
            }

            items.Sort(Compare);

            foreach (var item in items)
            {
                addLogAction(item.Text, item.Action);
            }
        }

        /// <summary>
        /// Logs all descendants of the control.
        /// </summary>
        internal static void LogControlDescendants()
        {
            var controls = AssemblyUtils.AllControlDescendants;

            App.Log("Control descendants added to log file");
            LogRangeToFile(controls.Keys);
        }

        /// <summary>
        /// Logs events for all descendants of the control.
        /// </summary>
        internal static void LogControlDescendantsEvents()
        {
            var events = AssemblyUtils.AllControlEvents;
            App.Log("Control descendants event names added to log file");
            LogRangeToFile(events.Keys);
        }

        /// <summary>
        /// Logs image pixel formats.
        /// </summary>
        internal static void LogImageBitsFormats()
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

        /// <summary>
        /// Logs to file all resource names.
        /// </summary>
        internal static void LogResourceNames()
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
        internal static void LogSkiaFont()
        {
            LogSkiaFont(SkiaHelper.DefaultFont);
        }

        /// <summary>
        /// Logs <see cref="SKBitmap"/> metrics.
        /// </summary>
        internal static void LogSkiaBitmap()
        {
            App.LogNameValue("SKImageInfo.PlatformColorType", SKImageInfo.PlatformColorType);

            App.LogNameValue(
                "SKImageInfo.PlatformColorAlphaShift",
                SKImageInfo.PlatformColorAlphaShift);

            App.LogNameValue("SKImageInfo.PlatformColorRedShift", SKImageInfo.PlatformColorRedShift);

            App.LogNameValue(
                "SKImageInfo.PlatformColorGreenShift",
                SKImageInfo.PlatformColorGreenShift);

            App.LogNameValue(
                "SKImageInfo.PlatformColorBlueShift",
                SKImageInfo.PlatformColorBlueShift);

            LogSkiaBitmap(new SKBitmap(), "new SKBitmap():");

            LogSkiaBitmap(
                new SKBitmap(50, 50, isOpaque: true),
                "new SKBitmap(50, 50, isOpaque: true):");

            LogSkiaBitmap(
                new SKBitmap(50, 50, isOpaque: false),
                "new SKBitmap(50, 50, isOpaque: false):");
        }

        /// <summary>
        /// Logs <see cref="SKBitmap"/> metrics.
        /// </summary>
        internal static void LogSkiaBitmap(SKBitmap bitmap, string? title = null)
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
        internal static void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        /// <summary>
        /// Logs monospaced Skia fonts.
        /// </summary>
        internal static void LogSkiaMonoFonts()
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
        internal static void LogSkiaFont(SKFont font)
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

            App.LogNameValue(
                "Metrics.Top",
                font.Metrics.Top,
                null,
                "Greatest distance above the baseline for any glyph. (<= 0).");

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
            LogMeasureSkiaFont("xy;", SkiaHelper.DefaultFont);
            App.LogEmptyLine();

            var canvas1 = GraphicsFactory.GetOrCreateMemoryCanvas(new(1));
            var canvas2 = GraphicsFactory.GetOrCreateMemoryCanvas(new(2));

            var textWidth1 = canvas1.MeasureText("Hello", AbstractControl.DefaultFont);
            var textWidth2 = canvas2.MeasureText("Hello", AbstractControl.DefaultFont);
            App.LogNameValue("Hello width with ScaleFactor 1/2", $"{textWidth1}/{textWidth2}");

            App.LogEndSection();
        }

        /// <summary>
        /// Logs measurements of the specified string for the given font.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <param name="font">Font.</param>
        internal static void LogMeasureSkiaFont(string text, SKFont font)
        {
            var result = font.MeasureText(text, out var bounds);
            App.Log($"Font.MeasureText: \"{text}\", {result}, {bounds}");
        }

        /// <summary>
        /// Logs <see cref="SKFontManager"/> related information.
        /// </summary>
        internal static void LogSkiaFontManager()
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
        internal static void LogControlInfo(AbstractControl control)
        {
            App.LogNameValue("Toolbar images", ToolBarUtils.GetDefaultImageSize(control));
            App.LogNameValue("Control.DefaultFont", AbstractControl.DefaultFont.ToInfoString());
            App.LogNameValue("Font.Default", Font.Default.ToInfoString());
            App.LogNameValue("Splitter.MinSashSize", AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize);
            App.LogNameValue("Control.DPI", control.GetDPI());
        }

        /// <summary>
        /// Logs OS related information (platform, version, etc.).
        /// </summary>
        internal static void LogOSInformation()
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
            App.LogNameValue("C++ CPU", WebBrowser.DoCommandGlobal("CPU"));

            var s = App.Handler.GetCustomData("wx.PortAndVersion");

            if(!string.IsNullOrEmpty(s))
                App.LogNameValue("wx.PortAndVersion", s);

            App.LogNameValue("AppUtils.FrameworkIdentifier", AppUtils.FrameworkIdentifier);

            App.LogNameValue(
                "RuntimeInformation.ProcessArchitecture",
                System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture);

            App.LogNameValue(
                "RuntimeInformation.OSArchitecture",
                System.Runtime.InteropServices.RuntimeInformation.OSArchitecture);

            if (App.IsLinuxOS)
            {
                App.LogNameValue("uname -s", LinuxUtils.UnameResult);
                App.LogNameValue("IsUbuntu", LinuxUtils.IsUbuntu);
            }

            App.LogNameValue("CommonUtils.GetAppExePath()", CommonUtils.GetAppExePath());
        }

        internal static void LogCulture(string sectionName, CultureInfo culture)
        {
            App.LogBeginSection(sectionName + " NumberFormat");

            App.LogNameValue("NumberDecimalDigits", culture.NumberFormat.NumberDecimalDigits);
            App.LogNameValue("NumberDecimalSeparator", culture.NumberFormat.NumberDecimalSeparator);
            App.LogNameValue("NumberGroupSeparator", culture.NumberFormat.NumberGroupSeparator);
            App.LogNameValue("NumberGroupSizes", culture.NumberFormat.NumberGroupSizes);
            App.LogNameValue("NumberNegativePattern", culture.NumberFormat.NumberNegativePattern);
            App.LogNameValue("PercentDecimalDigits", culture.NumberFormat.PercentDecimalDigits);
            App.LogNameValue("PercentDecimalSeparator", culture.NumberFormat.PercentDecimalSeparator);
            App.LogNameValue("PercentGroupSeparator", culture.NumberFormat.PercentGroupSeparator);
            App.LogNameValue("PercentGroupSizes", culture.NumberFormat.PercentGroupSizes);
            App.LogNameValue("PercentNegativePattern", culture.NumberFormat.PercentNegativePattern);
            App.LogNameValue("PercentPositivePattern", culture.NumberFormat.PercentPositivePattern);
            App.LogNameValue("PercentSymbol", culture.NumberFormat.PercentSymbol);
            App.LogNameValue("PerMilleSymbol", culture.NumberFormat.PerMilleSymbol);
            App.LogNameValue("NegativeSign", culture.NumberFormat.NegativeSign);
            App.LogNameValue("NegativeInfinitySymbol", culture.NumberFormat.NegativeInfinitySymbol);
            App.LogNameValue("NativeDigits", culture.NumberFormat.NativeDigits);
            App.LogNameValue("NaNSymbol", culture.NumberFormat.NaNSymbol);
            App.LogNameValue("DigitSubstitution", culture.NumberFormat.DigitSubstitution);
            App.LogNameValue("CurrencySymbol", culture.NumberFormat.CurrencySymbol);
            App.LogNameValue("CurrencyPositivePattern", culture.NumberFormat.CurrencyPositivePattern);
            App.LogNameValue("CurrencyNegativePattern", culture.NumberFormat.CurrencyNegativePattern);
            App.LogNameValue("CurrencyGroupSizes", culture.NumberFormat.CurrencyGroupSizes);
            App.LogNameValue("CurrencyGroupSeparator", culture.NumberFormat.CurrencyGroupSeparator);
            App.LogNameValue("CurrencyDecimalSeparator", culture.NumberFormat.CurrencyDecimalSeparator);
            App.LogNameValue("CurrencyDecimalDigits", culture.NumberFormat.CurrencyDecimalDigits);
            App.LogNameValue("PositiveInfinitySymbol", culture.NumberFormat.PositiveInfinitySymbol);
            App.LogNameValue("PositiveSign", culture.NumberFormat.PositiveSign);
            App.LogEndSection();
        }

        internal static void LogCurrentCulture()
        {
            LogCulture("CultureInfo.CurrentCulture", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Logs <see cref="SystemSettings"/>.
        /// </summary>
        internal static void LogSystemSettings()
        {
            App.LogBeginSection();
            App.Log($"AppearanceIsDark = {SystemSettings.AppearanceIsDark}");
            App.Log($"IsUsingDarkBackground = {SystemSettings.IsUsingDarkBackground}");
            App.Log($"IsWindows11AtLeast = {App.IsWindows11AtLeast}");
            App.Log($"AppearanceName = {SystemSettings.AppearanceName}");

            var defaultColors = AbstractControl.GetStaticDefaultFontAndColor(ControlTypeId.TextBox);
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
        internal static void LogFontFamilies()
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

        internal static void LogAssemblyPublicMembers()
        {
            OpenFileDialog.SelectFile(
            (fileName) =>
            {
                var asm = AssemblyMetaData.LoadAssemblyMetadata(fileName);
                if(asm is null)
                {
                    App.LogError("Error loading assembly: {fileName}");
                    return;
                }

                App.LogBeginSection("Exported Types of the Assembly");
                App.Log($"Assembly: {fileName}");

                var exportedTypes = asm.GetExportedTypes();

                foreach(var type in exportedTypes)
                {
                    App.Log(type.FullName);
                }

                App.LogEndSection();
            },
            FileMaskUtils.FileDialogFilterLibraryFiles);
        }

        internal static void LogUsedAlternetAssemblies()
        {
            var list = KnownAssemblies.AllLoadedAlternet;
            foreach (var item in list)
            {
                App.Log(item.FullName);
            }
        }

        /// <summary>
        /// Logs all system colors.
        /// </summary>
        internal static void LogSystemColors()
        {
            var type = typeof(SystemColors);
            foreach (var prop in type.GetFields(
                BindingFlags.Public | BindingFlags.Static))
            {
                if (prop.FieldType == typeof(Color))
                    LogUtils.LogColor(prop.Name, (Color)prop.GetValue(null)!);
            }

            var bgColor = AbstractControl.GetClassDefaultAttributesBgColor(ControlTypeId.TextBox);
            var fgColor = AbstractControl.GetClassDefaultAttributesFgColor(ControlTypeId.TextBox);
            LogUtils.LogColor("TextBox.ClassDefaultBgColor", bgColor);
            LogUtils.LogColor("TextBox.ClassDefaultFgColor", fgColor);
        }

        /// <summary>
        /// Tests different methods of getting Argb of the system color.
        /// </summary>
        internal static void TestSystemColors(
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

        [Conditional("DEBUG")]
        private static void EnumDebugLogActions(Action<string, Action> fn)
        {
            EnumDebugLogActionsFromType(fn, typeof(Alternet.UI.Tests.Tests));
        }
    }
}
