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
        internal static bool DrawGLTextAtCorner = DebugUtils.IsDebugDefined && false;

        private bool firstPaintDone;
        private GRGlFramebufferInfo? glInfo;
        private GRBackendRenderTarget? renderTarget;
        private GRGlInterface? glInterface;
        private GRContext? grContext;
        private IntPtr? lastUsedGLContext;

        public void ResetGLResources()
        {
            glInfo = null;
            SafeDispose(ref renderTarget);
            SafeDispose(ref glInterface);
            SafeDispose(ref grContext);
            SafeDispose(ref renderTarget);
            lastUsedGLContext = null;
        }

        public override void OnPlatformEventPaint()
        {
            if (!NeedUserPaint())
                return;

            var openGL = UIControl!.RenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharpWithOpenGL);

            if (openGL && Drawing.GraphicsFactory.IsOpenGLAvailable)
            {
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
                var newContext = GetGLContext();
                var contextChanged = newContext != lastUsedGLContext;
                lastUsedGLContext = newContext;

                // Step 1: Create the GL interface and context

                if (contextChanged || glInterface is null || grContext is null)
                {
                    ResetGLResources();
                }

                if (glInterface is null || grContext is null)
                {
                    glInterface = GRGlInterface.Create();
                    if (glInterface == null)
                    {
                        Debug.WriteLine("[Error] GRGlInterface.Create() returned null");
                        return false;
                    }

                    grContext = GRContext.CreateGl(glInterface);
                    if (grContext == null)
                    {
                        Debug.WriteLine("[Error] GRContext.CreateGl() returned null");
                        return false;
                    }
                }
                else
                {
                    grContext.ResetContext();
                }


                // Step 2: Define framebuffer info
                glInfo ??= new GRGlFramebufferInfo
                    {
                        FramebufferObjectId = 0,
                        Format = 0x8058, // GL_RGBA8
                    };

                var size = this.ViewportSize;

                if (renderTarget is null || renderTarget.Width != size.Width
                    || renderTarget.Height != size.Height)
                {
                    renderTarget?.Dispose();
                    renderTarget = new GRBackendRenderTarget(
                        width: size.Width,
                        height: size.Height,
                        sampleCount: 0,     // No multisampling
                        stencilBits: 8,     // Typical stencil buffer size
                        glInfo: glInfo.Value
                    );
                }

                // Step 4: Create the Skia surface
                using var surface = SKSurface.Create(
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


                var e = new PaintEventArgs(() => graphics, clientRect, clientRect);
                uiControl.RaisePaint(e);

                if(DrawGLTextAtCorner)
                    DrawingUtils.DrawDebugTextAtCorner(graphics, "GL", clientRect);

                canvas.Flush();

                return true;
            }
        }
    }
}
