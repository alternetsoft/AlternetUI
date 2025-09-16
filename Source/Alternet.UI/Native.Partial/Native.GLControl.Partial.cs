using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
using SkiaSharp;
using System.Diagnostics;
namespace Alternet.UI.Native
{
    internal partial class GLControl
    {
        private bool openGLPaintLogged;
        private bool firstPaintDone;

        public override void OnPlatformEventPaint()
        {
            if (!NeedUserPaint())
                return;

            var openGL = UIControl!.RenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharpWithOpenGL);

            if (openGL && Drawing.GraphicsFactory.IsOpenGLAvailable)
            {
                if (DebugUtils.IsDebugDefinedAndAttached && !openGLPaintLogged)
                {
                    openGLPaintLogged = true;
                    System.Diagnostics.Debug.WriteLine("GLControl: OpenGLPaint");
                }

                if (firstPaintDone || !App.IsWindowsOS)
                {
                    if (!OpenGLPaint())
                    {
                        DefaultPaintUsed = true;

                        if(App.IsWindowsOS)
                            SkiaPaint();
                        else
                            DefaultPaint();

                        UIControl.Refresh();
                    }
                }
                else
                {
                    DefaultPaintUsed = true;

                    if (App.IsWindowsOS)
                        SkiaPaint();
                    else
                        DefaultPaint();

                    firstPaintDone = true;
                }
            }
            else
                base.OnPlatformEventPaint();
        }

        bool OpenGLPaint()
        {
            var uiControl = UIControl!;

            KnownRunTimeTrackers.OpenGLPaintStart(uiControl);

            try
            {
                return InternalPaint();
            }
            finally
            {
                KnownRunTimeTrackers.OpenGLPaintStop(uiControl);
            }

            bool InternalPaint()
            {
                // Step 1: Create the GL interface and context

                var glInterface = GRGlInterface.Create();
                if (glInterface == null)
                {
                    Debug.WriteLine("[Error] GRGlInterface.Create() returned null");
                    return false;
                }

                var grContext = GRContext.CreateGl(glInterface);
                if (grContext == null)
                {
                    Debug.WriteLine("[Error] GRContext.CreateGl() returned null");
                    return false;
                }

                // Step 2: Define framebuffer info
                var glInfo = new GRGlFramebufferInfo
                {
                    FramebufferObjectId = 0,
                    Format = 0x8058, // GL_RGBA8
                };

                var size = this.ViewportSize;

                // Step 3: Create the backend render target
                var renderTarget = new GRBackendRenderTarget(
                    width: size.Width,
                    height: size.Height,
                    sampleCount: 0,     // No multisampling
                    stencilBits: 8,     // Typical stencil buffer size
                    glInfo: glInfo
                );

                // Step 4: Create the Skia surface
                var surface = SKSurface.Create(
                    grContext,
                    renderTarget,
                    GRSurfaceOrigin.BottomLeft, // OpenGL convention
                    SKColorType.Rgba8888        // Matches the format above
                );

                if (surface == null)
                {
                    Debug.WriteLine("[Error] SKSurface.Create(...) returned null");
                    return false;
                }

                // Step 5: Draw something
                var canvas = surface.Canvas;

                var scaleFactor = uiControl.ScaleFactor;
                var clientRect = uiControl.ClientRectangle;

                using var graphics = Drawing.SkiaUtils.CreateSkiaGraphicsOnCanvas(canvas, (float)scaleFactor);


                var e = new PaintEventArgs(() => graphics, clientRect);
                uiControl.RaisePaint(e);

                canvas.Flush();

                return true;
            }
        }
    }
}
