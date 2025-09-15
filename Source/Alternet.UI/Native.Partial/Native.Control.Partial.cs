using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Control
    {
        Drawing.DynamicBitmap? dynamicBitmap;

        public AbstractControl? EventUIFocusedControl
        {
            get
            {
                return WxApplicationHandler.FromNativeControl(EventFocusedControl);
            }
        }

        public WxControlHandler? Handler
        {
            get
            {
                return handler as WxControlHandler;
            }
        }

        public UI.Control? UIControl
        {
            get
            {
                var result = Handler?.Control;
                if (result?.DisposingOrDisposed ?? true)
                    return null;
                return result;
            }
        }

        public void OnPlatformEventIdle()
        {
        }

        public virtual void OnPlatformEventPaint()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;

            if (!uiControl.UserPaint)
                return;

            var clientRect = uiControl.ClientRectangle;
            if (clientRect.SizeIsEmpty)
                return;
            if (!uiControl.VisibleOnScreen)
                return;

            var skia = !App.IsMacOS &&
                uiControl.RenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp);

            Drawing.Graphics CreateDefaultGraphics()
            {
                return Handler?.OpenPaintDrawingContext() ?? Alternet.Drawing.PlessGraphics.Default;
            }

            void DefaultPaint()
            {
                var e = new PaintEventArgs(CreateDefaultGraphics, clientRect);

                try
                {
                    uiControl.RaisePaint(e);
                }
                finally
                {
                    if (e.GraphicsAllocated)
                    {
                        e.Graphics.Dispose();
                    }
                }
            }

            void OpenGLPaint()
            {
                using var glContext = new Drawing.MswSkiaOpenGLContext(uiControl);
                var context = glContext.Context;

                TestsDrawing.DrawSampleOnContext(context, false);
                /*
                MswUtils.NativeMethods.SwapBuffers(glContext.Hdc);
                var e = new PaintEventArgs(() => glContext.CreateSkiaGraphics(), clientRect);
                uiControl.RaisePaint(e);
                */
            }

            void SkiaPaint()
            {
                var scaleFactor = uiControl.ScaleFactor;
                Drawing.DynamicBitmap.CreateOrUpdate(
                    ref dynamicBitmap,
                    clientRect.Size,
                    scaleFactor,
                    isTransparent: true);

                var bitmap = dynamicBitmap.Bitmap;

                var canvasLock = bitmap.LockSurface(Drawing.ImageLockMode.WriteOnly);

                var canvas = canvasLock.Canvas;

                using var graphics = new Drawing.SkiaGraphics(canvas);

                graphics.OriginalScaleFactor = (float)scaleFactor;
                graphics.UseUnscaledDrawImage = true;
                graphics.InitialMatrix = canvas.TotalMatrix;

                canvas.Scale((float)scaleFactor);

                var e = new PaintEventArgs(() => graphics, clientRect);
                uiControl.RaisePaint(e);

                canvas.Flush();

                canvasLock.Dispose();

                using var dc = CreateDefaultGraphics();

                dc.DrawImage(bitmap, clientRect.Location);
            }

            if (skia)
            {
                if (DebugUtils.IsDebugDefinedAndAttached && App.IsWindowsOS
                    && uiControl.RenderingFlags.HasFlag(ControlRenderingFlags.UseOpenGL))
                    OpenGLPaint();
                else
                    SkiaPaint();
            }
            else
            {
                DefaultPaint();
            }
        }

        public virtual void OnPlatformEventMouseEnter()
        {
            UIControl?.RaiseMouseEnterOnTarget(EventArgs.Empty);
        }

        public virtual void OnPlatformEventMouseLeave()
        {
            UIControl?.RaiseMouseLeaveOnTarget(EventArgs.Empty);
        }

        public virtual void OnPlatformEventMouseClick()
        {
        }

        public virtual void OnPlatformEventVisibleChanged()
        {
            bool visible = Visible;

            if (App.IsLinuxOS && visible)
            {
                // todo: this is a workaround for a problem on Linux when
                // ClientSize is not reported correctly until the window is shown
                // So we need to perform layout after the proper client size is available
                // This should be changed later in respect to RedrawOnResize functionality.
                // Also we may need to do this for top-level windows.
                // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                UIControl?.PerformLayout();
            }
        }

        public virtual void OnPlatformEventMouseCaptureLost()
        {
            UIControl?.RaiseMouseCaptureLost(EventArgs.Empty);
        }

        public virtual void OnPlatformEventRequestCursor()
        {
            var uiControl = UIControl;
            if (uiControl is null)
                return;
        }

        public virtual void OnPlatformEventDpiChanged()
        {
            var uiControl = UIControl;
            if (uiControl is null)
                return;

            var oldDpi = EventOldDpi;
            var newDpi = EventNewDpi;

            var e = new DpiChangedEventArgs(oldDpi, newDpi);
            uiControl?.RaiseDpiChanged(e);
        }

        public virtual void OnPlatformEventDestroyed()
        {
            Handler?.OnNativeControlDestroyed();
        }

        public virtual void OnPlatformEventTextChanged()
        {
            UIControl?.RaiseHandlerTextChanged(Text);
        }

        public virtual void OnPlatformEventGotFocus()
        {
            UIControl?.RaiseGotFocus(new(EventUIFocusedControl));
        }

        public virtual void OnPlatformEventLostFocus()
        {
            UIControl?.RaiseLostFocus(new(EventUIFocusedControl));
        }

        public virtual void OnPlatformEventDragLeave()
        {
            UIControl?.RaiseDragLeave(EventArgs.Empty);
        }

        public virtual void OnPlatformEventVerticalScrollBarValueChanged()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;

            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Vertical,
                NewValue = GetScrollBarEvtPosition(),
                Type = (ScrollEventType)GetScrollBarEvtKind(),
            };

            uiControl.RaiseScroll(args);
        }

        public virtual void OnPlatformEventHorizontalScrollBarValueChanged()
        {
            var uiControl = UIControl;

            if (uiControl is null)
                return;

            var args = new ScrollEventArgs
            {
                ScrollOrientation = ScrollBarOrientation.Horizontal,
                NewValue = GetScrollBarEvtPosition(),
                Type = (ScrollEventType)GetScrollBarEvtKind(),
            };

            uiControl.RaiseScroll(args);
        }

        public virtual void OnPlatformEventSizeChanged()
        {
            UIControl?.RaiseHandlerSizeChanged(EventArgs.Empty);
        }

        public virtual void OnPlatformEventLocationChanged()
        {
            UIControl?.RaiseContainerLocationChanged(EventArgs.Empty);
        }

        public virtual void OnPlatformEventActivated()
        {
            UIControl?.RaiseActivated(EventArgs.Empty);
        }

        public virtual void OnPlatformEventDeactivated()
        {
            UIControl?.RaiseDeactivated(EventArgs.Empty);
        }

        public virtual void OnPlatformEventBeforeHandleDestroyed()
        {
            UIControl?.RaiseBeforeHandleDestroyed(EventArgs.Empty);
            Handler?.OnBeforeHandleDestroyed();
        }

        public virtual void OnPlatformEventHandleCreated()
        {
            if(UIControl is not null)
            {
                if(UIControl.ReportedBounds != Alternet.Drawing.RectD.MinusOne)
                    Bounds = UIControl.ReportedBounds;
            }

            Handler?.OnHandleCreated();
            UIControl?.RaiseHandleCreated(EventArgs.Empty);
        }

        public virtual void OnPlatformEventHandleDestroyed()
        {
            UIControl?.RaiseHandleDestroyed(EventArgs.Empty);
        }

        public virtual void OnPlatformEventSystemColorsChanged()
        {
            UIControl?.RaiseSystemColorsChanged(EventArgs.Empty);
        }

        public virtual void OnPlatformEventDragDrop(NativeEventArgs<DragEventData> ea)
        {
            if(UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragDrop);
        }

        public virtual void OnPlatformEventDragOver(NativeEventArgs<DragEventData> ea)
        {
            if (UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragOver);
        }

        public virtual void OnPlatformEventDragEnter(NativeEventArgs<DragEventData> ea)
        {
            if (UIControl is not null)
                RaiseDragAndDropEvent(ea, UIControl.RaiseDragEnter);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        private void RaiseDragAndDropEvent(
            Native.NativeEventArgs<Native.DragEventData> e,
            Action<DragEventArgs>? raiseAction)
        {
            if (raiseAction is null || (!UIControl?.AllowDrop ?? false))
                return;

            var data = e.Data;
            var ea = new DragEventArgs(
                new UnmanagedDataObjectAdapter(
                    new Native.UnmanagedDataObject(data.data)),
                new Drawing.PointD(data.mouseClientLocationX, data.mouseClientLocationY),
                (DragDropEffects)data.effect);

            raiseAction?.Invoke(ea);

            e.Result = new IntPtr((int)ea.Effect);
        }

        internal class NonAbstractNativeControl : Native.Control
        {
            public NonAbstractNativeControl()
            {
                SetNativePointer(NativeApi.Control_CreateControl_());
            }
        }
    }
}

    