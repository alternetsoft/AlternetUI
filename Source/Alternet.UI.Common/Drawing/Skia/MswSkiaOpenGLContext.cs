using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an OpenGL rendering context for a Windows control,
    /// designed for use with SkiaSharp's GPU-based rendering.
    /// </summary>
    /// <remarks>This class is specific to Windows and provides an OpenGL rendering
    /// context that is compatible
    /// with SkiaSharp. It initializes an OpenGL context using the specified control's
    /// window handle and sets up a
    /// SkiaSharp <see cref="GRContext"/> for GPU-based rendering. The caller must ensure
    /// that the associated control
    /// remains valid for the lifetime of this object. <para> The class relies on native
    /// Win32 API calls to configure
    /// the OpenGL context and is not cross-platform. </para> <para> Proper disposal
    /// of this object is required to
    /// release native resources. Use the <see cref="IDisposable.Dispose"/> method
    /// or a `using` statement to ensure resources are cleaned up appropriately.</para></remarks>
    public class MswSkiaOpenGLContext : DisposableObject
    {
        private readonly IntPtr hwnd;
        private readonly IntPtr hdc;
        private readonly IntPtr contextPtr;
        private readonly GRContext grContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MswSkiaOpenGLContext"/> class,
        /// setting up an OpenGL rendering
        /// context for the specified Windows control.
        /// </summary>
        /// <remarks>This constructor configures the OpenGL pixel format and creates an OpenGL rendering
        /// context associated with the specified control. It uses the control's
        /// window handle to initialize the
        /// rendering context. The caller is responsible for ensuring that the control
        /// remains valid  during the
        /// lifetime of the OpenGL context. <para> The created OpenGL context is compatible
        /// with SkiaSharp's GPU-based
        /// rendering, and a <see cref="GRContext"/> is initialized internally to support
        /// SkiaSharp operations. </para>
        /// <para> This class is specific to Windows and relies on native Win32 API calls to
        /// configure the OpenGL
        /// context.</para></remarks>
        /// <param name="control">The <see cref="Control"/> for which the OpenGL rendering
        /// context will be created. This control must have a valid window handle.</param>
        public MswSkiaOpenGLContext(Control control)
        {
            hwnd = control.GetHandle();
            hdc = MswUtils.NativeMethods.GetDC(hwnd);

            MswPixelFormatDescriptorEnums.Flags flags =
                MswPixelFormatDescriptorEnums.Flags.DrawToWindow |
                MswPixelFormatDescriptorEnums.Flags.SupportOpenGl;

            MswPixelFormatDescriptorEnums.PixelType pixelType =
                MswPixelFormatDescriptorEnums.PixelType.Rgba;

            MswPixelFormatDescriptorEnums.PixelLayerType layerType =
                MswPixelFormatDescriptorEnums.PixelLayerType.MainPlane;

            var pfd = new MswUtils.NativeMethods.PIXELFORMATDESCRIPTOR
            {
                nSize = (ushort)Marshal.SizeOf<MswUtils.NativeMethods.PIXELFORMATDESCRIPTOR>(),
                nVersion = 1,
                dwFlags = (uint)flags,
                iPixelType = (byte)pixelType,
                cColorBits = 32,
                cDepthBits = 24,
                iLayerType = (byte)layerType,
            };

            int pixelFormat = MswUtils.NativeMethods.ChoosePixelFormat(hdc, ref pfd);
            MswUtils.NativeMethods.SetPixelFormat(hdc, pixelFormat, ref pfd);

            contextPtr = MswUtils.NativeMethods.wglCreateContext(hdc);
            MswUtils.NativeMethods.wglMakeCurrent(hdc, contextPtr);

            grContext = GRContext.CreateGl();
        }

        /// <summary>
        /// Gets the current graphics rendering context.
        /// </summary>
        public GRContext Context => grContext;

        /// <summary>
        /// Gets the window handle associated with the OpenGL context.
        /// </summary>
        public IntPtr Hwnd => hwnd;

        /// <summary>
        /// Gets the device context handle associated with the OpenGL context.
        /// </summary>
        public IntPtr Hdc => hdc;

        /// <summary>
        /// Gets the native OpenGL context pointer.
        /// </summary>
        public IntPtr ContextPtr => contextPtr;

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            MswUtils.NativeMethods.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            MswUtils.NativeMethods.wglDeleteContext(contextPtr);
            MswUtils.NativeMethods.ReleaseDC(hwnd, hdc);

            base.DisposeManaged();
        }
    }
}
