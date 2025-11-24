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
        /// Determines whether the current operating system is Windows 10 or later.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the operating system is Windows 10 (version 10.0) or later; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindows10OrLater()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT &&
                   os.Version.Major >= 10;
        }

        /// <summary>
        /// Determines whether the current operating system is Windows 7 or later.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the operating system is Windows 7 (version 6.1) or later; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindows7OrLater()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT &&
                   (os.Version.Major > 6 || (os.Version.Major == 6 && os.Version.Minor >= 1));
        }

        /// <summary>
        /// Determines whether the current operating system is Windows 8 or later.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the operating system is Windows 8 (version 6.2) or later; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindows8OrLater()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT &&
                   (os.Version.Major > 6 || (os.Version.Major == 6 && os.Version.Minor >= 2));
        }

        /// <summary>
        /// Determines whether the current operating system is Windows Vista or later.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the operating system is Windows Vista (version 6.0) or later; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindowsVistaOrLater()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT && os.Version.Major >= 6;
        }

        /// <summary>
        /// Determines whether the current operating system is exactly Windows Vista.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the operating system is exactly Windows Vista (version 6.0); otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExactlyWindowsVista()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT &&
                   os.Version.Major == 6 &&
                   os.Version.Minor == 0;
        }

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
#pragma warning disable
            public const int BI_RGB = 0;

            public const int DIB_RGB_COLORS = 0;

            public const int SRCCOPY = 0x00CC0020;
#pragma warning restore

            private const string User32 = "user32.dll";
            private const string UxTheme = "uxtheme.dll";
            private const string DwmApi = "dwmapi.dll";

            /// <summary>
            /// Specifies the application theme mode, allowing control over light and dark themes.
            /// </summary>
            /// <remarks>This enumeration provides options for configuring the application's theme
            /// behavior, including default system settings, forcing specific themes, or allowing user
            /// preferences.</remarks>
            public enum AppThemeMode
            {
                /// <summary>
                /// The application will use the system's default theme setting,
                /// </summary>
                Default,

                /// <summary>
                /// The application will follow the system theme setting,
                /// adapting to light or dark mode as per the user's system preferences.
                /// </summary>
                AllowDark,

                /// <summary>
                /// The application will always use a dark theme, regardless of the system settings.
                /// </summary>
                ForceDark,

                /// <summary>
                /// Forces the light to turn on regardless of other conditions or settings.
                /// </summary>
                /// <remarks>This method overrides any automatic or conditional light controls,
                /// ensuring the light is turned on. Use this method with caution,
                /// as it may interfere with other light
                /// management systems.</remarks>
                ForceLight,

                /// <summary>
                /// Gets the maximum value.
                /// </summary>
                Max,
            }

            /// <summary>
            /// Represents various window composition attributes that can
            /// be queried or set for a window.
            /// These attributes are used with native calls such
            /// as <c>SetWindowCompositionAttribute</c> to
            /// control non-client rendering, accent policies, cloaking,
            /// dark mode colors, and other window
            /// composition behaviors.
            /// </summary>
            public enum WindowCompositionAttribute : uint
            {
                /// <summary>
                /// Undefined or unknown attribute. No operation.
                /// </summary>
                Undefined = 0,

                /// <summary>
                /// Enables or disables non-client rendering for the window.
                /// </summary>
                NcrenderingEnabled = 1,

                /// <summary>
                /// Sets the non-client rendering policy for the window.
                /// </summary>
                NcrenderingPolicy = 2,

                /// <summary>
                /// When set, transitions (animations) are forcibly disabled.
                /// </summary>
                TransitionsForcedisabled = 3,

                /// <summary>
                /// Allows the window to receive non-client paint messages.
                /// </summary>
                AllowNcpaint = 4,

                /// <summary>
                /// Retrieves or sets the caption button bounds.
                /// </summary>
                CaptionButtonBounds = 5,

                /// <summary>
                /// Sets non-client right-to-left layout for the window.
                /// </summary>
                NonclientRtlLayout = 6,

                /// <summary>
                /// Forces the window to use an iconic (minimized) representation.
                /// </summary>
                ForceIconicRepresentation = 7,

                /// <summary>
                /// Retrieves the extended frame bounds of the window.
                /// </summary>
                ExtendedFrameBounds = 8,

                /// <summary>
                /// Indicates the window has an iconic bitmap.
                /// </summary>
                HasIconicBitmap = 9,

                /// <summary>
                /// Controls theme-related attributes for non-client areas.
                /// </summary>
                ThemeAttributes = 10,

                /// <summary>
                /// Extended non-client rendering state (exiled state).
                /// </summary>
                NcrenderingExiled = 11,

                /// <summary>
                /// Provides additional non-client adornment information.
                /// </summary>
                Ncadornmentinfo = 12,

                /// <summary>
                /// Excludes the window from live preview (alt-tab or peek).
                /// </summary>
                ExcludedFromLivepreview = 13,

                /// <summary>
                /// Indicates that a video overlay is active for the window.
                /// </summary>
                VideoOverlayActive = 14,

                /// <summary>
                /// Forces the active window appearance when set.
                /// </summary>
                ForceActivewindowAppearance = 15,

                /// <summary>
                /// Disables the Peek feature for the window.
                /// </summary>
                DisallowPeek = 16,

                /// <summary>
                /// Cloaks the window (makes it invisible to the user but still present).
                /// </summary>
                Cloak = 17,

                /// <summary>
                /// Indicates whether the window is currently cloaked.
                /// </summary>
                Cloaked = 18,

                /// <summary>
                /// Controls the accent policy (blur, acrylic, etc.) for the window.
                /// </summary>
                AccentPolicy = 19,

                /// <summary>
                /// Freezes the window representation to prevent updates.
                /// </summary>
                FreezeRepresentation = 20,

                /// <summary>
                /// Indicates the window has been ever uncloaked.
                /// </summary>
                EverUncloaked = 21,

                /// <summary>
                /// Identifies the visual owner for the window.
                /// </summary>
                VisualOwner = 22,

                /// <summary>
                /// Holographic-related composition attribute (platform specific).
                /// </summary>
                Holographic = 23,

                /// <summary>
                /// Excludes the window from DirectDraw acceleration (DDA).
                /// </summary>
                ExcludedFromDda = 24,

                /// <summary>
                /// Enables passive update mode for certain composition operations.
                /// </summary>
                Passiveupdatemode = 25,

                /// <summary>
                /// Indicates that dark mode colors should be used for the window.
                /// </summary>
                Usedarkmodecolors = 26,

                /// <summary>
                /// Last valid attribute index (sentinel value).
                /// </summary>
                Last = 27,
            }

            /// <summary>
            /// Specifies attributes for Desktop Window Manager (DWM) window settings.
            /// </summary>
            /// <param name="DirectoryFlags">The directory flags to set.</param>
            /// <returns></returns>
            [DllImport("kernel32", SetLastError = true)]
            public static extern bool SetDefaultDllDirectories(uint DirectoryFlags);

            /// <summary>
            /// Adds a directory to the process DLL search path.
            /// </summary>
            /// <param name="NewDirectory">The directory to add.</param>
            /// <returns>A handle to the added directory if successful; otherwise, <see cref="IntPtr.Zero"/>.</returns>
            [DllImport("kernel32", SetLastError = true)]
            public static extern IntPtr AddDllDirectory([MarshalAs(UnmanagedType.LPWStr)] string NewDirectory);

            /// <summary>
            /// Creates a memory device context (DC) compatible with the specified device context.
            /// </summary>
            /// <remarks>The memory DC can be used for off-screen drawing and is compatible with the
            /// device context specified by <paramref name="hdc"/>. After using the
            /// memory DC, it must be deleted by
            /// calling the <c>DeleteDC</c> function to release system resources.</remarks>
            /// <param name="hdc">A handle to an existing device context.
            /// If this parameter is <see langword="null"/>, the function
            /// creates a memory DC compatible with the application's current screen.</param>
            /// <returns>A handle to the newly created memory device context
            /// if the operation succeeds; otherwise, <see
            /// cref="IntPtr.Zero"/>.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            /// <summary>
            /// Deletes the specified device context (DC).
            /// </summary>
            /// <remarks>This method is a wrapper for the native <c>DeleteDC</c> function in the GDI
            /// library.  It should be used to release a device context that was created
            /// using functions such as
            /// <c>CreateCompatibleDC</c>. Failure to delete a device context may result
            /// in resource leaks.</remarks>
            /// <param name="hdc">A handle to the device context to be deleted.</param>
            /// <returns><see langword="true"/> if the device context is successfully
            /// deleted; otherwise, <see langword="false"/>.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern bool DeleteDC(IntPtr hdc);

            /// <summary>
            /// Selects an object into the specified device context, replacing the previous object.
            /// </summary>
            /// <remarks>This method is a wrapper for the GDI `SelectObject` function. The caller is
            /// responsible for ensuring that the selected object is compatible with the device context.
            /// The previous
            /// object returned by this method should be restored to the device context before releasing
            /// it to avoid
            /// resource leaks.</remarks>
            /// <param name="hdc">A handle to the device context.</param>
            /// <param name="hgdiobj">A handle to the GDI object to be selected into the device context.
            /// This can be a pen, brush, font,
            /// bitmap, or other GDI object.</param>
            /// <returns>A handle to the object being replaced in the device context,
            /// or <see cref="IntPtr.Zero"/> if an error occurs.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

            /// <summary>
            /// Deletes a GDI object, such as a pen, brush, font, bitmap, or region,
            /// and frees any system resources
            /// associated with it.
            /// </summary>
            /// <remarks>After the object is deleted, the handle becomes invalid and should not be
            /// used in subsequent GDI calls.  Ensure that the handle is no longer
            /// in use before calling this method to
            /// avoid undefined behavior.</remarks>
            /// <param name="hObject">A handle to the GDI object to be deleted.
            /// This handle must have been created by a GDI function.</param>
            /// <returns><see langword="true"/> if the object is successfully deleted;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern bool DeleteObject(IntPtr hObject);

            /// <summary>
            /// Performs a bit-block transfer of color data from a source device context
            /// to a destination device
            /// context.
            /// </summary>
            /// <remarks>The <c>BitBlt</c> function is a low-level GDI operation used for copying
            /// pixel data between device contexts. It is commonly used for rendering
            /// graphics or performing screen
            /// captures.</remarks>
            /// <param name="hdcDest">A handle to the destination device context.</param>
            /// <param name="nXDest">The x-coordinate, in logical units, of the upper-left
            /// corner of the destination rectangle.</param>
            /// <param name="nYDest">The y-coordinate, in logical units, of the upper-left
            /// corner of the destination rectangle.</param>
            /// <param name="nWidth">The width, in logical units, of the rectangle
            /// to be transferred.</param>
            /// <param name="nHeight">The height, in logical units, of the rectangle to
            /// be transferred.</param>
            /// <param name="hdcSrc">A handle to the source device context.</param>
            /// <param name="nXSrc">The x-coordinate, in logical units, of the upper-left
            /// corner of the source rectangle.</param>
            /// <param name="nYSrc">The y-coordinate, in logical units, of the upper-left
            /// corner of the source rectangle.</param>
            /// <param name="rop">The raster-operation code that specifies how the source
            /// and destination data are combined. For a list of
            /// common raster-operation codes, see the Windows GDI documentation.</param>
            /// <returns>If the operation succeeds, the return value is a nonzero value.
            /// If the operation fails, the return value
            /// is zero, and extended error information can be retrieved by calling <see
            /// cref="Marshal.GetLastWin32Error"/>.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern int BitBlt(
                IntPtr hdcDest,
                int nXDest,
                int nYDest,
                int nWidth,
                int nHeight,
                IntPtr hdcSrc,
                int nXSrc,
                int nYSrc,
                int rop);

            /// <summary>
            /// Creates a device-independent bitmap (DIB) and returns a handle to the newly created DIB.
            /// </summary>
            /// <remarks>The <see cref="CreateDIBSection"/> function allows applications to directly
            /// access the bits of the bitmap, which can improve performance for certain operations.
            /// The caller is
            /// responsible for releasing the DIB handle using the <c>DeleteObject</c> function
            /// when it is no longer
            /// needed.</remarks>
            /// <param name="hdc">A handle to a device context. This parameter can be
            /// <see langword="null"/> if the DIB is not associated
            /// with a specific device context.</param>
            /// <param name="pbmi">A reference to a <see cref="BITMAPINFO"/> structure
            /// that specifies the dimensions and color format of
            /// the DIB.</param>
            /// <param name="iUsage">Specifies whether the <paramref name="pbmi"/>
            /// structure contains RGB values or palette indices.  Use
            /// <c>DIB_RGB_COLORS</c> for RGB values or <c>DIB_PAL_COLORS</c> for palette indices.</param>
            /// <param name="ppvBits">When the function returns, this parameter receives
            /// a pointer to the location of the DIB's bit values.</param>
            /// <param name="hSection">A handle to a file mapping object that the
            /// DIB will use to store the bitmap. If this parameter is <see
            /// langword="null"/>, the DIB is created in memory.</param>
            /// <param name="dwOffset">The offset from the beginning of the file
            /// mapping object referenced by <paramref name="hSection"/>. This
            /// value is ignored if <paramref name="hSection"/> is <see langword="null"/>.</param>
            /// <returns>A handle to the newly created DIB, or <see cref="IntPtr.Zero"/>
            /// if the function fails. Call <see
            /// cref="Marshal.GetLastWin32Error"/> to retrieve extended error information.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            public static extern IntPtr CreateDIBSection(
                IntPtr hdc,
                ref BITMAPINFO pbmi,
                uint iUsage,
                out IntPtr ppvBits,
                IntPtr hSection,
                uint dwOffset);

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

            /// <summary>
            /// Sets the viewport for rendering in OpenGL.
            /// </summary>
            /// <remarks>The viewport specifies the affine transformation of normalized device
            /// coordinates to window coordinates. It defines the rectangular region of the window where OpenGL will
            /// draw.  Typically, this method is called during initialization or when the window is resized.</remarks>
            /// <param name="x">The x-coordinate of the lower-left corner of the viewport, in pixels.</param>
            /// <param name="y">The y-coordinate of the lower-left corner of the viewport, in pixels.</param>
            /// <param name="width">The width of the viewport, in pixels. Must be greater than or equal to 0.</param>
            /// <param name="height">The height of the viewport, in pixels. Must be greater than or equal to 0.</param>
            [DllImport("opengl32.dll")]
            public static extern void glViewport(int x, int y, int width, int height);

            /// <summary>
            /// Exchanges the front and back buffers if the pixel format supports double buffering.
            /// </summary>
            /// <param name="hdc">Handle to the device context associated with the OpenGL rendering surface.</param>
            /// <returns>True if the operation succeeds; otherwise, false.</returns>
            [DllImport("gdi32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SwapBuffers(IntPtr hdc);

            /// <summary>
            /// Windows-specific: resolve OpenGL function pointers
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            [DllImport("opengl32.dll")]
            public static extern IntPtr wglGetProcAddress(string name);

            /// <summary>
            /// Retrieves a handle to the device context (DC) for the specified window or for the entire screen.
            /// </summary>
            /// <remarks>The device context can be used in subsequent GDI (Graphics Device Interface)
            /// operations.  After finishing with the device context, the caller must release it by calling the
            /// <c>ReleaseDC</c> function.</remarks>
            /// <param name="hwnd">A handle to the window whose device context is to be retrieved.
            /// If this parameter is <see
            /// cref="IntPtr.Zero"/>,  the device context for the entire screen is retrieved.</param>
            /// <returns>A handle to the device context for the specified window or screen.
            /// If the function fails, the return
            /// value is <see cref="IntPtr.Zero"/>.</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hwnd);

            /// <summary>
            /// Releases a device context (DC) for the specified window, freeing it for use by other applications.
            /// </summary>
            /// <remarks>This method is a wrapper for the native `ReleaseDC` function in the Windows
            /// API. It should be used to release a DC obtained using <see cref="GetDC"/> or similar methods.
            /// Failure to
            /// release a DC can result in resource leaks.</remarks>
            /// <param name="hwnd">A handle to the window whose DC is to be released. This parameter
            /// can be <see cref="IntPtr.Zero"/> if
            /// the DC is not associated with a specific window.</param>
            /// <param name="hdc">A handle to the device context to be released.</param>
            /// <returns>A nonzero value if the device context is released successfully; otherwise,
            /// <see cref="IntPtr.Zero"/> if
            /// the release fails.</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

            /// <summary>
            /// Chooses the pixel format that best matches the specified device context and pixel format descriptor.
            /// </summary>
            /// <remarks>This method is a wrapper for the native <c>ChoosePixelFormat</c> function in
            /// the GDI32 library.  The returned index can be used with other GDI functions to set or query the pixel
            /// format of the device context.</remarks>
            /// <param name="hdc">A handle to the device context for which the pixel format is to be selected.</param>
            /// <param name="pfd">A reference to a <see cref="PIXELFORMATDESCRIPTOR"/> structure that
            /// specifies the desired pixel format
            /// attributes.</param>
            /// <returns>The index of the pixel format that best matches the specified requirements,
            /// or 0 if the function fails.</returns>
            [DllImport("gdi32.dll")]
            public static extern int ChoosePixelFormat(IntPtr hdc, ref PIXELFORMATDESCRIPTOR pfd);

            /// <summary>
            /// Sets the pixel format for a device context to the specified format.
            /// </summary>
            /// <remarks>This method is a P/Invoke wrapper for the GDI `SetPixelFormat` function. The
            /// pixel format must be set before rendering to the device context. Once a pixel format is set,
            /// it cannot be changed for the lifetime of the device context.</remarks>
            /// <param name="hdc">A handle to the device context for which the pixel format is to be set.</param>
            /// <param name="format">The index of the pixel format to set. This value must correspond
            /// to a valid pixel format supported by
            /// the device context.</param>
            /// <param name="pfd">A reference to a <see cref="PIXELFORMATDESCRIPTOR"/> structure
            /// that defines the properties of the pixel
            /// format.</param>
            /// <returns><see langword="true"/> if the pixel format was successfully set; otherwise,
            /// <see langword="false"/>.</returns>
            [DllImport("gdi32.dll")]
            public static extern bool SetPixelFormat(IntPtr hdc, int format, ref PIXELFORMATDESCRIPTOR pfd);

            /// <summary>
            /// Creates a new OpenGL rendering context for the specified device context.
            /// </summary>
            /// <remarks>The created rendering context must be made current using
            /// <c>wglMakeCurrent</c> before it can be used for rendering. Ensure that the device context specified by
            /// <paramref name="hdc"/> is compatible with OpenGL.</remarks>
            /// <param name="hdc">A handle to a device context for which the OpenGL
            /// rendering context is created.</param>
            /// <returns>A handle to the created OpenGL rendering context if successful; otherwise,
            /// <see cref="IntPtr.Zero"/>.</returns>
            [DllImport("opengl32.dll")]
            public static extern IntPtr wglCreateContext(IntPtr hdc);

            /// <summary>
            /// Makes a specified OpenGL rendering context the current rendering context for the calling thread.
            /// </summary>
            /// <remarks>This method binds the specified OpenGL rendering context to the calling
            /// thread and the specified device context.  If <paramref name="hglrc"/> is <see langword="IntPtr.Zero"/>,
            /// the current rendering context is released from the calling thread. Only one rendering
            /// context can be
            /// current per thread at a time.</remarks>
            /// <param name="hdc">A handle to the device context (HDC) to which the OpenGL rendering context
            /// is to be attached. This
            /// parameter cannot be <see langword="IntPtr.Zero"/>.</param>
            /// <param name="hglrc">A handle to the OpenGL rendering context (HGLRC) to be made current.
            /// This parameter can be <see langword="IntPtr.Zero"/> to release the current rendering context.</param>
            /// <returns><see langword="true"/> if the operation succeeds;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport("opengl32.dll")]
            public static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);

            /// <summary>
            /// Deletes a specified OpenGL rendering context.
            /// </summary>
            /// <remarks>This method releases the resources associated with the specified OpenGL
            /// rendering context.  Ensure that the rendering context is no longer current in any thread before calling
            /// this method.</remarks>
            /// <param name="hglrc">A handle to the OpenGL rendering context to delete.
            /// This handle must not be null.</param>
            /// <returns><see langword="true"/> if the rendering context was successfully deleted;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport("opengl32.dll")]
            public static extern bool wglDeleteContext(IntPtr hglrc);

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
            /// Sets the visual style of a window by applying a theme from the current theme data.
            /// </summary>
            /// <remarks>This method is a wrapper for the native SetWindowTheme function in the
            /// uxtheme.dll library. It allows you to customize the appearance of a window
            /// by associating it with a
            /// specific theme.</remarks>
            /// <param name="hWnd">A handle to the window whose visual style is to be changed.</param>
            /// <param name="pszSubAppName">The application name to associate with the window.
            /// Pass <see langword="null"/> to remove any existing
            /// association.</param>
            /// <param name="pszSubIdList">The sub-identifier list to associate with the window.
            /// Pass <see langword="null"/> to remove any existing
            /// association.</param>
            /// <returns>Returns 0 if the operation succeeds; otherwise, returns a nonzero error code.
            /// For more information about
            /// error codes, see the Windows API documentation.</returns>
            [DllImport("uxtheme.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

            /// <summary>
            /// Loads the specified module into the address space of the calling process.
            /// </summary>
            /// <remarks>This method is a platform invocation (P/Invoke) wrapper for the Windows API
            /// function <c>LoadLibrary</c>. The caller is responsible for ensuring
            /// that the module is unloaded using
            /// the appropriate method (e.g., <c>FreeLibrary</c>) when it is no longer needed.</remarks>
            /// <param name="lpFileName">The name of the module to load.
            /// This can be either a full path or the name of a module in the system's
            /// search path.</param>
            /// <returns>A handle to the loaded module if the operation succeeds;
            /// otherwise, <see cref="IntPtr.Zero"/>.</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            /// <summary>
            /// Retrieves the address of an exported function or variable
            /// from the specified dynamic-link library (DLL).
            /// </summary>
            /// <remarks>This method is used to dynamically retrieve the address
            /// of a function or
            /// variable exported by a DLL.  Ensure that the module handle passed
            /// to <paramref name="hModule"/> is valid
            /// and that the function or variable name  specified in <paramref name="procName"/>
            /// exists in the module.
            /// </remarks>
            /// <param name="hModule">A handle to the DLL module that contains the function or variable.
            /// This handle must be obtained by
            /// calling <see cref="LoadLibrary"/> or a similar method.</param>
            /// <param name="procName">The name of the function or variable whose address
            /// is to be retrieved. Alternatively, this can be the
            /// ordinal value of the function, specified as a string.</param>
            /// <returns>A pointer to the function or variable if the operation succeeds;
            /// otherwise, <see cref="IntPtr.Zero"/>.</returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            /// <summary>
            /// Writes text to console.
            /// </summary>
            /// <param name="hConsoleOutput">The handle to the console output buffer.</param>
            /// <param name="lpBuffer">The buffer containing the characters to write to the console.</param>
            /// <param name="dwBufferSize">The size of the buffer, in characters.</param>
            /// <param name="dwBufferCoord">The coordinates of the buffer region to write to.</param>
            /// <param name="lpWriteRegion">The region of the console screen to write to.</param>
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

            /// <summary>
            /// Retrieves the full path of a known folder identified by its GUID.
            /// </summary>
            /// <remarks>This method is a P/Invoke declaration for the Windows API function
            /// SHGetKnownFolderPath. It is used to retrieve the path of a known folder, such as the Documents or
            /// Desktop folder. Ensure that the memory pointed to by <paramref name="ppszPath"/>
            /// is freed after use to
            /// avoid memory leaks.</remarks>
            /// <param name="rfid">The GUID of the known folder. This identifies the folder whose path
            /// is being retrieved.</param>
            /// <param name="dwFlags">Flags that specify special retrieval options.
            /// Typically set to 0 for default behavior.</param>
            /// <param name="hToken">An access token that represents a particular user.
            /// If this parameter is <see cref="IntPtr.Zero"/>, the
            /// function uses the access token of the calling process.</param>
            /// <param name="ppszPath">When the method returns, contains a pointer to the string
            /// that specifies the path of the known folder.
            /// The caller is responsible for freeing this memory using <see cref="Marshal.FreeCoTaskMem"/>.</param>
            /// <returns>Returns 0 if the function succeeds; otherwise, returns an error code.
            /// For a list of possible error
            /// codes, see the Windows API documentation.</returns>
            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            public static extern int SHGetKnownFolderPath(
                    [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
                    uint dwFlags,
                    IntPtr hToken,
                    out IntPtr ppszPath);

            /// <summary>
            /// Retrieves information about the specified window.
            /// </summary>
            /// <remarks>This method is a wrapper for the native <c>GetWindowInfo</c> function in the
            /// Windows API. It retrieves information such as the window's dimensions, style, and extended
            /// style.</remarks>
            /// <param name="hwnd">A handle to the window whose information is to be retrieved.</param>
            /// <param name="pwi">A reference to a <see cref="WindowInfo"/> structure that receives
            /// the window information. The caller
            /// must initialize the <c>cbSize</c> member of this structure to the size of the
            /// structure before calling
            /// this method.</param>
            /// <returns><see langword="true"/> if the function succeeds; otherwise,
            /// <see langword="false"/>. Call <see
            /// cref="Marshal.GetLastWin32Error"/> to retrieve extended error
            /// information if the function fails.</returns>
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(User32, SetLastError = true)]
            public static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo pwi);

            /// <summary>
            /// Refreshes the immersive color policy state used by the system theme (internal uxtheme call).
            /// </summary>
            /// <remarks>Invokes an undocumented entry in uxtheme.dll to refresh theme
            /// color policy state.</remarks>
            [DllImport(UxTheme, EntryPoint = "#104")]
            public static extern void RefreshImmersiveColorPolicyState();

            /// <summary>
            /// Enables or disables dark mode for a specific window
            /// using an undocumented uxtheme entry.
            /// </summary>
            /// <param name="window">A handle to the window for which dark mode should
            /// be allowed or disallowed.</param>
            /// <param name="isDarkModeAllowed">Pass <see langword="true"/> to allow
            /// dark mode for the window; otherwise <see langword="false"/>.</param>
            /// <returns><see langword="true"/> if the operation succeeds; otherwise,
            /// <see langword="false"/>.</returns>
            [DllImport(UxTheme, EntryPoint = "#133", SetLastError = true)]
            public static extern bool AllowDarkModeForWindow(IntPtr window, bool isDarkModeAllowed);

            /// <summary>
            /// Sets the preferred application theme mode (internal uxtheme API).
            /// </summary>
            /// <param name="preferredAppMode">The preferred theme mode
            /// to set for the application.</param>
            /// <returns><see langword="true"/> if the operation succeeds; otherwise,
            /// <see langword="false"/>.</returns>
            /// <remarks>Available in Windows 10 build 1903 (May 2019 Update) and later.</remarks>
            [DllImport(UxTheme, EntryPoint = "#135", SetLastError = true)]
            public static extern bool SetPreferredAppMode(AppThemeMode preferredAppMode);

            /// <summary>
            /// Allows or disallows dark mode for the entire application (internal uxtheme API).
            /// </summary>
            /// <param name="isDarkModeAllowed">Pass <see langword="true"/>
            /// to allow dark mode for the application; otherwise <see langword="false"/>.</param>
            /// <returns><see langword="true"/> if the operation succeeds;
            /// otherwise, <see langword="false"/>.</returns>
            /// <remarks>Available only in Windows 10 build 1809 (October 2018 Update).</remarks>
            [DllImport(UxTheme, EntryPoint = "#135", SetLastError = true)]
            public static extern bool AllowDarkModeForApp(bool isDarkModeAllowed);

            /// <summary>
            /// Determines whether dark mode is allowed for a specific window (internal uxtheme API).
            /// </summary>
            /// <param name="window">A handle to the window to check.</param>
            /// <returns><see langword="true"/> if dark mode is allowed for the window;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport(UxTheme, EntryPoint = "#137", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsDarkModeAllowedForWindow(IntPtr window);

            /// <summary>
            /// Determines whether dark mode is allowed for the current application (internal uxtheme API).
            /// </summary>
            /// <returns><see langword="true"/> if dark mode is allowed for the application;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport(UxTheme, EntryPoint = "#139", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsDarkModeAllowedForApp();

            /// <summary>
            /// Sets various window composition attributes (such as accent, cloak, etc.).
            /// </summary>
            /// <param name="window">A handle to the window whose composition attribute will
            /// be set.</param>
            /// <param name="windowCompositionAttribute">A reference to a
            /// <see cref="WindowCompositionAttributeData"/> structure that specifies the
            /// attribute and its value.</param>
            /// <returns><see langword="true"/> if the function succeeds; otherwise,
            /// <see langword="false"/>.</returns>
            [DllImport(User32, SetLastError = true)]
            public static extern bool SetWindowCompositionAttribute(
                IntPtr window,
                ref WindowCompositionAttributeData windowCompositionAttribute);

            /// <summary>
            /// Associates a string property with the specified window handle.
            /// </summary>
            /// <param name="window">A handle to the window.</param>
            /// <param name="propertyName">The name of the property to set.</param>
            /// <param name="propertyValue">A pointer-sized value to associate with
            /// the property name.</param>
            /// <returns><see langword="true"/> if the property is set successfully;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport(User32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool SetProp(
                IntPtr window,
                string propertyName,
                IntPtr propertyValue);

            /// <summary>
            /// Sets a Desktop Window Manager (DWM) window attribute by pointer.
            /// </summary>
            /// <param name="window">A handle to the window for which to set the attribute.</param>
            /// <param name="attribute">The DWM attribute to set.</param>
            /// <param name="valuePointer">A pointer to the value for the attribute.</param>
            /// <param name="valuePointerSize">The size, in bytes, of the value pointed to by
            /// <paramref name="valuePointer"/>.</param>
            /// <returns>An HRESULT-like integer result. Zero (S_OK) typically indicates success.</returns>
            [DllImport(DwmApi, SetLastError = false)]
            public static extern int DwmSetWindowAttribute(
                IntPtr window,
                DwmWindowAttribute attribute,
                IntPtr valuePointer,
                int valuePointerSize);

            /// <summary>
            /// Retrieves or sets various system-wide parameters.
            /// </summary>
            /// <param name="uiAction">The system-wide parameter to be retrieved or set.</param>
            /// <param name="uiParam">A parameter whose usage depends on the action specified
            /// by <paramref name="uiAction"/>.</param>
            /// <param name="callback">A reference to a <see cref="HighContrastData"/> structure
            /// or other data, depending on the action.</param>
            /// <param name="fwinini">Flags indicating whether to update user profile or broadcast setting changes.</param>
            /// <returns><see langword="true"/> if the call succeeds; otherwise, <see langword="false"/>.</returns>
            [DllImport(User32, CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern bool SystemParametersInfo(
                uint uiAction,
                uint uiParam,
                ref HighContrastData callback,
                uint fwinini);

            /// <summary>
            /// Sends the specified message to a window or windows.
            /// </summary>
            /// <param name="hWnd">A handle to the window whose window procedure will
            /// receive the message. If this parameter is <see cref="IntPtr.Zero"/>, the message
            /// is sent to all top-level windows in the system.</param>
            /// <param name="message">The message to be sent.</param>
            /// <param name="wParam">Additional message-specific information.</param>
            /// <param name="lParam">Additional message-specific information.</param>
            /// <returns>The result of the message processing; value depends on the message sent.</returns>
            [DllImport(User32, SetLastError = true)]
            public static extern uint SendMessage(
                IntPtr hWnd,
                uint message,
                IntPtr wParam,
                IntPtr lParam);

            /// <summary>
            /// Flushes menu themes, forcing theme changes to menus to be applied immediately.
            /// </summary>
            [DllImport(UxTheme, EntryPoint = "#136")]
            public static extern void FlushMenuThemes();

            /// <summary>
            /// Retrieves a handle to the foreground window (the window with which the user is
            /// currently working).
            /// </summary>
            /// <returns>A handle to the foreground window. If no foreground window exists,
            /// the return value is <see cref="IntPtr.Zero"/>.</returns>
            [DllImport(User32)]
            public static extern IntPtr GetForegroundWindow();

            /// <summary>
            /// Enumerates the modules (loaded DLLs) for the specified process.
            /// </summary>
            /// <param name="hProcess">A handle to the process whose modules are to be enumerated.</param>
            /// <param name="lphModule">An array that receives the module handles.</param>
            /// <param name="cb">The size, in bytes, of the array pointed to by
            /// <paramref name="lphModule"/>.</param>
            /// <param name="lpcbNeeded">Receives the number of bytes required
            /// to store all module handles or the number actually written.</param>
            /// <returns><see langword="true"/> if the function succeeds;
            /// otherwise, <see langword="false"/>.</returns>
            [DllImport("psapi.dll")]
            public static extern bool EnumProcessModules(
                IntPtr hProcess,
                IntPtr[] lphModule,
                int cb,
                out int lpcbNeeded);

            /// <summary>
            /// Retrieves the fully qualified path for the file containing the specified module.
            /// </summary>
            /// <param name="hProcess">A handle to the process that contains the module.</param>
            /// <param name="hModule">A handle to the module. If this parameter
            /// is <see cref="IntPtr.Zero"/>, the function returns the path of the executable
            /// file for the process
            /// specified in <paramref name="hProcess"/>.</param>
            /// <param name="lpFilename">A <see cref="StringBuilder"/> that receives
            /// the module path.</param>
            /// <param name="nSize">The size of the <paramref name="lpFilename"/> buffer,
            /// in characters.</param>
            /// <returns>The length, in characters, of the string copied to the buffer,
            /// or zero on failure.</returns>
            [DllImport("psapi.dll")]
            public static extern uint GetModuleFileNameEx(
                IntPtr hProcess,
                IntPtr hModule,
                StringBuilder lpFilename,
                uint nSize);

            /// <summary>
            /// Opens an existing process object and returns a handle that
            /// can be used to access the process.
            /// </summary>
            /// <param name="dwDesiredAccess">The access level requested for the process handle.</param>
            /// <param name="bInheritHandle">If <see langword="true"/>, processes created
            /// by this process will inherit the handle.</param>
            /// <param name="dwProcessId">The identifier of the local process to be opened.</param>
            /// <returns>A handle to the specified process, or <see cref="IntPtr.Zero"/> on failure.</returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(
                uint dwDesiredAccess,
                bool bInheritHandle,
                uint dwProcessId);

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

#pragma warning disable
            [StructLayout(LayoutKind.Sequential)]
            public struct BITMAPINFOHEADER
            {
                public uint biSize;
                public int biWidth;
                public int biHeight;
                public short biPlanes;
                public short biBitCount;
                public uint biCompression;
                public uint biSizeImage;
                public int biXPelsPerMeter;
                public int biYPelsPerMeter;
                public uint biClrUsed;
                public uint biClrImportant;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RGBQUAD
            {
                public byte rgbBlue;
                public byte rgbGreen;
                public byte rgbRed;
                public byte rgbReserved;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct BITMAPINFO
            {
                public BITMAPINFOHEADER bmiHeader;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
                public RGBQUAD[] bmiColors;
            }

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

#pragma warning disable
            [StructLayout(LayoutKind.Sequential)]
            public struct PIXELFORMATDESCRIPTOR
            {
                public ushort nSize;
                public ushort nVersion;
                public uint dwFlags;
                public byte iPixelType;
                public byte cColorBits;
                public byte cRedBits;
                public byte cRedShift;
                public byte cGreenBits;
                public byte cGreenShift;
                public byte cBlueBits;
                public byte cBlueShift;
                public byte cAlphaBits;
                public byte cAlphaShift;
                public byte cAccumBits;
                public byte cAccumRedBits;
                public byte cAccumGreenBits;
                public byte cAccumBlueBits;
                public byte cAccumAlphaBits;
                public byte cDepthBits;
                public byte cStencilBits;
                public byte cAuxBuffers;
                public byte iLayerType;
                public byte bReserved;
                public uint dwLayerMask;
                public uint dwVisibleMask;
                public uint dwDamageMask;
            }
#pragma warning restore

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

            /// <summary>
            /// Contains data used by <see cref="NativeMethods.SystemParametersInfo"/>
            /// for high contrast settings.
            /// </summary>
            /// <remarks>
            /// This structure maps to the native HIGHCONTRAST structure. The <see cref="size"/>
            /// field must be
            /// initialized to the size of this structure (in bytes) before calling native APIs.
            /// </remarks>
            [StructLayout(LayoutKind.Sequential)]
            public readonly struct HighContrastData
            {
                /// <summary>
                /// Size of this structure, in bytes. This is initialized in the constructor
                /// to <see cref="Marshal.SizeOf(Type)"/>.
                /// </summary>
                public readonly uint size;

                /// <summary>
                /// Flags that specify the high-contrast options. The meaning of the bits
                /// corresponds to the native HIGHCONTRAST flags.
                /// </summary>
                public readonly uint flags;

                /// <summary>
                /// Pointer to a null-terminated string that contains the name of the
                /// high-contrast color scheme.
                /// This is typically <see cref="IntPtr.Zero"/> when not used.
                /// </summary>
                public readonly IntPtr schemeNamePointer;

                /// <summary>
                /// Initializes a new instance of the <see cref="HighContrastData"/>
                /// structure with default values.
                /// </summary>
                /// <param name="_">Optional parameter to disambiguate constructor;
                /// ignored. Exists to keep structure readonly semantics simple.</param>
                public HighContrastData(object? _ = null)
                {
                    size = (uint)Marshal.SizeOf(typeof(HighContrastData));
                    flags = 0;
                    schemeNamePointer = IntPtr.Zero;
                }
            }

            /// <summary>
            /// Represents data used with <see cref="NativeMethods.SetWindowCompositionAttribute"/>
            /// to set a specific window composition attribute.
            /// </summary>
            /// <remarks>
            /// The <see cref="attribute"/> field specifies which composition attribute is being set.
            /// The <see cref="data"/> field is a pointer to the attribute value, and <see cref="size"/>
            /// is the size, in bytes, of that value.
            /// This structure should be marshaled exactly as defined when passed to native APIs.
            /// </remarks>
            [StructLayout(LayoutKind.Sequential)]
            public readonly struct WindowCompositionAttributeData
            {
                /// <summary>
                /// The composition attribute to get or set.
                /// </summary>
                public readonly WindowCompositionAttribute attribute;

                /// <summary>
                /// Pointer to the data for the attribute. The interpretation depends
                /// on <see cref="attribute"/>.
                /// </summary>
                public readonly IntPtr data;

                /// <summary>
                /// Size, in bytes, of the data pointed to by <see cref="data"/>.
                /// </summary>
                public readonly int size;

                /// <summary>
                /// Initializes a new instance of the
                /// <see cref="WindowCompositionAttributeData"/> structure.
                /// </summary>
                /// <param name="attribute">The window composition attribute to set.</param>
                /// <param name="data">A pointer to the attribute-specific data.</param>
                /// <param name="size">The size, in bytes, of the attribute data referenced
                /// by <paramref name="data"/>.</param>
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
