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
    /// <remarks>This class is specific to Windows and provides an OpenGL rendering context that is compatible
    /// with SkiaSharp. It initializes an OpenGL context using the specified control's window handle and sets up a
    /// SkiaSharp <see cref="GRContext"/> for GPU-based rendering. The caller must ensure that the associated control
    /// remains valid for the lifetime of this object. <para> The class relies on native Win32 API calls to configure
    /// the OpenGL context and is not cross-platform. </para> <para> Proper disposal of this object is required to
    /// release native resources. Use the <see cref="IDisposable.Dispose"/> method
    /// or a `using` statement to ensure resources are cleaned up appropriately.</para></remarks>
    public class SkiaOpenGLContextMsw_ : DisposableObject
    {
        private readonly IntPtr hwnd;
        private readonly IntPtr hdc;
        private readonly IntPtr contextPtr;
        private readonly GRContext grContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaOpenGLContextMsw_"/> class,
        /// setting up an OpenGL rendering
        /// context for the specified Windows control.
        /// </summary>
        /// <remarks>This constructor configures the OpenGL pixel format and creates an OpenGL rendering
        /// context associated with the specified control. It uses the control's window handle to initialize the
        /// rendering context. The caller is responsible for ensuring that the control remains valid  during the
        /// lifetime of the OpenGL context. <para> The created OpenGL context is compatible with SkiaSharp's GPU-based
        /// rendering, and a <see cref="GRContext"/> is initialized internally to support SkiaSharp operations. </para>
        /// <para> This class is specific to Windows and relies on native Win32 API calls to configure the OpenGL
        /// context.</para></remarks>
        /// <param name="control">The <see cref="Control"/> for which the OpenGL rendering
        /// context will be created. This control must have a valid window handle.</param>
        public SkiaOpenGLContextMsw_(Control control)
        {
            hwnd = control.GetHandle();
            hdc = MswUtils.NativeMethods.GetDC(hwnd);

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

            contextPtr = MswUtils.NativeMethods.wglCreateContext(hdc);
            MswUtils.NativeMethods.wglMakeCurrent(hdc, contextPtr);

            grContext = GRContext.CreateGl();
        }

        /// <summary>
        /// Gets the current graphics rendering context.
        /// </summary>
        public GRContext Context => grContext;

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
