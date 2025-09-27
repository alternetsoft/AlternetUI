using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using SkiaSharp;

#pragma warning disable CA2101
#pragma warning disable CS1591

namespace Alternet.UI
{
    public static partial class LinuxUtils
    {
        private static bool? isCairoSupported;

        /// <summary>
        /// Contains P/Invoke signatures for the native Cairo graphics library used on Linux.
        /// </summary>
        public static class CairoNative
        {
            /// <summary>
            /// Native library name used for DllImport attributes.
            /// </summary>
            /// <remarks>
            /// This should match the installed libcairo shared object on the target system.
            /// </remarks>
            private const string LIB_CAIRO = "libcairo.so.2";

            /// <summary>
            /// Represents the native constant <c>CAIRO_FORMAT_ARGB32</c> (value 0).
            /// </summary>
            /// <remarks>
            /// Pixel format is 32-bit ARGB pre-multiplied (native endianness).
            /// </remarks>
            public const int CAIRO_FORMAT_ARGB32 = 0;

            /// <summary>
            /// Gets the width, in pixels, of the specified Cairo image surface.
            /// </summary>
            /// <param name="surface">Pointer to a <c>cairo_surface_t</c>
            /// representing an image surface.</param>
            /// <returns>The width of the image surface in pixels.
            /// If <paramref name="surface"/> is invalid, behavior is undefined.</returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern int cairo_image_surface_get_width(IntPtr surface);

            /// <summary>
            /// Gets the height, in pixels, of the specified Cairo image surface.
            /// </summary>
            /// <param name="surface">Pointer to a <c>cairo_surface_t</c>
            /// representing an image surface.</param>
            /// <returns>The height of the image surface in pixels.
            /// If <paramref name="surface"/> is invalid, behavior is undefined.</returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern int cairo_image_surface_get_height(IntPtr surface);

            /// <summary>
            /// Returns the target surface associated with the given Cairo drawing context.
            /// </summary>
            /// <param name="cr">Pointer to a <c>cairo_t</c> drawing context.</param>
            /// <returns>
            /// Pointer to the <c>cairo_surface_t</c> that is the
            /// current target for <paramref name="cr"/>.
            /// Returns <c>IntPtr.Zero</c> if no target is set or on failure.
            /// </returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr cairo_get_target(IntPtr cr);

            /// <summary>
            /// Creates an image surface that uses the provided pixel buffer as its backing store.
            /// </summary>
            /// <param name="data">Pointer to the pixel buffer (byte*).
            /// The buffer must remain valid for the lifetime of the surface.</param>
            /// <param name="format">Cairo pixel format (e.g., <see cref="CAIRO_FORMAT_ARGB32"/>).</param>
            /// <param name="width">Width of the surface in pixels.</param>
            /// <param name="height">Height of the surface in pixels.</param>
            /// <param name="stride">Number of bytes between the start of rows in the buffer.</param>
            /// <returns>
            /// A pointer to a newly created <c>cairo_surface_t</c>. Returns <c>IntPtr.Zero</c> on failure.
            /// The caller is responsible for calling <see cref="cairo_surface_destroy"/> when finished.
            /// </returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr cairo_image_surface_create_for_data(
                IntPtr data,
                int format,
                int width,
                int height,
                int stride
            );

            /// <summary>
            /// Sets the specified surface as the source pattern for the given Cairo context at (x, y).
            /// </summary>
            /// <param name="cr">Pointer to a <c>cairo_t</c> context.</param>
            /// <param name="surface">Pointer to a <c>cairo_surface_t</c> to use as source.</param>
            /// <param name="x">X offset to place the surface.</param>
            /// <param name="y">Y offset to place the surface.</param>
            /// <remarks>
            /// After calling this, drawing operations on <paramref name="cr"/> will
            /// use the provided surface as the source.
            /// </remarks>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern void cairo_set_source_surface(
                IntPtr cr,
                IntPtr surface,
                double x,
                double y
            );

            /// <summary>
            /// Paints the current source everywhere within the current
            /// clip region of the given Cairo context.
            /// </summary>
            /// <param name="cr">Pointer to a <c>cairo_t</c> context.</param>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern void cairo_paint(IntPtr cr);

            /// <summary>
            /// Retrieves the version number of the Cairo graphics library.
            /// </summary>
            /// <remarks>This method provides the version of the Cairo library linked at runtime.  The
            /// version number can be used to verify compatibility or enable features based
            /// on the library
            /// version.</remarks>
            /// <returns>An integer representing the version of the Cairo library,
            /// encoded as a single number. The version is
            /// typically represented as (major * 10000 + minor * 100 + micro).</returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern int cairo_version();

            /// <summary>
            /// Destroys a Cairo surface and frees associated resources.
            /// </summary>
            /// <param name="surface">Pointer to a <c>cairo_surface_t</c> to destroy.</param>
            /// <remarks>
            /// After calling this, the surface pointer must not be used.
            /// </remarks>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern void cairo_surface_destroy(IntPtr surface);

            /// <summary>
            /// Destroys a Cairo drawing context (<c>cairo_t</c>) and frees associated resources.
            /// </summary>
            /// <param name="cr">Pointer to the <c>cairo_t</c> to destroy.</param>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern void cairo_destroy(IntPtr cr);

            /// <summary>
            /// Creates a new <c>cairo_t</c> drawing context for the given target surface.
            /// </summary>
            /// <param name="target">Pointer to the target surface (for example, a GDK/GTK surface).</param>
            /// <returns>
            /// Pointer to the newly created <c>cairo_t</c>. Returns <c>IntPtr.Zero</c> on failure.
            /// The caller is responsible for calling <see cref="cairo_destroy"/> when finished.
            /// </returns>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr cairo_create(IntPtr target);

            /// <summary>
            /// Flushes any pending drawing operations for the specified
            /// surface and synchronizes the surface contents with its memory buffer.
            /// </summary>
            /// <param name="surface">Pointer to the <c>cairo_surface_t</c> to flush.</param>
            /// <remarks>
            /// Useful when the surface's backing store is shared with native
            /// code and explicit synchronization is required.
            /// </remarks>
            [DllImport(LIB_CAIRO, CallingConvention = CallingConvention.Cdecl)]
            public static extern void cairo_surface_flush(IntPtr surface);
        }

        /// <summary>
        /// Gets the Cairo library version as a .NET Version struct.
        /// </summary>
        public static Version GetCairoVersion()
        {
            try
            {
                int version = CairoNative.cairo_version();
                int major = version / 10000;
                int minor = (version / 100) % 100;
                int build = version % 100;
                return new Version(major, minor, build);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Error] Failed to get Cairo version: {ex.Message}");
                return new Version(0, 0, 0);
            }
        }

        /// <summary>
        /// Checks if the Cairo library version is 1.10.0 or higher.
        /// </summary>
        public static bool IsCairoVersionSupported()
        {
            Version caVersion = GetCairoVersion();
            Version minRequired = new(1, 10, 0);
            return caVersion >= minRequired;
        }

        /// <summary>
        /// Checks if the Cairo library and key functions are available.
        /// Use this at startup to ensure your application will run correctly.
        /// </summary>
        public static bool IsCairoSupported()
        {
            var result = isCairoSupported ??= Internal();
            return result;

            bool Internal()
            {
                try
                {
                    var result = IsCairoVersionSupported();

                    if (!result)
                        return false;

                    // Attempt to call a basic Cairo function as a test
                    IntPtr dummy = CairoNative.cairo_image_surface_create_for_data(IntPtr.Zero, 0, 1, 1, 4);
                    CairoNative.cairo_surface_destroy(dummy);
                    // Add more function calls if needed
                    return true;
                }
                catch (DllNotFoundException)
                {
                    Debug.WriteLine("[Error] Cairo library not found.");
                    return false;
                }
                catch (EntryPointNotFoundException ex)
                {
                    Debug.WriteLine($"[Error] Cairo function missing: {ex.Message}. The library version may be outdated or incompatible.");
                    return false;
                }
            }
        }

        /// <summary>
        /// Paints the SkiaSharp buffer onto the given Cairo drawing context.
        /// </summary>
        /// <param name="cr">
        /// A pointer to the native Cairo context (cairo_t*) provided by the GTK paint event.
        /// This parameter represents the drawing context for the target GTK surface.
        /// </param>
        /// <param name="drawAction">The action to perform on the SkiaSharp surface.</param>
        public static bool DrawOnCairo(IntPtr cr, Action<SKSurface>? drawAction)
        {
            if(!App.IsLinuxOS || !IsCairoSupported())
            {
                Debug.WriteLine("[Error] Cairo is not supported on this system.");
                return false;
            }

            IntPtr surface = CairoNative.cairo_get_target(cr);
            int width = CairoNative.cairo_image_surface_get_width(surface);
            int height = CairoNative.cairo_image_surface_get_height(surface);

            if (width <= 0 || height <= 0)
                return false;

            var info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
            int stride = info.RowBytes;
            byte[] buffer = new byte[height * stride];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pixels = handle.AddrOfPinnedObject();

            using (var skiaSurface = SKSurface.Create(info, pixels, stride))
            {
                if(skiaSurface is null)
                {
                    handle.Free();
                    Debug.WriteLine("[Error] Failed to create SkiaSharp surface over Cairo.");
                    return false;
                }

                if (drawAction is null)
                {
                    var canvas = skiaSurface.Canvas;
                    canvas.Clear(SKColors.Yellow);
                    canvas.DrawRect(
                        new SKRect(10, 10, 120, 80),
                        new SKPaint { Color = SKColors.Red, IsAntialias = true });
#pragma warning disable
                    canvas.DrawText(
                        "Hello SkiaSharp!",
                        20,
                        60,
                        new SKPaint { Color = SKColors.Blue, TextSize = 24 });
#pragma warning restore
                    skiaSurface.Flush();
                }
                else
                {
                    drawAction(skiaSurface);
                }
            }

            IntPtr caSurface = CairoNative.cairo_image_surface_create_for_data(
                pixels, CairoNative.CAIRO_FORMAT_ARGB32, width, height, stride);
            if (caSurface == IntPtr.Zero)
            {
                handle.Free();
                Debug.WriteLine("[Error] Failed to create Cairo surface.");
                return false;
            }

            CairoNative.cairo_set_source_surface(cr, caSurface, 0, 0);
            CairoNative.cairo_paint(cr);
            CairoNative.cairo_surface_destroy(caSurface);
            handle.Free();
            return true;
        }
    }
}
