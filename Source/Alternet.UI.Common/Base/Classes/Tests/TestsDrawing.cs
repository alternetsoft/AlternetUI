using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Provides test methods related to drawing.
    /// </summary>
    /// <remarks>This class is intended for use in debugging scenarios and contains methods that are
    /// conditionally compiled based on the presence of the <c>DEBUG</c> compilation symbol.</remarks>
    public static class TestsDrawing
    {
        /// <summary>
        /// Creates an offscreen rendering context using OpenGL and renders a sample graphic.
        /// </summary>
        /// <remarks>This method initializes an invisible form to create an offscreen rendering context.
        /// It sets up the necessary OpenGL environment, including a pixel format and rendering context,
        /// and uses it to render a sample graphic.
        /// The method ensures that all resources, including  the rendering context and device
        /// context, are properly released after rendering. This method is primarily intended for testing or
        /// demonstration purposes where rendering  output does not need to be displayed on screen.</remarks>
        public static void TestRenderOffscreen()
        {
            if(!App.IsWindowsOS)
                return;

            var form = new Form
            {
                Width = 1,
                Height = 1,
                ShowInTaskbar = false,
                HasBorder = false,
                StartLocation = WindowStartLocation.Manual,
                Location = new PointD(-32000, -32000),
            };

            form.HandleNeeded();

            IntPtr hwnd = form.GetHandle();
            IntPtr hdc = MswUtils.NativeMethods.GetDC(hwnd);

            var pfd = new MswUtils.NativeMethods.PIXELFORMATDESCRIPTOR
            {
                nSize = (ushort)Marshal.SizeOf<MswUtils.NativeMethods.PIXELFORMATDESCRIPTOR>(),
                nVersion = 1,
                dwFlags = 0x00000004 | 0x00000020, // PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL
                iPixelType = 0, // PFD_TYPE_RGBA
                cColorBits = 32,
                cDepthBits = 24,
                iLayerType = 0, // PFD_MAIN_PLANE
            };

            int pixelFormat = MswUtils.NativeMethods.ChoosePixelFormat(hdc, ref pfd);
            MswUtils.NativeMethods.SetPixelFormat(hdc, pixelFormat, ref pfd);

            IntPtr hglrc = MswUtils.NativeMethods.wglCreateContext(hdc);
            MswUtils.NativeMethods.wglMakeCurrent(hdc, hglrc);

            var grContext = GRContext.CreateGl();

            DrawSampleOnContext(grContext);

            MswUtils.NativeMethods.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            MswUtils.NativeMethods.wglDeleteContext(hglrc);
            MswUtils.NativeMethods.ReleaseDC(hwnd, hdc);
            form.Dispose();
        }

        /// <summary>
        /// Renders a sample frame on the specified Skia graphics context for debugging purposes.
        /// </summary>
        /// <remarks>This method is only executed in debug builds, as it is marked with the
        /// <see cref="ConditionalAttribute"/> for the "DEBUG" symbol. It creates a 500x500 surface,
        /// renders a sample frame with a black background and red text, and saves the rendered frame
        /// as an image file to the desktop.</remarks>
        /// <param name="context">The Skia graphics context (<see cref="GRContext"/>) on which the
        /// sample frame will be drawn.</param>
        [Conditional("DEBUG")]
        public static void DrawSampleOnContext(GRContext context)
        {
            var info = new SKImageInfo(500, 500);
            using var surface = SKSurface.Create(context, true, info);
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.Black);
            canvas.DrawText("Frame rendered", 50, 100, SKTextAlign.Left, Control.DefaultFont, Brushes.Red);
            canvas.Flush();
            SkiaUtils.SaveSurfaceToFile(
                surface,
                Path.Combine(PathUtils.GetDesktopPath(), "frame_rendered.png"));
        }
    }
}
