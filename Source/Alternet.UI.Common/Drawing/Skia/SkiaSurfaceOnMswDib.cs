using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
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
        /// <see cref="SKSurface"/> to draw on and the clipping region as a <see cref="RectI"/>.</param>
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

            MswUtils.NativeMethods.BitBlt(
                hdcTarget,
                clip.Left,
                clip.Top,
                clip.Width,
                clip.Height,
                memDC,
                clip.Left,
                clip.Top,
                MswUtils.NativeMethods.SRCCOPY);
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

            var bmi = new MswUtils.NativeMethods.BITMAPINFO
            {
                bmiHeader = new MswUtils.NativeMethods.BITMAPINFOHEADER
                {
                    biSize = (uint)Marshal.SizeOf<MswUtils.NativeMethods.BITMAPINFOHEADER>(),
                    biWidth = width,

                    // IMPORTANT: negative height => top-down DIB (Skia expects top-down)
                    biHeight = -height,
                    biPlanes = 1,
                    biBitCount = 32,
                    biCompression = MswUtils.NativeMethods.BI_RGB,
                    biSizeImage = (uint)(rowBytes * height),
                },
                bmiColors = new MswUtils.NativeMethods.RGBQUAD[256], // must be allocated even if not used
            };

            memDC = MswUtils.NativeMethods.CreateCompatibleDC(IntPtr.Zero);
            if (memDC == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateCompatibleDC failed.");

            hBitmap = MswUtils.NativeMethods.CreateDIBSection(
                memDC,
                ref bmi,
                MswUtils.NativeMethods.DIB_RGB_COLORS,
                out pixelPtr,
                IntPtr.Zero,
                0);

            if (hBitmap == IntPtr.Zero || pixelPtr == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateDIBSection failed.");

            oldBitmap = MswUtils.NativeMethods.SelectObject(memDC, hBitmap);
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
                    MswUtils.NativeMethods.SelectObject(memDC, oldBitmap);
                    oldBitmap = IntPtr.Zero;
                }
            }

            if (hBitmap != IntPtr.Zero)
            {
                MswUtils.NativeMethods.DeleteObject(hBitmap);
                hBitmap = IntPtr.Zero;
            }

            if (memDC != IntPtr.Zero)
            {
                MswUtils.NativeMethods.DeleteDC(memDC);
                memDC = IntPtr.Zero;
            }

            pixelPtr = IntPtr.Zero;
            surfWidth = surfHeight = 0;
        }
    }
}
