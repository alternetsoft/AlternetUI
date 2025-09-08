using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to Windows operating system.
    /// </summary>
    public static class MswUtils
    {
        private static bool consoleAllocated = false;

        /// <summary>
        /// Sets preference for the window corners.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="value"></param>
        public static void SetWindowRoundCorners(Window window, WindowRoundedCornerPreference value)
        {
            if (App.IsWindowsOS)
            {
                var version = System.Environment.OSVersion;
                if (version.Version.Major >= 10)
                {
                    IntPtr hWnd = window.GetHandle();
                    var attribute = MswUtils.NativeMethods
                    .DwmWindowAttribute.DWMWA_WINDOW_CORNER_PREFERENCE;
                    var preference = (int)value;
                    MswUtils.NativeMethods.DwmSetWindowAttribute(
                        hWnd,
                        attribute,
                        ref preference,
                        sizeof(uint));
                }
            }
        }

        /// <summary>
        /// Shows console window on the screen.
        /// </summary>
        public static void ShowConsole()
        {
            if (!consoleAllocated)
            {
                MswUtils.NativeMethods.AllocConsole();
                consoleAllocated = true;
            }
        }

        /// <summary>
        /// Retrieves a list of loaded native libraries on Windows.
        /// </summary>
        /// <returns>An array of strings containing the paths of the loaded libraries.</returns>
        internal static string[] GetLoadedLibraries()
        {
            List<string> libraries = new();
            int pid = Process.GetCurrentProcess().Id;
            IntPtr hProcess = NativeMethods.OpenProcess(0x0410, false, (uint)pid);

            IntPtr[] modules = new IntPtr[1024];
            NativeMethods.EnumProcessModules(hProcess, modules, modules.Length * IntPtr.Size, out int needed);

            for (int i = 0; i < needed / IntPtr.Size; i++)
            {
                StringBuilder moduleName = new(260);
                NativeMethods.GetModuleFileNameEx(hProcess, modules[i], moduleName, (uint)moduleName.Capacity);
                libraries.Add(moduleName.ToString());
            }

            return libraries.ToArray();
        }

        /// <summary>
        /// Contains native methods.
        /// </summary>
        public static class NativeMethods
        {
            private const string User32 = "user32.dll";
            private const string UxTheme = "uxtheme.dll";
            private const string DwmApi = "dwmapi.dll";

            internal enum AppThemeMode
            {
                Default,
                AllowDark,
                ForceDark,
                ForceLight,
                Max,
            }

            internal enum WindowCompositionAttribute : uint
            {
                Undefined = 0,
                NcrenderingEnabled = 1,
                NcrenderingPolicy = 2,
                TransitionsForcedisabled = 3,
                AllowNcpaint = 4,
                CaptionButtonBounds = 5,
                NonclientRtlLayout = 6,
                ForceIconicRepresentation = 7,
                ExtendedFrameBounds = 8,
                HasIconicBitmap = 9,
                ThemeAttributes = 10,
                NcrenderingExiled = 11,
                Ncadornmentinfo = 12,
                ExcludedFromLivepreview = 13,
                VideoOverlayActive = 14,
                ForceActivewindowAppearance = 15,
                DisallowPeek = 16,
                Cloak = 17,
                Cloaked = 18,
                AccentPolicy = 19,
                FreezeRepresentation = 20,
                EverUncloaked = 21,
                VisualOwner = 22,
                Holographic = 23,
                ExcludedFromDda = 24,
                Passiveupdatemode = 25,
                Usedarkmodecolors = 26,
                Last = 27,
            }

            /// <summary>
            /// Sets the value of Desktop Window Manager (DWM) non-client rendering
            /// attributes for a window.
            /// </summary>
            [DllImport("dwmapi.dll")]
            public static extern int DwmSetWindowAttribute(
                IntPtr hwnd,
                DwmWindowAttribute dwAttribute,
                ref int pvAttribute,
                int cbAttribute);

#pragma warning disable
            [Flags]
            public enum DwmWindowAttribute : uint
            {
                DWMWA_NCRENDERING_ENABLED = 1,
                DWMWA_NCRENDERING_POLICY,
                DWMWA_TRANSITIONS_FORCEDISABLED,
                DWMWA_ALLOW_NCPAINT,
                DWMWA_CAPTION_BUTTON_BOUNDS,
                DWMWA_NONCLIENT_RTL_LAYOUT,
                DWMWA_FORCE_ICONIC_REPRESENTATION,
                DWMWA_FLIP3D_POLICY,
                DWMWA_EXTENDED_FRAME_BOUNDS,
                DWMWA_HAS_ICONIC_BITMAP,
                DWMWA_DISALLOW_PEEK,
                DWMWA_EXCLUDED_FROM_PEEK,
                DWMWA_CLOAK,
                DWMWA_CLOAKED,
                DWMWA_FREEZE_REPRESENTATION,
                DWMWA_PASSIVE_UPDATE_MODE,
                DWMWA_USE_HOSTBACKDROPBRUSH,
                DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
                DWMWA_WINDOW_CORNER_PREFERENCE = 33,
                DWMWA_BORDER_COLOR,
                DWMWA_CAPTION_COLOR,
                DWMWA_TEXT_COLOR,
                DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
                DWMWA_SYSTEMBACKDROP_TYPE,
                DWMWA_LAST,
            }
#pragma warning restore

            /// <summary>
            /// Windows-specific: resolve OpenGL function pointers
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            [DllImport("opengl32.dll")]
            public static extern IntPtr wglGetProcAddress(string name);

            /// <summary>
            /// Gets key state.
            /// </summary>
            /// <param name="vKey"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern short GetAsyncKeyState(int vKey);

            /// <summary>
            /// Creates file.
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="fileAccess"></param>
            /// <param name="fileShare"></param>
            /// <param name="securityAttributes"></param>
            /// <param name="creationDisposition"></param>
            /// <param name="flags"></param>
            /// <param name="template"></param>
            /// <returns></returns>
            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] uint fileAccess,
                [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] int flags,
                IntPtr template);

            /// <summary>
            /// Writes text to console.
            /// </summary>
            /// <param name="hConsoleOutput"></param>
            /// <param name="lpBuffer"></param>
            /// <param name="dwBufferSize"></param>
            /// <param name="dwBufferCoord"></param>
            /// <param name="lpWriteRegion"></param>
            /// <returns></returns>
            [DllImport(
                "kernel32.dll",
                SetLastError = true,
                CharSet = CharSet.Auto,
                EntryPoint = "WriteConsoleOutputW")]
            public static extern bool WriteConsoleOutput(
                SafeFileHandle hConsoleOutput,
                ConsoleCharInfo[] lpBuffer,
                SmallPoint dwBufferSize,
                SmallPoint dwBufferCoord,
                ref SmallRect lpWriteRegion);

            /// <summary>
            /// Allocates console.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int AllocConsole();

            /// <summary>
            /// Frees console.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int FreeConsole();

            /// <summary>
            /// Gets console window.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(User32, SetLastError = true)]
            internal static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo pwi);

            [DllImport(UxTheme, EntryPoint = "#104")]
            internal static extern void RefreshImmersiveColorPolicyState();

            [DllImport(UxTheme, EntryPoint = "#133", SetLastError = true)]
            internal static extern bool AllowDarkModeForWindow(IntPtr window, bool isDarkModeAllowed);

            /// <remarks>Available in Windows 10 build 1903 (May 2019 Update) and later</remarks>
            [DllImport(UxTheme, EntryPoint = "#135", SetLastError = true)]
            internal static extern bool SetPreferredAppMode(AppThemeMode preferredAppMode);

            /// <remarks>Available only in Windows 10 build 1809 (October 2018 Update)</remarks>
            [DllImport(UxTheme, EntryPoint = "#135", SetLastError = true)]
            internal static extern bool AllowDarkModeForApp(bool isDarkModeAllowed);

            [DllImport(UxTheme, EntryPoint = "#137", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsDarkModeAllowedForWindow(IntPtr window);

            [DllImport(UxTheme, EntryPoint = "#139", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsDarkModeAllowedForApp();

            [DllImport(User32, SetLastError = true)]
            internal static extern bool SetWindowCompositionAttribute(
                IntPtr window,
                ref WindowCompositionAttributeData windowCompositionAttribute);

            [DllImport(User32, SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern bool SetProp(
                IntPtr window,
                string propertyName,
                IntPtr propertyValue);

            [DllImport(DwmApi, SetLastError = false)]
            internal static extern int DwmSetWindowAttribute(
                IntPtr window,
                DwmWindowAttribute attribute,
                IntPtr valuePointer,
                int valuePointerSize);

            [DllImport(User32, CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool SystemParametersInfo(
                uint uiAction,
                uint uiParam,
                ref HighContrastData callback,
                uint fwinini);

            [DllImport(User32, SetLastError = true)]
            internal static extern uint SendMessage(
                IntPtr hWnd,
                uint message,
                IntPtr wParam,
                IntPtr lParam);

            [DllImport(UxTheme, EntryPoint = "#136")]
            internal static extern void FlushMenuThemes();

            [DllImport(User32)]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport(UxTheme, CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int SetWindowTheme(
                IntPtr window,
                string? substituteAppName,
                string? substituteIdList);

            [DllImport("psapi.dll")]
            internal static extern bool EnumProcessModules(
                IntPtr hProcess,
                IntPtr[] lphModule,
                int cb,
                out int lpcbNeeded);

            [DllImport("psapi.dll")]
            internal static extern uint GetModuleFileNameEx(
                IntPtr hProcess,
                IntPtr hModule,
                StringBuilder lpFilename,
                uint nSize);

            [DllImport("kernel32.dll")]
            internal static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            /// <summary>
            /// Represents X and Y coordinates.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SmallPoint
            {
                /// <summary>
                /// Get or sets X coordinate.
                /// </summary>
                public short X;

                /// <summary>
                /// Get or sets Y coordinate.
                /// </summary>
                public short Y;

                /// <summary>
                /// Initializes a new instance of the <see cref="SmallPoint"/> struct.
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                public SmallPoint(short x, short y)
                {
                    X = x;
                    Y = y;
                }
            }

            /// <summary>
            /// Represents unicode or ascii character value.
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct ConsoleCharUnion
            {
                /// <summary>
                /// Gets or sets unicode character value.
                /// </summary>
                [FieldOffset(0)]
                public char UnicodeChar;

                /// <summary>
                /// Gets or sets ascii character value.
                /// </summary>
                [FieldOffset(0)]
                public byte AsciiChar;
            }

            /// <summary>
            /// Represents console character information.
            /// </summary>
            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
            public struct ConsoleCharInfo
            {
                /// <summary>
                /// Character value.
                /// </summary>
                [FieldOffset(0)]
                public ConsoleCharUnion Char;

                /// <summary>
                /// Character foreground and background colors.
                /// </summary>
                [FieldOffset(2)]
                public short Attributes;
            }

            /// <summary>
            /// Represents small rectangle.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SmallRect
            {
                /// <summary>
                /// Left bound of the rectangle.
                /// </summary>
                public short Left;

                /// <summary>
                /// Top bound of the rectangle.
                /// </summary>
                public short Top;

                /// <summary>
                /// Right bound of the rectangle.
                /// </summary>
                public short Right;

                /// <summary>
                /// Bottom bound of the rectangle.
                /// </summary>
                public short Bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal readonly struct HighContrastData
            {
                internal readonly uint size;
                internal readonly uint flags;
                internal readonly IntPtr schemeNamePointer;

                public HighContrastData(object? _ = null)
                {
                    size = (uint)Marshal.SizeOf(typeof(HighContrastData));
                    flags = 0;
                    schemeNamePointer = IntPtr.Zero;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            internal readonly struct WindowCompositionAttributeData
            {
                internal readonly WindowCompositionAttribute attribute;
                internal readonly IntPtr data;
                internal readonly int size;

                public WindowCompositionAttributeData(
                    WindowCompositionAttribute attribute,
                    IntPtr data,
                    int size)
                {
                    this.attribute = attribute;
                    this.data = data;
                    this.size = size;
                }
            }
        }
    }
}
