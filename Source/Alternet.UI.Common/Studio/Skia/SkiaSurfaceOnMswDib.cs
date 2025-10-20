using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using SkiaSharp;

#pragma warning disable
#if ALTERNETUI
namespace Alternet.Skia
#else
namespace Alternet.Common.Skia
#endif
#pragma warning restore
{
    /// <summary>
    /// Provides functionality for creating and managing a SkiaSharp
    /// <see cref="SKSurface"/> backed by a top-down Device
    /// Independent Bitmap (DIB) on Windows.
    /// </summary>
    /// <remarks>This class is designed to facilitate rendering with SkiaSharp
    /// on a Windows platform using a
    /// memory-based DIB. It provides methods to ensure the surface is properly initialized,
    /// render content to the
    /// surface, and transfer the rendered content to a target device context (HDC).
    /// <para> The surface is created as a
    /// 32-bit BGRA image with pre-multiplied alpha, and it is optimized for top-down memory layout,
    /// which is required by SkiaSharp. </para>
    /// <para> Call <see cref="EnsureSurface(int, int)"/> to initialize or resize the surface,
    /// <see cref="Paint(IntPtr, SKRectI, SKSizeI, SKColor, Action{SKSurface, SKRectI})"/>
    /// to render content and transfer it
    /// to a target HDC, and <see cref="DisposeSurface"/>
    /// to release resources when the surface is no longer needed.
    /// </para></remarks>
    public class SkiaSurfaceOnMswDib
    {
        private IntPtr memDC = IntPtr.Zero;
        private IntPtr hBitmap = IntPtr.Zero;
        private IntPtr oldBitmap = IntPtr.Zero;
        private IntPtr pixelPtr = IntPtr.Zero;
        private SKSurface? surface;
        private int surfWidth;
        private int surfHeight;
        private int rowBytes;
        private SKPaint? clearColorPaint;

        /// <summary>
        /// Renders content to the specified target device context within the given clipping region.
        /// </summary>
        /// <remarks>This method prepares a rendering surface, applies the specified clipping region,
        /// clears the region with the provided color if necessary, and invokes the
        /// <paramref name="drawAction"/> to
        /// perform custom drawing. After drawing, the rendered content
        /// is copied to the target device
        /// context.</remarks>
        /// <param name="hdcTarget">The handle to the device context (HDC) where the
        /// content will be rendered.</param>
        /// <param name="clip">The rectangular region of the target to which rendering
        /// is restricted. If empty, the entire client area is
        /// used.</param>
        /// <param name="clientSize">The size of the client area, in pixels,
        /// used to determine the rendering surface dimensions.</param>
        /// <param name="clearColor">The color used to clear the clipping region before drawing.
        /// If <see langword="SKColors.Transparent"/>, no
        /// clearing is performed.</param>
        /// <param name="drawAction">A callback action that performs the actual
        /// drawing on the rendering surface. The callback receives the
        /// <see cref="SKSurface"/> to draw on and the clipping region as a <see cref="SKRectI"/>.</param>
        public void Paint(
            IntPtr hdcTarget,
            SKRectI clip,
            SKSizeI clientSize,
            SKColor clearColor,
            Action<SKSurface, SKRectI> drawAction)
        {
            var w = clientSize.Width;
            var h = clientSize.Height;

            EnsureSurface(w, h);
            if (surface == null)
                return;

            var canvas = surface.Canvas;

            canvas.Save();

            if (clip.IsEmpty)
                clip = SKRectI.Create(0, 0, w, h);
            else
                canvas.ClipRect(new SKRect(clip.Left, clip.Top, clip.Right, clip.Bottom));

            if (clearColor != SKColors.Transparent)
            {
                // Clear only the clip (optional) or full:
                // canvas.Clear(new SKColor(0xFF, 0xFF, 0xFF, 0xFF));
                // If partial clear is needed:
                clearColorPaint ??= new SKPaint
                {
                    BlendMode = SKBlendMode.Src,
                };

                clearColorPaint.Color = clearColor;

                canvas.DrawRect(
                    new SKRect(clip.Left, clip.Top, clip.Right, clip.Bottom),
                    clearColorPaint);
            }

            drawAction(surface, clip);

            canvas.Restore();
            canvas.Flush();

            NativeMethods.BitBlt(
                hdcTarget,
                clip.Left,
                clip.Top,
                clip.Width,
                clip.Height,
                memDC,
                clip.Left,
                clip.Top,
                NativeMethods.SRCCOPY);
        }

        /// <summary>
        /// Ensures that a drawing surface with the specified dimensions is created and ready for use.
        /// </summary>
        /// <remarks>If a surface already exists with the specified dimensions, this method does nothing.
        /// If the dimensions differ or no surface exists, the current surface
        /// is disposed and a new one is created. The
        /// surface is configured as a 32-bit BGRA top-down DIB (Device Independent Bitmap)
        /// with pre-multiplied
        /// alpha.</remarks>
        /// <param name="width">The width of the surface, in pixels. Must be greater than 0.</param>
        /// <param name="height">The height of the surface, in pixels. Must be greater than 0.</param>
        /// <exception cref="Win32Exception">Thrown if a Win32 API call
        /// fails during the creation of the drawing surface.</exception>
        /// <exception cref="Exception">Thrown if the creation
        /// of the SkiaSharp surface fails.</exception>
        public void EnsureSurface(int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;

            if (surface != null && width == surfWidth && height == surfHeight)
                return;

            DisposeSurface();

            // 32bpp BGRA pre-multiplied
            surfWidth = width;
            surfHeight = height;
            rowBytes = width * 4;

            var bmi = new NativeMethods.BITMAPINFO
            {
                bmiHeader = new NativeMethods.BITMAPINFOHEADER
                {
                    biSize = (uint)Marshal.SizeOf<NativeMethods.BITMAPINFOHEADER>(),
                    biWidth = width,

                    // IMPORTANT: negative height => top-down DIB (Skia expects top-down)
                    biHeight = -height,
                    biPlanes = 1,
                    biBitCount = 32,
                    biCompression = NativeMethods.BI_RGB,
                    biSizeImage = (uint)(rowBytes * height),
                },
                bmiColors = new NativeMethods.RGBQUAD[256], // must be allocated even if not used
            };

            memDC = NativeMethods.CreateCompatibleDC(IntPtr.Zero);
            if (memDC == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateCompatibleDC failed.");

            hBitmap = NativeMethods.CreateDIBSection(
                memDC,
                ref bmi,
                NativeMethods.DIB_RGB_COLORS,
                out pixelPtr,
                IntPtr.Zero,
                0);

            if (hBitmap == IntPtr.Zero || pixelPtr == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateDIBSection failed.");

            oldBitmap = NativeMethods.SelectObject(memDC, hBitmap);
            if (oldBitmap == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "SelectObject failed.");

            var info = new SKImageInfo(
                width,
                height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

            surface = SKSurface.Create(info, pixelPtr, rowBytes);
            if (surface == null)
                throw new Exception("Failed to create SKSurface from DIB.");
        }

        /// <summary>
        /// Releases all resources associated with the surface and resets related state.
        /// </summary>
        /// <remarks>This method disposes of the underlying surface and cleans up
        /// associated unmanaged
        /// resources,  such as device contexts and bitmaps. After calling this method,
        /// the surface and related
        /// resources  are no longer usable. Ensure that no further operations are performed
        /// on the surface after
        /// disposal.</remarks>
        public void DisposeSurface()
        {
            surface?.Dispose();
            surface = null;

            if (memDC != IntPtr.Zero)
            {
                if (oldBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDC, oldBitmap);
                    oldBitmap = IntPtr.Zero;
                }
            }

            if (hBitmap != IntPtr.Zero)
            {
                NativeMethods.DeleteObject(hBitmap);
                hBitmap = IntPtr.Zero;
            }

            if (memDC != IntPtr.Zero)
            {
                NativeMethods.DeleteDC(memDC);
                memDC = IntPtr.Zero;
            }

            pixelPtr = IntPtr.Zero;
            surfWidth = surfHeight = 0;
        }

        private class NativeMethods
        {
#pragma warning disable
            public const int BI_RGB = 0;

            public const int DIB_RGB_COLORS = 0;

            public const int SRCCOPY = 0x00CC0020;
#pragma warning restore

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
        }
    }
}
